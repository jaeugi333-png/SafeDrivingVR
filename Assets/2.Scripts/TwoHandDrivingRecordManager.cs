using UnityEngine;

public class TwoHandDrivingRecordManager : MonoBehaviour
{
    public static TwoHandDrivingRecordManager Instance
    {
        get;
        private set;
    }

    [Header("양손 운전 기록")]
    public bool has_measurement;
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
        violation_count = 0;
    }

    public void RecordMeasurement()
    {
        has_measurement = true;
    }

    public void AddViolation()
    {
        violation_count++;

        Debug.Log(
            "양손 운전 위반 기록: "
            + violation_count
            + "회"
        );
    }

    public string GetFinalResult()
    {
        if (!has_measurement)
        {
            return "측정되지 않음";
        }

        if (violation_count == 0)
        {
            return "안전";
        }

        if (violation_count == 1)
        {
            return "주의";
        }

        return "위험";
    }
}