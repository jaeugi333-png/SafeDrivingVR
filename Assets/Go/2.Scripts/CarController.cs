using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("차량 설정")]
    public float accel_power = 25f;
    public float brake_power = 35f;
    public float turn_power = 100f;
    public float max_speed = 70f;

    [Header("후진 설정")]
    public float max_reverse_speed = 20f;

    [Header("입력값")]
    public float accel_input;
    public float brake_input;
    public float steer_input;

    [Header("현재 속도")]
    public float current_speed = 0f;

    [Header("현재 기어")]
    public bool is_reverse = false;

    [Header("기어 변경 조건")]
    public float gear_change_speed = 0.1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }

    void Move()
    {
        // 현재 기어에 따른 최고 속도
        float current_max_speed =
            is_reverse ? max_reverse_speed : max_speed;

        // 엑셀
        current_speed +=
            accel_input * accel_power * Time.fixedDeltaTime;

        // 브레이크
        current_speed -=
            brake_input * brake_power * Time.fixedDeltaTime;

        // 자연 감속
        if (accel_input <= 0f && brake_input <= 0f)
        {
            current_speed -= 4f * Time.fixedDeltaTime;
        }

        // 속도 제한
        current_speed =
            Mathf.Clamp(current_speed, 0f, current_max_speed);

        // km/h → m/s
        float speed_ms = current_speed / 3.6f;

        // 기어에 따라 이동 방향 변경
        Vector3 move_direction =
            is_reverse ? -transform.forward : transform.forward;

        rb.velocity = move_direction * speed_ms;
    }

    void Turn()
    {
        // 후진할 때는 조향 방향 반전
        float direction = is_reverse ? -1f : 1f;

        transform.Rotate(
            Vector3.up *
            steer_input *
            turn_power *
            direction *
            Time.fixedDeltaTime
        );
    }

    // 기어 변경
    public bool ChangeGear()
    {
        if (current_speed > gear_change_speed ||
            rb.velocity.magnitude > gear_change_speed / 3.6f)
        {
            return false;
        }

        is_reverse = !is_reverse;

        current_speed = 0f;
        return true;
    }
}