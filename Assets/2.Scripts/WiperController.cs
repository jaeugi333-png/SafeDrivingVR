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

    void Update()
    {
        if (device_controller == null)
        {
            return;
        }

        if (device_controller.wiper_on)
        {
            time_value += Time.deltaTime * speed;

            float t = (Mathf.Sin(time_value) + 1f) / 2f;
            float angle = Mathf.Lerp(min_angle, max_angle, t);

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
            left_wiper.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (right_wiper != null)
        {
            right_wiper.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}