using UnityEngine;

public class SafeDistanceRecordManager : MonoBehaviour
{
    public static SafeDistanceRecordManager Instance { get; private set; }

    [Header("안전거리 기록")]
    public float minimumDistance = Mathf.Infinity;
    public float cautionDuration;
    public float dangerDuration;
    public int violationCount;

    [Header("평가 상태")]
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

    public void ResetRecord()
    {
        minimumDistance = Mathf.Infinity;
        cautionDuration = 0f;
        dangerDuration = 0f;
        violationCount = 0;
        hasMeasurement = false;
    }

    public void RecordDistance(float distance)
    {
        hasMeasurement = true;

        if (distance < minimumDistance)
        {
            minimumDistance = distance;
        }
    }

    public void AddCautionTime(float time)
    {
        cautionDuration += time;
    }

    public void AddDangerTime(float time)
    {
        dangerDuration += time;
    }

    public void AddViolation()
    {
        violationCount++;
    }

    public string GetFinalResult()
    {
        if (!hasMeasurement)
        {
            return "Unmeasured";
        }

        if (violationCount >= 2 || dangerDuration >= 5f)
        {
            return "Dangerous";
        }

        if (violationCount == 1 || cautionDuration >= 5f)
        {
            return "Attention";
        }

        return "Safe";
    }
}