using UnityEngine;

public class StartSafetyRecordManager : MonoBehaviour
{
    public static StartSafetyRecordManager Instance
    {
        get;
        private set;
    }

    [Header("출발 전 확인 기록")]
    public bool has_measurement;
    public bool left_checked;
    public bool right_checked;
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
        left_checked = false;
        right_checked = false;
        violation_count = 0;
    }

    public void CheckLeft()
    {
        left_checked = true;
    }

    public void CheckRight()
    {
        right_checked = true;
    }

    public void FinishMeasurement()
    {
        has_measurement = true;

        if (!left_checked || !right_checked)
        {
            violation_count = 1;
        }
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

        return "위반";
    }
}