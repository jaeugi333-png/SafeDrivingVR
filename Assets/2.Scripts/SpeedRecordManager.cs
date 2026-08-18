using UnityEngine;

public class SpeedRecordManager : MonoBehaviour
{
    public static SpeedRecordManager Instance
    {
        get;
        private set;
    }

    [Header("속도 평가 기록")]
    public bool has_measurement;
    public float max_speed;
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
        max_speed = 0f;
        violation_count = 0;
    }

    public void RecordSpeed(float current_speed)
    {
        has_measurement = true;

        if (current_speed > max_speed)
        {
            max_speed = current_speed;
        }
    }

    public void AddViolation()
    {
        violation_count++;

        Debug.Log(
            "과속 위반: " +
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