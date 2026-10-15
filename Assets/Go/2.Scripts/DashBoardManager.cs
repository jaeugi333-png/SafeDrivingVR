using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashBoardManager : MonoBehaviour
{
    [Header("차량")]
    public CarController carController;

    [Header("차량 장치")]
    public CarDeviceControl carDeviceControl;

    [Header("속도 UI")]
    public TMP_Text speedText;

    [Header("방향지시등 UI")]
    public Image leftSignalImage;
    public Image rightSignalImage;

    [Header("기어 UI")]
    public GameObject dGearImage;
    public GameObject rGearImage;

    [Header("속도 색상")]
    public Color normalColor = Color.white;
    public Color overSpeedColor = Color.red;


    private void Update()
    {
        UpdateSpeed();
        UpdateTurnSignal();
        UpdateGear();
    }

    // 속도계
    private void UpdateSpeed()
    {
        if (carController == null || speedText == null)
            return;

        float currentSpeed = carController.current_speed;

        int displaySpeed =
            Mathf.RoundToInt(currentSpeed);

        speedText.text =
            displaySpeed + "<size=60%>km/h</size>";


        // 제한속도 확인
        if (SpeedLimitManager.Instance != null)
        {
            float speedLimit =
                SpeedLimitManager.Instance.current_speed_limit;

            if (currentSpeed > speedLimit)
            {
                speedText.color = overSpeedColor;
            }
            else
            {
                speedText.color = normalColor;
            }
        }
        else
        {
            speedText.color = normalColor;
        }
    }

    // 방향지시등
    private void UpdateTurnSignal()
    {
        if (carDeviceControl == null)
            return;

        bool blink =
            carDeviceControl.IsSignalBlinkOn();

        if (leftSignalImage != null)
        {
            bool leftOn =
                carDeviceControl.IsLeftSignalOn();

            leftSignalImage.gameObject.SetActive(
                leftOn && blink
            );
        }

        if (rightSignalImage != null)
        {
            bool rightOn =
                carDeviceControl.IsRightSignalOn();

            rightSignalImage.gameObject.SetActive(
                rightOn && blink
            );
        }
    }

    private void UpdateGear()
    {
        // D 기어일 때
        dGearImage.SetActive(!carController.is_reverse);

        // R 기어일 때
        rGearImage.SetActive(carController.is_reverse);
    }
}