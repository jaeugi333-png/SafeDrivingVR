
using UnityEngine;

public class SafeDistanceEvent : MonoBehaviour
{
    [Header("플레이어 차량")]
    public Transform playerCar;
    public Transform playerFrontPoint;

    [Header("앞차")]
    public GameObject frontCar;
    public Transform frontCarRearPoint;
    public FrontCarStraightMover frontCarMover;

    [Header("앞차 시작 위치")]
    public Transform spawnPoint;

    [Header("거리 기준")]
    public float dangerDistance = 5f;
    public float cautionDistance = 10f;

    [Header("위반 판정")]
    public float violationHoldTime = 2f;

    [Header("현재 상태")]
    public float currentDistance;
    public bool eventStarted;

    private bool hasTriggered;
    private float dangerHoldTimer;
    private bool violationRecorded;

    private void Start()
    {
        eventStarted = false;
        hasTriggered = false;
        dangerHoldTimer = 0f;
        violationRecorded = false;

        if (frontCar != null)
        {
            frontCar.SetActive(false);
        }
    }

    private void Update()
    {
        if (!eventStarted)
        {
            return;
        }

        if (playerFrontPoint == null || frontCarRearPoint == null)
        {
            return;
        }

        MeasureAndRecordDistance();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
        {
            return;
        }

        if (playerCar == null)
        {
            return;
        }

        if (other.transform.root != playerCar.root)
        {
            return;
        }

        StartSafeDistanceEvent();
    }

    private void StartSafeDistanceEvent()
    {
        if (frontCar == null || spawnPoint == null)
        {
            Debug.LogWarning("앞차 또는 SpawnPoint가 연결되지 않았습니다.");
            return;
        }

        hasTriggered = true;

        frontCar.transform.SetPositionAndRotation(
            spawnPoint.position,
            spawnPoint.rotation
        );

        frontCar.SetActive(true);

        if (frontCarMover != null)
        {
            frontCarMover.StartDriving();
        }

        eventStarted = true;
    }

    private void MeasureAndRecordDistance()
    {
        Vector3 playerPosition = playerFrontPoint.position;
        Vector3 frontCarPosition = frontCarRearPoint.position;

        playerPosition.y = 0f;
        frontCarPosition.y = 0f;

        currentDistance = Vector3.Distance(
            playerPosition,
            frontCarPosition
        );

        SafeDistanceRecordManager manager =
            SafeDistanceRecordManager.Instance;

        if (manager == null)
        {
            return;
        }

        // 현재 안전거리 기록
        manager.RecordDistance(currentDistance);

        // 위험 구간: 5m 미만
        if (currentDistance < dangerDistance)
        {
            manager.AddDangerTime(Time.deltaTime);

            dangerHoldTimer += Time.deltaTime;

            // 위험 구간이 2초 이상 지속되면 위반
            if (dangerHoldTimer >= violationHoldTime &&
                !violationRecorded)
            {
                // 기존 안전거리 위반 기록
                manager.AddViolation();

                // 안전거리 위반 알림 요청
                ShowSafeDistanceNotification();

                violationRecorded = true;
            }
        }

        // 주의 구간: 5m 이상 10m 미만
        else if (currentDistance < cautionDistance)
        {
            manager.AddCautionTime(Time.deltaTime);

            dangerHoldTimer = 0f;
            violationRecorded = false;
        }

        // 안전 구간: 10m 이상
        else
        {
            dangerHoldTimer = 0f;
            violationRecorded = false;
        }
    }

    // 안전거리 위반 알림
    private void ShowSafeDistanceNotification()
    {
        if (ViolationNotificationManager.Instance != null)
        {
            ViolationNotificationManager.Instance.ShowViolation(
                ViolationNotificationManager.ViolationType.SafeDistance
            );
        }
    }

    public void StopSafeDistanceEvent()
    {
        if (!eventStarted)
        {
            return;
        }

        eventStarted = false;

        if (frontCarMover != null)
        {
            frontCarMover.StopDriving();
        }

        if (frontCar != null)
        {
            frontCar.SetActive(false);
        }
    }
}
