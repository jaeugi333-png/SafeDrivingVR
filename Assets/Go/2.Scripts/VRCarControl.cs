using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class VRCarControl : MonoBehaviour
{
    [Header("자동차")]
    public CarController car_controller;

    [Header("차량 장치")]
    public CarDeviceControl device_controller;

    [Header("방향지시등 입력 설정")]
    public float signal_input_threshold = 0.7f;
    public float signal_reset_threshold = 0.3f;

    [Header("기어 설정")]
    public bool is_reverse = false;

    [Header("기어 변경 조건")]
    public float gear_change_speed = 0.1f;

    [Header("디버그 텍스트")]
    public TMP_Text debug_text;

    private InputDevice left_device;
    private InputDevice right_device;

    private bool prev_x;
    private bool prev_y;
    private bool prev_a;

    private bool signal_input_ready = true;

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

        float accel = GetRightLever();
        float brake = GetLeftLever();

        if (car_controller != null)
        {
            car_controller.accel_input = accel;
            car_controller.brake_input = brake;
        }

        CheckButtons();
        CheckTurnSignalInput();

        if (debug_text != null &&
            device_controller != null)
        {
            Vector2 left_axis = GetLeftAxis();

            debug_text.text = "Hello";
        }
    }

    void CheckButtons()
    {
        if (device_controller == null)
        {
            return;
        }

        bool x;
        bool y;
        bool a;
        bool b;

        left_device.TryGetFeatureValue(
            CommonUsages.primaryButton,
            out x
        );

        left_device.TryGetFeatureValue(
            CommonUsages.secondaryButton,
            out y
        );

        right_device.TryGetFeatureValue(
            CommonUsages.primaryButton,
            out a
        );

        right_device.TryGetFeatureValue(
            CommonUsages.secondaryButton,
            out b
        );

        if (x && !prev_x)
        {
            device_controller.ToggleLight();
        }

        if (y && !prev_y)
        {
            device_controller.ToggleWiper();
        }

        if (a && !prev_a)
        {
            car_controller.ChangeGear();
        }

        prev_x = x;
        prev_y = y;
        prev_a = a;
    }

    void CheckTurnSignalInput()
    {
        if (device_controller == null)
        {
            return;
        }

        Vector2 left_axis = GetLeftAxis();

        /*
         * 조이스틱이 중립에 가까워지면
         * 다음 방향지시등 입력을 받을 수 있게 한다.
         */
        if (Mathf.Abs(left_axis.x) <= signal_reset_threshold)
        {
            signal_input_ready = true;
        }

        if (!signal_input_ready)
        {
            return;
        }

        if (left_axis.x <= -signal_input_threshold)
        {
            device_controller.ToggleLeftSignal();
            signal_input_ready = false;
        }
        else if (left_axis.x >= signal_input_threshold)
        {
            device_controller.ToggleRightSignal();
            signal_input_ready = false;
        }
    }

    Vector2 GetLeftAxis()
    {
        Vector2 axis;

        if (left_device.TryGetFeatureValue(
            CommonUsages.primary2DAxis,
            out axis
        ))
        {
            return axis;
        }

        return Vector2.zero;
    }

    float GetRightLever()
    {
        Vector2 axis;

        if (right_device.TryGetFeatureValue(
            CommonUsages.primary2DAxis,
            out axis
        ))
        {
            return Mathf.Clamp01(axis.y);
        }

        return 0f;
    }

    float GetLeftLever()
    {
        Vector2 axis;

        if (left_device.TryGetFeatureValue(
            CommonUsages.primary2DAxis,
            out axis
        ))
        {
            return Mathf.Clamp01(axis.y);
        }

        return 0f;
    }
}