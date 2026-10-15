using UnityEngine;

public class TwoHandDrivingChecker : MonoBehaviour
{
    [Header("핸들")]
    public VRSteeringWheel steering_wheel;

    [Header("자동차")]
    public CarController car_controller;

    [Header("위반 기준")]
    public float violation_hold_time = 3f;

    [Header("주행 판정")]
    public float minimum_driving_speed = 0.1f;

    [Header("현재 상태")]
    public bool is_both_hands_driving;
    public bool is_measuring;
    public float non_two_hand_duration;

    private bool violation_recorded;

    private void Start()
    {
        non_two_hand_duration = 0f;
        violation_recorded = false;
    }

    private void Update()
    {
        if (steering_wheel == null)
        {
            return;
        }

        if (TwoHandDrivingRecordManager.Instance == null)
        {
            return;
        }

        CheckTwoHandDriving();
    }

    // 양손 운전 확인
    private void CheckTwoHandDriving()
    {
        is_both_hands_driving =
            steering_wheel.IsBothHandsGripping;

        
        // 차가 움직이는 동안만 평가
        is_measuring = IsCarDriving();

        if (!is_measuring)
        {
            ResetViolationState();
            return;
        }

        TwoHandDrivingRecordManager.Instance
            .RecordMeasurement();

        if (is_both_hands_driving)
        {
            ResetViolationState();
        }
        else
        {
            RecordNonTwoHandDriving();
        }
    }

    // 주행 중인지 확인
    private bool IsCarDriving()
    {
        if (car_controller == null)
        {
            return true;
        }

        Rigidbody car_rigidbody =
            car_controller.GetComponent<Rigidbody>();

        if (car_rigidbody == null)
        {
            return true;
        }

        return car_rigidbody.velocity.magnitude
            >= minimum_driving_speed;
    }

    // 위반 기록
    private void RecordNonTwoHandDriving()
    {
        non_two_hand_duration += Time.deltaTime;

        if (non_two_hand_duration >= violation_hold_time
            && !violation_recorded)
        {
            TwoHandDrivingRecordManager.Instance
                .AddViolation();

            violation_recorded = true;
        }
    }

    // 양손 운전 위반 초기화
    private void ResetViolationState()
    {
        non_two_hand_duration = 0f;
        violation_recorded = false;
    }
}