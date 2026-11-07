
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
        // 이미 통과했다면 중복 판정 방지
        if (has_passed)
        {
            return;
        }

        if (player_car == null)
        {
            return;
        }

        if (traffic_light == null)
        {
            return;
        }

        // 플레이어 차량인지 확인
        if (other.transform.root != player_car.root)
        {
            return;
        }

        CheckSignal();
    }

    private void CheckSignal()
    {
        has_passed = true;

        // 통과 당시 신호등 상태 저장
        passed_signal = traffic_light.current_signal;

        TrafficViolationRecordManager manager =
            TrafficViolationRecordManager.Instance;

        if (manager == null)
        {
            return;
        }

        // 기존 신호 통과 기록 및 위반 판정
        manager.RecordPass(passed_signal);

        // 빨간불 또는 노란불 통과 시 알림
        if (passed_signal == TrafficLightController.SignalState.Red ||
            passed_signal == TrafficLightController.SignalState.Yellow)
        {
            ShowTrafficNotification();
        }
    }

    // 신호 위반 알림
    private void ShowTrafficNotification()
    {
        if (ViolationNotificationManager.Instance != null)
        {
            ViolationNotificationManager.Instance.ShowViolation(
                ViolationNotificationManager.ViolationType.Traffic
            );
        }
    }
}
