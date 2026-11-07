
using UnityEngine;

public class ForwardAttentionChecker : MonoBehaviour
{
    [Header("기준 오브젝트")]
    public Transform playerCar;
    public Transform headCamera;

    [Header("전방주시 판정 기준")]
    [Range(0f, 90f)]
    public float allowedHorizontalAngle = 30f;

    [Range(0f, 90f)]
    public float allowedVerticalAngle = 25f;

    [Header("위반 판정")]
    public float violationHoldTime = 2f;

    [Header("추가 감점 간격")]
    public float additionalPenaltyInterval = 1f;

    [Header("측정 설정")]
    public bool measurementEnabled = true;

    [Header("현재 측정값")]
    public float currentHorizontalAngle;
    public float currentVerticalAngle;
    public float currentLookAwayDuration;
    public bool isLookingForward;
    public bool isLookAwayViolation;

    private bool firstViolationRecorded;
    private float nextPenaltyTime;

    private void Start()
    {
        ResetLookAwayState();
    }

    private void Update()
    {
        if (!measurementEnabled)
        {
            return;
        }

        if (playerCar == null || headCamera == null)
        {
            return;
        }

        CheckForwardAttention();
    }

    private void CheckForwardAttention()
    {
        ForwardAttentionRecordManager manager =
            ForwardAttentionRecordManager.Instance;

        if (manager == null)
        {
            return;
        }

        // 전체 측정 시간 기록
        manager.RecordMeasurement(Time.deltaTime);

        Vector3 carForward = playerCar.forward;
        Vector3 cameraForward = headCamera.forward;

        CalculateHorizontalAngle(
            carForward,
            cameraForward
        );

        CalculateVerticalAngle(cameraForward);

        // 전방주시 여부 판정
        isLookingForward =
            currentHorizontalAngle <= allowedHorizontalAngle &&
            currentVerticalAngle <= allowedVerticalAngle;

        if (isLookingForward)
        {
            ResetLookAwayState();
        }
        else
        {
            RecordLookAway(manager);
        }
    }

    private void CalculateHorizontalAngle(
        Vector3 carForward,
        Vector3 cameraForward
    )
    {
        Vector3 flatCarForward = Vector3.ProjectOnPlane(
            carForward,
            Vector3.up
        ).normalized;

        Vector3 flatCameraForward = Vector3.ProjectOnPlane(
            cameraForward,
            Vector3.up
        ).normalized;

        currentHorizontalAngle = Vector3.Angle(
            flatCarForward,
            flatCameraForward
        );
    }

    private void CalculateVerticalAngle(
        Vector3 cameraForward
    )
    {
        Vector3 flatCameraForward = Vector3.ProjectOnPlane(
            cameraForward,
            Vector3.up
        ).normalized;

        currentVerticalAngle = Vector3.Angle(
            flatCameraForward,
            cameraForward
        );
    }

    private void RecordLookAway(
        ForwardAttentionRecordManager manager
    )
    {
        // 전방을 보지 않은 시간 누적
        currentLookAwayDuration += Time.deltaTime;

        // 누적 이탈 시간 기록
        manager.AddLookAwayTime(Time.deltaTime);

        // 최장 연속 이탈 시간 기록
        manager.UpdateLongestLookAwayTime(
            currentLookAwayDuration
        );

        // 최초 감점 기준 시간
        float holdTime =
            Mathf.Max(0f, violationHoldTime);

        // 추가 감점 간격
        float interval =
            Mathf.Max(0.01f, additionalPenaltyInterval);

        // 2초 이상 전방을 보지 않으면 최초 감점
        if (!firstViolationRecorded &&
            currentLookAwayDuration >= holdTime)
        {
            isLookAwayViolation = true;

            manager.AddViolation();

            // 전방주시 위반 알림 요청
            ShowForwardAttentionNotification();

            firstViolationRecorded = true;

            nextPenaltyTime = holdTime + interval;
        }

        // 이후 1초마다 추가 감점
        if (firstViolationRecorded)
        {
            while (currentLookAwayDuration >= nextPenaltyTime)
            {
                manager.AddViolation();

                // 추가 감점 시에도 알림 요청
                ShowForwardAttentionNotification();

                nextPenaltyTime += interval;
            }
        }
    }

    // 전방주시 위반 알림
    private void ShowForwardAttentionNotification()
    {
        if (ViolationNotificationManager.Instance != null)
        {
            ViolationNotificationManager.Instance.ShowViolation(
                ViolationNotificationManager.ViolationType.ForwardAttention
            );
        }
    }

    private void ResetLookAwayState()
    {
        currentLookAwayDuration = 0f;

        firstViolationRecorded = false;

        nextPenaltyTime = 0f;

        isLookAwayViolation = false;
    }

    public void StartMeasurement()
    {
        measurementEnabled = true;
    }

    public void StopMeasurement()
    {
        measurementEnabled = false;

        ResetLookAwayState();
    }
}
