using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("차량 설정")]
    public float accel_power = 15f;
    public float brake_power = 20f;
    public float turn_power = 100f;
    public float max_speed = 50f;

    [Header("현재 속도")]
    public float current_speed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        Turn();

        current_speed = rb.velocity.magnitude * 3.6f;
    }

    void Move()
    {
        // 가속
        if (Input.GetKey(KeyCode.W))
        {
            if (current_speed < max_speed)
            {
                rb.AddForce(transform.forward * accel_power, ForceMode.Acceleration);
            }
        }

        // 브레이크
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * brake_power, ForceMode.Acceleration);
        }
    }

    void Turn()
    {
        float horizontal = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1f;
        }

        transform.Rotate(Vector3.up * horizontal * turn_power * Time.fixedDeltaTime);
    }
}