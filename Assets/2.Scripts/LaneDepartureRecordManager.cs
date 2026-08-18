using UnityEngine;

public class LaneDepartureRecordManager : MonoBehaviour
{
    public static LaneDepartureRecordManager Instance
    {
        get;
        private set;
    }

    [Header("차선 이탈 기록")]
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
        has_measurement = true;
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
        has_measurement = true;
        violation_count++;

        Debug.Log(
            "차선 이탈 위반: " +
            violation_count +
            "회"
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