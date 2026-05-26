using UnityEngine;
using UnityEngine.XR;

public class VRSteeringWheel : MonoBehaviour
{
    [Header("자동차")]
    public CarController car_controller;

    [Header("핸들 설정")]
    public float max_wheel_angle = 90f;

    [Header("현재 핸들 값")]
    public float steer_input;
    public float wheel_angle;

    private InputDevice left_device;
    private InputDevice right_device;

    private Vector3 left_pos;
    private Vector3 right_pos;

    private bool left_grip;
    private bool right_grip;

    void Start()
    {
        left_device =
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        right_device =
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        if (!left_device.isValid)
        {
            left_device =
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }

        if (!right_device.isValid)
        {
            right_device =
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        left_device.TryGetFeatureValue(
            CommonUsages.gripButton,
            out left_grip);

        right_device.TryGetFeatureValue(
            CommonUsages.gripButton,
            out right_grip);

        if (left_grip && right_grip)
        {
            UpdateWheelAngle();
        }

        steer_input = wheel_angle / max_wheel_angle;

        if (car_controller != null)
        {
            car_controller.steer_input = steer_input;
        }

        transform.localRotation =
            Quaternion.Euler(
                0f,
                0f,
                -wheel_angle);
    }

    void UpdateWheelAngle()
    {
        bool left_ok =
            left_device.TryGetFeatureValue(
                CommonUsages.devicePosition,
                out left_pos);

        bool right_ok =
            right_device.TryGetFeatureValue(
                CommonUsages.devicePosition,
                out right_pos);

        if (!left_ok || !right_ok)
        {
            return;
        }

        Vector3 dir = right_pos - left_pos;

        float angle =
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        wheel_angle =
            -Mathf.Clamp(
                angle,
                -max_wheel_angle,
                max_wheel_angle);
    }
}