using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    public enum SignalState
    {
        Red,
        Yellow,
        Green
    }

    [Header("현재 신호")]
    public SignalState current_signal = SignalState.Red;

    [Header("전구 오브젝트")]
    public GameObject red_light;
    public GameObject yellow_light;
    public GameObject green_light;

    [Header("전구 조명")]
    public Light red_point_light;
    public Light yellow_point_light;
    public Light green_point_light;

    [Header("신호 시간")]
    public float red_time = 8f;
    public float green_time = 8f;
    public float yellow_time = 2f;

    private float timer = 0f;

    void Start()
    {
        SetSignal(SignalState.Red);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (current_signal == SignalState.Red && timer >= red_time)
        {
            SetSignal(SignalState.Green);
        }
        else if (current_signal == SignalState.Green && timer >= green_time)
        {
            SetSignal(SignalState.Yellow);
        }
        else if (current_signal == SignalState.Yellow && timer >= yellow_time)
        {
            SetSignal(SignalState.Red);
        }
    }

    void SetSignal(SignalState signal)
    {
        current_signal = signal;
        timer = 0f;

        red_light.SetActive(signal == SignalState.Red);
        yellow_light.SetActive(signal == SignalState.Yellow);
        green_light.SetActive(signal == SignalState.Green);

        red_point_light.enabled = signal == SignalState.Red;
        yellow_point_light.enabled = signal == SignalState.Yellow;
        green_point_light.enabled = signal == SignalState.Green;
    }
}