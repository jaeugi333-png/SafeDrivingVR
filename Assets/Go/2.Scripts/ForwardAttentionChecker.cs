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

    [Header("측정 설정")]
    public bool measurementEnabled = true;

    [Header("현재 측정값")]
    public float currentHorizontalAngle;
    public float currentVerticalAngle;
    public float currentLookAwayDuration;
    public bool isLookingForward;
    public bool isLookAwayViolation;

    private bool violationRecorded;

    private void Start()
    {
        currentLookAwayDuration = 0f;
        violationRecorded = false;
        isLookAwayViolation = false;
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
            Debug.LogWarning(
                "ForwardAttentionRecordManager를 찾을 수 없습니다."
            );
            return;
        }

        // 정면주시 여부와 관계없이 측정했다는 사실을 기록
        manager.RecordMeasurement(Time.deltaTime);

        Vector3 carForward = playerCar.forward;
        Vector3 cameraForward = headCamera.forward;

        CalculateHorizontalAngle(carForward, cameraForward);
        CalculateVerticalAngle(cameraForward);

        isLookingForward =
            currentHorizontalAngle <= allowedHorizontalAngle &&
            currentVerticalAngle <= allowedVerticalAngle;

        if (isLookingForward)
        {
            ResetLookAwayState();
        }
        else
        {
            RecordLookAway();
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

    private void CalculateVerticalAngle(Vector3 cameraForward)
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

    private void RecordLookAway()
    {
        currentLookAwayDuration += Time.deltaTime;

        ForwardAttentionRecordManager manager =
            ForwardAttentionRecordManager.Instance;

        if (manager != null)
        {
            manager.AddLookAwayTime(Time.deltaTime);

            manager.UpdateLongestLookAwayTime(
                currentLookAwayDuration
            );
        }

        if (currentLookAwayDuration >= violationHoldTime)
        {
            isLookAwayViolation = true;

            if (!violationRecorded)
            {
                if (manager != null)
                {
                    manager.AddViolation();
                }

                violationRecorded = true;

                Debug.Log("전방주시 위반 1회 기록");
            }
        }
    }

    private void ResetLookAwayState()
    {
        currentLookAwayDuration = 0f;
        violationRecorded = false;
        isLookAwayViolation = false;
    }

    public void StartMeasurement()
    {
        measurementEnabled = true;
    }

    public void StopMeasurement()
    {
        measurementEnabled = false;
        currentLookAwayDuration = 0f;
        violationRecorded = false;
        isLookAwayViolation = false;
    }
}