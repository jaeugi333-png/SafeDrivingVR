using UnityEngine;

public class TurnSignalRecordManager : MonoBehaviour
{
    public static TurnSignalRecordManager Instance { get; private set; }

    [Header("방향지시등 평가 기록")]
    public bool has_measurement;
    public int total_turn_count;
    public int correct_count;
    public int violation_count;

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
        has_measurement = false;
        total_turn_count = 0;
        correct_count = 0;
        violation_count = 0;
    }

    public void RecordCorrect()
    {
        has_measurement = true;
        total_turn_count++;
        correct_count++;
    }

    public void RecordViolation()
    {
        has_measurement = true;
        total_turn_count++;
        violation_count++;
    }

    public string GetFinalResult()
    {
        if (!has_measurement)
            return "측정되지 않음";

        if (violation_count == 0)
            return "안전";

        if (violation_count == 1)
            return "주의";

        return "위험";
    }
}