using UnityEngine;

public class WiperController : MonoBehaviour
{
    [Header("와이퍼 피벗")]
    public Transform left_wiper;
    public Transform right_wiper;

    [Header("차량 장치")]
    public CarDeviceControl device_controller;

    [Header("와이퍼 설정")]
    public float min_angle = 0f;
    public float max_angle = 90f;
    public float speed = 3f;

    private float time_value;

    // 와이퍼의 처음 회전값 저장
    private Quaternion left_start_rotation;
    private Quaternion right_start_rotation;


    void Start()
    {
        // 현재 Inspector에서 설정한 회전을 기본 자세로 저장
        if (left_wiper != null)
        {
            left_start_rotation = left_wiper.localRotation;
        }

        if (right_wiper != null)
        {
            right_start_rotation = right_wiper.localRotation;
        }
    }


    void Update()
    {
        if (device_controller == null)
        {
            return;
        }

        if (device_controller.wiper_on)
        {
            time_value += Time.deltaTime * speed;

            float t =
                (Mathf.Sin(time_value) + 1f) / 2f;

            float angle =
                Mathf.Lerp(min_angle, max_angle, t);

            ApplyWiper(angle);
        }
        else
        {
            ApplyWiper(min_angle);
        }
    }


    void ApplyWiper(float angle)
    {
        if (left_wiper != null)
        {
            left_wiper.localRotation =
                left_start_rotation *
                Quaternion.Euler(0f, 0f, -angle);
        }

        if (right_wiper != null)
        {
            right_wiper.localRotation =
                right_start_rotation *
                Quaternion.Euler(0f, 0f, -angle);
        }
    }
}