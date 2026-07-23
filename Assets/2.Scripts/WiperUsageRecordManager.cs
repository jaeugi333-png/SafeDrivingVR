using UnityEngine;

public class WiperUsageRecordManager : MonoBehaviour
{
    public static WiperUsageRecordManager Instance
    {
        get;
        private set;
    }

    [Header("와이퍼 평가 기록")]
    public float total_rain_time;
    public float total_wiper_off_time;
    public float longest_wiper_off_time;
    public int violation_count;

    [Header("측정 상태")]
    public bool has_measurement;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        ResetRecord();
    }

    public void ResetRecord()
    {
        total_rain_time = 0f;
        total_wiper_off_time = 0f;
        longest_wiper_off_time = 0f;
        violation_count = 0;

        has_measurement = false;
    }

    public void RecordRainTime(float delta_time)
    {
        has_measurement = true;
        total_rain_time += delta_time;
    }

    public void RecordWiperOffTime(
        float delta_time,
        float current_off_duration
    )
    {
        total_wiper_off_time += delta_time;

        if (current_off_duration > longest_wiper_off_time)
        {
            longest_wiper_off_time =
                current_off_duration;
        }
    }

    public void AddViolation()
    {
        violation_count++;
    }

    public string GetFinalResult()
    {
        if (!has_measurement)
        {
            return "측정되지 않음";
        }

        if (violation_count >= 2 ||
            total_wiper_off_time >= 10f)
        {
            return "위험";
        }

        if (violation_count == 1 ||
            total_wiper_off_time >= 3f)
        {
            return "주의";
        }

        return "안전";
    }
}