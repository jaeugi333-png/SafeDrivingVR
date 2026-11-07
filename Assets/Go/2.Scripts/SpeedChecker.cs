
using UnityEngine;

public class SpeedChecker : MonoBehaviour
{
    [Header("자동차")]
    public CarController car_controller;

    [Header("과속 판정")]
    public float speed_tolerance = 3f;
    public float violation_hold_time = 3f;
    public float additional_penalty_interval = 1f;

    [Header("현재 상태")]
    public float current_speed;
    public float current_speed_limit;
    public float overspeed_duration;
    public bool is_overspeed;

    private bool first_violation_recorded;
    private float next_penalty_time;

    private void Start()
    {
        ResetOverspeedState();
    }

    private void Update()
    {
        if (car_controller == null)
        {
            return;
        }

        if (SpeedLimitManager.Instance == null)
        {
            return;
        }

        if (SpeedRecordManager.Instance == null)
        {
            return;
        }

        CheckSpeed();
    }

    private void CheckSpeed()
    {
        current_speed =
            car_controller.current_speed;

        current_speed_limit =
            SpeedLimitManager.Instance.current_speed_limit;

        SpeedRecordManager.Instance
            .RecordSpeed(current_speed);

        float violation_speed =
            current_speed_limit + speed_tolerance;

        is_overspeed =
            current_speed > violation_speed;

        if (is_overspeed)
        {
            overspeed_duration += Time.deltaTime;

            // 과속 3초 지속 시 최초 감점
            if (!first_violation_recorded &&
                overspeed_duration >= violation_hold_time)
            {
                AddSpeedPenalty();

                first_violation_recorded = true;

                next_penalty_time =
                    violation_hold_time +
                    Mathf.Max(0.01f, additional_penalty_interval);
            }

            // 이후 1초마다 추가 감점
            if (first_violation_recorded)
            {
                float interval =
                    Mathf.Max(0.01f, additional_penalty_interval);

                while (overspeed_duration >= next_penalty_time)
                {
                    AddSpeedPenalty();

                    next_penalty_time += interval;
                }
            }
        }
        else
        {
            ResetOverspeedState();
        }
    }

    private void AddSpeedPenalty()
    {
        SpeedRecordManager.Instance.AddViolation();

        // 과속 위반 알림 요청
        if (ViolationNotificationManager.Instance != null)
        {
            ViolationNotificationManager.Instance.ShowViolation(
                ViolationNotificationManager.ViolationType.Speed
            );
        }
    }

    private void ResetOverspeedState()
    {
        overspeed_duration = 0f;
        first_violation_recorded = false;
        next_penalty_time = 0f;
        is_overspeed = false;
    }
}
