using UnityEngine;

public class TrafficViolationRecordManager : MonoBehaviour
{
    public static TrafficViolationRecordManager Instance
    {
        get;
        private set;
    }

    [Header("신호 통과 기록")]
    public int total_pass_count;
    public int green_pass_count;
    public int yellow_pass_count;
    public int red_violation_count;

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
        total_pass_count = 0;
        green_pass_count = 0;
        yellow_pass_count = 0;
        red_violation_count = 0;

        has_measurement = false;
    }

    public void RecordPass(
        TrafficLightController.SignalState signal
    )
    {
        has_measurement = true;
        total_pass_count++;

        switch (signal)
        {
            case TrafficLightController.SignalState.Green:
                green_pass_count++;
                Debug.Log("정상 신호 통과");
                break;

            case TrafficLightController.SignalState.Yellow:
                yellow_pass_count++;
                Debug.Log("황색 신호 통과");
                break;

            case TrafficLightController.SignalState.Red:
                red_violation_count++;
                Debug.Log("적색 신호 위반");
                break;
        }
    }

    public string GetFinalResult()
    {
        if (!has_measurement)
        {
            return "측정되지 않음";
        }

        if (red_violation_count >= 2)
        {
            return "위험";
        }

        if (red_violation_count == 1 ||
            yellow_pass_count >= 1)
        {
            return "주의";
        }

        return "안전";
    }
}