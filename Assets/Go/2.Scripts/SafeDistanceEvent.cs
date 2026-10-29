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
            Debug.LogWarning("Player Car가 연결되지 않았습니다.");
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

        manager.RecordDistance(currentDistance);

        if (currentDistance < dangerDistance)
        {
            manager.AddDangerTime(Time.deltaTime);

            dangerHoldTimer += Time.deltaTime;

            if (dangerHoldTimer >= violationHoldTime &&
                !violationRecorded)
            {
                manager.AddViolation();
                violationRecorded = true;
            }
        }
        else if (currentDistance < cautionDistance)
        {
            manager.AddCautionTime(Time.deltaTime);

            dangerHoldTimer = 0f;
            violationRecorded = false;
        }
        else
        {
            dangerHoldTimer = 0f;
            violationRecorded = false;
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