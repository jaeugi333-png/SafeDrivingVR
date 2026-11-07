
using UnityEngine;

public class TurnSignalCheckZone : MonoBehaviour
{
    public enum RequiredTurn
    {
        Left,
        Right
    }

    [Header("필요한 회전 방향")]
    public RequiredTurn required_turn;

    [Header("플레이어 차량")]
    public Transform player_car;

    [Header("차량 장치")]
    public CarDeviceControl device_controller;

    [Header("평가 상태")]
    public bool has_checked;

    private void Start()
    {
        has_checked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 이미 검사했다면 중복 판정 방지
        if (has_checked)
        {
            return;
        }

        // 필요한 오브젝트가 연결되어 있는지 확인
        if (player_car == null || device_controller == null)
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
        has_checked = true;

        bool is_correct = false;

        // 좌회전 구간
        if (required_turn == RequiredTurn.Left)
        {
            is_correct = device_controller.IsLeftSignalOn();
        }

        // 우회전 구간
        else if (required_turn == RequiredTurn.Right)
        {
            is_correct = device_controller.IsRightSignalOn();
        }

        TurnSignalRecordManager manager =
            TurnSignalRecordManager.Instance;

        if (manager == null)
        {
            return;
        }

        // 방향지시등을 올바르게 사용한 경우
        if (is_correct)
        {
            manager.RecordCorrect();
        }

        // 방향지시등을 사용하지 않았거나 잘못 사용한 경우
        else
        {
            // 기존 방향지시등 위반 기록
            manager.RecordViolation();

            // 방향지시등 위반 알림 요청
            ShowTurnSignalNotification();
        }

        // 방향지시등 자동 해제
        device_controller.TurnOffSignal();
    }

    // 방향지시등 위반 알림
    private void ShowTurnSignalNotification()
    {
        if (ViolationNotificationManager.Instance != null)
        {
            ViolationNotificationManager.Instance.ShowViolation(
                ViolationNotificationManager.ViolationType.TurnSignal
            );
        }
    }
}
