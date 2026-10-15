using UnityEngine;

public class TrafficLineChecker : MonoBehaviour
{
    [Header("플레이어 차량")]
    public Transform player_car;

    [Header("연결된 신호등")]
    public TrafficLightController traffic_light;

    [Header("현재 기록")]
    public bool has_passed;

    public TrafficLightController.SignalState passed_signal;

    private void Start()
    {
        has_passed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (has_passed)
        {
            return;
        }

        if (player_car == null)
        {
            Debug.LogWarning(
                gameObject.name +
                ": Player Car가 연결되지 않았습니다."
            );

            return;
        }

        if (traffic_light == null)
        {
            Debug.LogWarning(
                gameObject.name +
                ": Traffic Light가 연결되지 않았습니다."
            );

            return;
        }

        if (other.transform.root != player_car.root)
        {
            return;
        }

        CheckSignal();
    }

    private void CheckSignal()
    {
        has_passed = true;

        // 현재 신호등 상태 저장
        passed_signal = traffic_light.current_signal;

        TrafficViolationRecordManager manager =
            TrafficViolationRecordManager.Instance;

        if (manager == null)
        {
            Debug.LogWarning(
                "TrafficViolationRecordManager를 찾을 수 없습니다."
            );

            return;
        }

        manager.RecordPass(passed_signal);

        Debug.Log(
            gameObject.name +
            " 통과 당시 신호: " +
            passed_signal
        );
    }
}