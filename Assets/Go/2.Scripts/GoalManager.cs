using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
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

        Debug.Log("GoalZone 도착");

        // 1. 차량 정지 및 조작 비활성화
        StopCar();

        // 2. DrivingScene의 평가 결과 저장
        if (DrivingResultData.Instance != null)
        {
            DrivingResultData.Instance.SaveResult();

            Debug.Log("주행 결과 저장 완료");
        }
        else
        {
            Debug.LogWarning("DrivingResultData.Instance가 없습니다.");
        }

        // 3. MainScene으로 이동
        SceneManager.LoadScene("New Scene");
    }


    private void StopCar()
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
        {
            vr_car_control.enabled = false;
        }

        if (vr_steering_wheel != null)
        {
            vr_steering_wheel.enabled = false;
        }

        if (car_rigidbody != null)
        {
            car_rigidbody.velocity = Vector3.zero;
            car_rigidbody.angularVelocity = Vector3.zero;
        }
    }
}