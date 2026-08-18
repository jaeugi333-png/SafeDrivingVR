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
        if (evaluation_finished)
        {
            return;
        }

        if (player_car == null ||
            head_camera == null ||
            car_controller == null)
        {
            return;
        }

        CheckHeadDirection();
        CheckStart();
    }

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

        // Unity 기준:
        // 음수 → 왼쪽
        // 양수 → 오른쪽

        if (current_signed_angle <= -check_angle)
        {
            left_checked = true;

            if (StartSafetyRecordManager.Instance != null)
            {
                StartSafetyRecordManager.Instance.CheckLeft();
            }
        }

        if (current_signed_angle >= check_angle)
        {
            right_checked = true;

            if (StartSafetyRecordManager.Instance != null)
            {
                StartSafetyRecordManager.Instance.CheckRight();
            }
        }
    }

    private void CheckStart()
    {
        if (car_controller.current_speed < start_speed)
        {
            return;
        }

        evaluation_finished = true;

        if (StartSafetyRecordManager.Instance != null)
        {
            StartSafetyRecordManager.Instance
                .FinishMeasurement();
        }

        Debug.Log(
            "출발 전 확인 평가 완료 / Left: " +
            left_checked +
            " / Right: " +
            right_checked
        );
    }
}