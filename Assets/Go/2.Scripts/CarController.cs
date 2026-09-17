using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("차량 설정")]
    public float accel_power = 25f;
    public float brake_power = 35f;
    public float turn_power = 100f;
    public float max_speed = 70f;

    [Header("입력값")]
    public float accel_input;
    public float brake_input;
    public float steer_input;

    [Header("현재 속도")]
    public float current_speed = 0f;

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
        current_speed += accel_input * accel_power * Time.fixedDeltaTime;
        current_speed -= brake_input * brake_power * Time.fixedDeltaTime;

        if (accel_input <= 0f && brake_input <= 0f)
        {
            current_speed -= 8f * Time.fixedDeltaTime;
        }

        current_speed = Mathf.Clamp(current_speed, 0f, max_speed);

        float speed_ms = current_speed / 3.6f;
        rb.velocity = transform.forward * speed_ms;
    }

    void Turn()
    {
        transform.Rotate(Vector3.up * steer_input * turn_power * Time.fixedDeltaTime);
    }
}