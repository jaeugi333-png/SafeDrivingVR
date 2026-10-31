
using UnityEngine;

public class WiperUsageChecker : MonoBehaviour
{
    [Header("차량 장치")]
    public CarDeviceControl device_controller;

    [Header("위반 기준")]
    public float violation_hold_time = 3f;

    [Header("추가 감점 간격")]
    public float additional_penalty_interval = 1f;

    [Header("현재 상태")]
    public bool is_raining;
    public bool is_wiper_off;

    [Header("현재 기록")]
    public float current_wiper_off_duration;

    private bool first_violation_recorded;
    private float next_penalty_time;

    private void Start()
    {
        ResetWiperOffState();
    }

    private void Update()
    {
        if (WeatherManager.instance == null)
        {
            return;
        }

        if (device_controller == null)
        {
            return;
        }

        CheckWiperUsage();
    }

    private void CheckWiperUsage()
    {
        is_raining =
            WeatherManager.instance.current_weather
            == WeatherType.Rain;

        // 비가 오지 않으면 미작동 시간 초기화
        if (!is_raining)
        {
            ResetWiperOffState();
            return;
        }

        WiperUsageRecordManager manager =
            WiperUsageRecordManager.Instance;

        if (manager == null)
        {
            return;
        }

        // 비가 오는 전체 시간 기록
        manager.RecordRainTime(Time.deltaTime);

        // 와이퍼 작동 여부 확인
        is_wiper_off = !device_controller.wiper_on;

        if (is_wiper_off)
        {
            RecordWiperOff(manager);
        }
        else
        {
            ResetWiperOffState();
        }
    }

    private void RecordWiperOff(
        WiperUsageRecordManager manager
    )
    {
        // 와이퍼 미작동 시간 누적
        current_wiper_off_duration += Time.deltaTime;

        // 누적 미작동 시간과 최장 미작동 시간 기록
        manager.RecordWiperOffTime(
            Time.deltaTime,
            current_wiper_off_duration
        );

        // 최초 감점 기준 시간
        float hold_time =
            Mathf.Max(0f, violation_hold_time);

        // 추가 감점 간격
        float interval =
            Mathf.Max(0.01f, additional_penalty_interval);

        // 3초 이상 미작동 시 최초 감점
        if (!first_violation_recorded &&
            current_wiper_off_duration >= hold_time)
        {
            manager.AddViolation();

            first_violation_recorded = true;

            next_penalty_time = hold_time + interval;
        }

        // 이후 1초마다 추가 감점
        if (first_violation_recorded)
        {
            while (current_wiper_off_duration >= next_penalty_time)
            {
                manager.AddViolation();

                next_penalty_time += interval;
            }
        }
    }

    private void ResetWiperOffState()
    {
        current_wiper_off_duration = 0f;

        first_violation_recorded = false;

        next_penalty_time = 0f;

        is_wiper_off = false;
    }
}
