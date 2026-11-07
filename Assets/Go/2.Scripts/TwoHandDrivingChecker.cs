
using UnityEngine;

public class TwoHandDrivingChecker : MonoBehaviour
{
    [Header("핸들")]
    public VRSteeringWheel steering_wheel;

    [Header("자동차")]
    public CarController car_controller;

    [Header("위반 기준")]
    public float violation_hold_time = 3f;

    [Header("주행 판정 (km/h)")]
    public float minimum_driving_speed = 1f;

    [Header("현재 상태")]
    public bool is_both_hands_driving;
    public bool is_measuring;
    public float non_two_hand_duration;

    [Header("디버그")]
    public bool left_grip;
    public bool right_grip;
    public float current_speed;

    private bool violation_recorded;

    private void Start()
    {
        non_two_hand_duration = 0f;
        violation_recorded = false;
        is_measuring = false;
    }

    private void LateUpdate()
    {
        if (steering_wheel == null ||
            car_controller == null)
        {
            is_measuring = false;
            ResetViolationState();
            return;
        }

        if (TwoHandDrivingRecordManager.Instance == null)
        {
            return;
        }

        CheckTwoHandDriving();
    }

    // 양손 운전 여부 확인
    private void CheckTwoHandDriving()
    {
        // VR 컨트롤러 Grip 상태
        left_grip = steering_wheel.LeftGrip;
        right_grip = steering_wheel.RightGrip;

        is_both_hands_driving =
            left_grip && right_grip;

        // 자동차 현재 속도
        current_speed = Mathf.Abs(
            car_controller.current_speed
        );

        // 주행 중인지 확인
        is_measuring =
            current_speed >= minimum_driving_speed;

        // 정지 상태에서는 평가하지 않음
        if (!is_measuring)
        {
            ResetViolationState();
            return;
        }

        // 기존 주행 측정 기록
        TwoHandDrivingRecordManager.Instance
            .RecordMeasurement();

        // 양손을 모두 잡고 있다면 정상
        if (is_both_hands_driving)
        {
            ResetViolationState();
            return;
        }

        // 한 손 또는 양손을 놓았다면 시간 누적
        RecordNonTwoHandDriving();
    }

    // 양손 운전 위반 기록
    private void RecordNonTwoHandDriving()
    {
        non_two_hand_duration += Time.deltaTime;

        // 기준 시간 이상 지속되었을 때 1회 위반
        if (non_two_hand_duration >= violation_hold_time &&
            !violation_recorded)
        {
            TwoHandDrivingRecordManager.Instance
                .AddViolation();

            ShowTwoHandNotification();

            violation_recorded = true;
        }
    }

    // 양손 운전 위반 알림
    private void ShowTwoHandNotification()
    {
        if (ViolationNotificationManager.Instance != null)
        {
            ViolationNotificationManager.Instance.ShowViolation(
                ViolationNotificationManager.ViolationType.TwoHand
            );
        }
    }

    // 위반 시간 및 상태 초기화
    private void ResetViolationState()
    {
        non_two_hand_duration = 0f;
        violation_recorded = false;
    }
}
