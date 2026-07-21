using UnityEngine;

public class ForwardAttentionRecordManager : MonoBehaviour
{
    public static ForwardAttentionRecordManager Instance { get; private set; }

    [Header("전방주시 기록")]
    public float totalMeasurementTime;
    public float totalLookAwayTime;
    public float longestLookAwayTime;
    public int violationCount;

    [Header("측정 상태")]
    public bool hasMeasurement;

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
        totalMeasurementTime = 0f;
        totalLookAwayTime = 0f;
        longestLookAwayTime = 0f;
        violationCount = 0;
        hasMeasurement = false;
    }

    public void RecordMeasurement(float deltaTime)
    {
        hasMeasurement = true;
        totalMeasurementTime += deltaTime;
    }

    public void AddLookAwayTime(float deltaTime)
    {
        totalLookAwayTime += deltaTime;
    }

    public void UpdateLongestLookAwayTime(float currentDuration)
    {
        if (currentDuration > longestLookAwayTime)
        {
            longestLookAwayTime = currentDuration;
        }
    }

    public void AddViolation()
    {
        violationCount++;
    }

    public string GetFinalResult()
    {
        if (!hasMeasurement || totalMeasurementTime <= 0f)
        {
            return "측정되지 않음";
        }

        if (violationCount >= 3 || totalLookAwayTime >= 10f)
        {
            return "위험";
        }

        if (violationCount >= 1 || totalLookAwayTime >= 5f)
        {
            return "주의";
        }

        return "안전";
    }
}