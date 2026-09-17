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

    private void CheckTwoHandDriving()
    {
        is_both_hands_driving =
            steering_wheel.IsBothHandsGripping;

        /*
         * 차가 실제로 움직이는 동안만 평가한다.
         * 시작 화면에서 컨트롤러를 잡지 않은 상태까지
         * 위반으로 기록되는 것을 방지한다.
         */
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

    private bool IsCarDriving()
    {
        if (car_controller == null)
        {
            // 차량 속도를 연결하지 않은 경우에는
            // 항상 측정
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

    private void ResetViolationState()
    {
        non_two_hand_duration = 0f;
        violation_recorded = false;
    }
}