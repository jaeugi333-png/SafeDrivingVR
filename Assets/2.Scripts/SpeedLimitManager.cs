using UnityEngine;

public class SpeedLimitManager : MonoBehaviour
{
    public static SpeedLimitManager Instance { get; private set; }

    [Header("제한속도")]
    public float normal_speed_limit = 50f;
    public float current_speed_limit = 50f;

    [Header("현재 상태")]
    public bool is_school_zone = false;

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
        SetNormalRoad();
    }

    public void EnterSchoolZone()
    {
        is_school_zone = true;
        current_speed_limit = 30f;

        Debug.Log("스쿨존 진입 - 제한속도 30km/h");
    }

    public void ExitSchoolZone()
    {
        SetNormalRoad();

        Debug.Log("스쿨존 이탈 - 제한속도 50km/h");
    }

    private void SetNormalRoad()
    {
        is_school_zone = false;
        current_speed_limit = normal_speed_limit;
    }
}