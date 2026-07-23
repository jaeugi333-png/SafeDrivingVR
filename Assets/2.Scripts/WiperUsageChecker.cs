using UnityEngine;

public class WiperUsageChecker : MonoBehaviour
{
    [Header("차량 장치")]
    public CarDeviceControl device_controller;

    [Header("위반 기준")]
    public float violation_hold_time = 3f;

    [Header("현재 상태")]
    public bool is_raining;
    public bool is_wiper_off;

    [Header("현재 기록")]
    public float current_wiper_off_duration;

    private bool violation_recorded;

    private void Start()
    {
        current_wiper_off_duration = 0f;
        violation_recorded = false;
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

        // 비가 오는 동안 와이퍼 측정
        manager.RecordRainTime(Time.deltaTime);

        is_wiper_off = !device_controller.wiper_on;

        if (is_wiper_off)
        {
            RecordWiperOff();
        }
        else
        {
            ResetWiperOffState();
        }
    }

    private void RecordWiperOff()
    {
        current_wiper_off_duration += Time.deltaTime;

        WiperUsageRecordManager manager =
            WiperUsageRecordManager.Instance;

        if (manager == null)
        {
            return;
        }

        manager.RecordWiperOffTime(
            Time.deltaTime,
            current_wiper_off_duration
        );

        if (current_wiper_off_duration
            >= violation_hold_time)
        {
            if (!violation_recorded)
            {
                manager.AddViolation();
                violation_recorded = true;

                Debug.Log(
                    "비 구간 와이퍼 미사용 위반 1회 기록"
                );
            }
        }
    }

    private void ResetWiperOffState()
    {
        current_wiper_off_duration = 0f;
        violation_recorded = false;
        is_wiper_off = false;
    }
}