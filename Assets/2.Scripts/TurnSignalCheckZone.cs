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
        if (has_checked)
            return;

        if (player_car == null || device_controller == null)
            return;

        if (other.transform.root != player_car.root)
            return;

        CheckSignal();
    }

    private void CheckSignal()
    {
        has_checked = true;

        bool is_correct = false;

        if (required_turn == RequiredTurn.Left)
        {
            is_correct = device_controller.IsLeftSignalOn();
        }
        else if (required_turn == RequiredTurn.Right)
        {
            is_correct = device_controller.IsRightSignalOn();
        }

        if (TurnSignalRecordManager.Instance == null)
            return;

        if (is_correct)
        {
            TurnSignalRecordManager.Instance.RecordCorrect();
            Debug.Log("방향지시등 정상 사용");
        }
        else
        {
            TurnSignalRecordManager.Instance.RecordViolation();
            Debug.Log("방향지시등 미사용 또는 잘못된 방향");
        }

        device_controller.TurnOffSignal();
    }
}