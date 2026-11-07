
using UnityEngine;

public class StartSafetyChecker : MonoBehaviour
{
    [Header("기준 오브젝트")]
    public Transform player_car;
    public Transform head_camera;

    [Header("자동차")]
    public CarController car_controller;

    [Header("확인 기준")]
    public float check_angle = 45f;

    [Header("출발 판정 속도")]
    public float start_speed = 1f;

    [Header("현재 상태")]
    public float current_signed_angle;
    public bool left_checked;
    public bool right_checked;
    public bool evaluation_finished;

    private void Start()
    {
        left_checked = false;
        right_checked = false;
        evaluation_finished = false;
    }

    private void Update()
    {
        // 이미 출발 전 확인 평가가 끝났다면 종료
        if (evaluation_finished)
        {
            return;
        }

        // 필요한 오브젝트가 연결되어 있는지 확인
        if (player_car == null ||
            head_camera == null ||
            car_controller == null)
        {
            return;
        }

        CheckHeadDirection();
        CheckStart();
    }

    // 출발 전 좌우 확인 여부 검사
    private void CheckHeadDirection()
    {
        Vector3 car_forward =
            Vector3.ProjectOnPlane(
                player_car.forward,
                Vector3.up
            ).normalized;

        Vector3 head_forward =
            Vector3.ProjectOnPlane(
                head_camera.forward,
                Vector3.up
            ).normalized;

        current_signed_angle =
            Vector3.SignedAngle(
                car_forward,
                head_forward,
                Vector3.up
            );

        // 음수 == 왼쪽
        // 양수 == 오른쪽

        // 왼쪽 45도 이상 확인
        if (current_signed_angle <= -check_angle)
        {
            left_checked = true;

            if (StartSafetyRecordManager.Instance != null)
            {
                StartSafetyRecordManager.Instance.CheckLeft();
            }
        }

        // 오른쪽 45도 이상 확인
        if (current_signed_angle >= check_angle)
        {
            right_checked = true;

            if (StartSafetyRecordManager.Instance != null)
            {
                StartSafetyRecordManager.Instance.CheckRight();
            }
        }
    }

    // 출발 여부 확인
    private void CheckStart()
    {
        // 출발 속도에 도달하지 않았다면 평가 계속
        if (car_controller.current_speed < start_speed)
        {
            return;
        }

        // 출발 전 확인 평가 종료
        evaluation_finished = true;

        // 기존 평가 결과 기록
        if (StartSafetyRecordManager.Instance != null)
        {
            StartSafetyRecordManager.Instance
                .FinishMeasurement();
        }

        // 좌우 중 하나라도 확인하지 않았다면 위반 알림
        if (!left_checked || !right_checked)
        {
            ShowStartSafetyNotification();
        }
    }

    // 출발 전 확인 위반 알림
    private void ShowStartSafetyNotification()
    {
        if (ViolationNotificationManager.Instance != null)
        {
            ViolationNotificationManager.Instance.ShowViolation(
                ViolationNotificationManager.ViolationType.StartSafety
            );
        }
    }
}
