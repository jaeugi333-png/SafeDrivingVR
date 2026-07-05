using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [Header("종료 UI")]
    public GameObject finish_image;

    [Header("멈출 대상")]
    public CarController car_controller;
    public VRCarControl vr_car_control;
    public VRSteeringWheel vr_steering_wheel;
    public Rigidbody car_rigidbody;

    private bool is_finish = false;

    private void OnTriggerEnter(Collider other)
    {
        if (is_finish)
            return;

        if (!other.CompareTag("Player"))
            return;

        is_finish = true;

        if (finish_image != null)
            finish_image.SetActive(true);

        StopCar();

        Debug.Log("GoalZone 도착: 차량 정지 + 조작 비활성화");
    }

    void StopCar()
    {
        if (car_controller != null)
        {
            car_controller.accel_input = 0f;
            car_controller.brake_input = 0f;
            car_controller.steer_input = 0f;
            car_controller.current_speed = 0f;
            car_controller.enabled = false;
        }

        if (vr_car_control != null)
            vr_car_control.enabled = false;

        if (vr_steering_wheel != null)
            vr_steering_wheel.enabled = false;

        if (car_rigidbody != null)
        {
            car_rigidbody.velocity = Vector3.zero;
            car_rigidbody.angularVelocity = Vector3.zero;
        }
    }
}