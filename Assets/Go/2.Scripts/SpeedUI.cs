using UnityEngine;
using TMPro;

public class SpeedUI : MonoBehaviour
{
    [Header("차량")]
    public CarController car_controller;

    [Header("속도 텍스트")]
    public TMP_Text speed_text;

    void Update()
    {
        if (car_controller == null || speed_text == null)
        {
            return;
        }

        speed_text.text =
            "Current Speed : " +
            car_controller.current_speed.ToString("F0") +
            " km/h";
    }
}