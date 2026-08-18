using UnityEngine;

public class SpeedChecker : MonoBehaviour
{
    [Header("자동차")]
    public CarController car_controller;

    [Header("과속 판정")]
    public float speed_tolerance = 3f;
    public float violation_hold_time = 3f;

    [Header("현재 상태")]
    public float current_speed;
    public float current_speed_limit;
    public float overspeed_duration;
    public bool is_overspeed;

    private bool violation_recorded;

    private void Start()
    {
        overspeed_duration = 0f;
        violation_recorded = false;
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

            if (overspeed_duration >= violation_hold_time &&
                !violation_recorded)
            {
                SpeedRecordManager.Instance
                    .AddViolation();

                violation_recorded = true;

                Debug.Log(
                    "과속 위반 기록 / 현재속도: " +
                    current_speed.ToString("F1") +
                    " / 제한속도: " +
                    current_speed_limit.ToString("F0")
                );
            }
        }
        else
        {
            ResetOverspeedState();
        }
    }

    private void ResetOverspeedState()
    {
        overspeed_duration = 0f;
        violation_recorded = false;
    }
}