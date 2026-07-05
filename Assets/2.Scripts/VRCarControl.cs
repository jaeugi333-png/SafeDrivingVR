using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class VRCarControl : MonoBehaviour
{
    [Header("자동차")]
    public CarController car_controller;

    [Header("차량 장치")]
    public CarDeviceControl device_controller;

    [Header("디버그 텍스트")]
    public TMP_Text debug_text;

    private InputDevice left_device;
    private InputDevice right_device;

    private bool prev_x;
    private bool prev_y;
    private bool prev_a;
    private bool prev_b;

    void Start()
    {
        left_device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        right_device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        if (!left_device.isValid)
        {
            left_device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }

        if (!right_device.isValid)
        {
            right_device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        float accel = GetRightLever();
        float brake = GetLeftLever();

        if (car_controller != null)
        {
            car_controller.accel_input = accel;
            car_controller.brake_input = brake;
        }

        CheckButtons();

        if (debug_text != null && device_controller != null)
        {
            debug_text.text =
                "Left Valid : " + left_device.isValid + "\n" +
                "Right Valid : " + right_device.isValid + "\n" +
                "Accel : " + accel.ToString("F2") + "\n" +
                "Brake : " + brake.ToString("F2") + "\n" +
                "Weather : " + WeatherManager.instance.current_weather + "\n" +
                "RainEffect : " + WeatherManager.instance.rain_effect.activeSelf + "\n" +
                "Light(X) : " + device_controller.light_on + "\n" +
                "Wiper(Y) : " + device_controller.wiper_on + "\n" +
                "Hazard(A) : " + device_controller.hazard_on + "\n" +
                "Menu(B) : " + device_controller.menu_on;
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

        left_device.TryGetFeatureValue(CommonUsages.primaryButton, out x);      // X
        left_device.TryGetFeatureValue(CommonUsages.secondaryButton, out y);    // Y
        right_device.TryGetFeatureValue(CommonUsages.primaryButton, out a);     // A
        right_device.TryGetFeatureValue(CommonUsages.secondaryButton, out b);   // B

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
            device_controller.ToggleHazard();
        }

        if (b && !prev_b)
        {
            device_controller.ToggleMenu();
        }

        prev_x = x;
        prev_y = y;
        prev_a = a;
        prev_b = b;
    }

    float GetRightLever()
    {
        Vector2 axis;

        if (right_device.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis))
        {
            return Mathf.Clamp01(axis.y);
        }

        return 0f;
    }

    float GetLeftLever()
    {
        Vector2 axis;

        if (left_device.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis))
        {
            return Mathf.Clamp01(axis.y);
        }

        return 0f;
    }
}