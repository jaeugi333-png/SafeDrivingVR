using UnityEngine;

public class CarDeviceControl : MonoBehaviour
{
    [Header("차량 장치 상태")]
    public bool light_on = false;
    public bool wiper_on = false;
    public bool hazard_on = false;
    public bool menu_on = false;

    [Header("조명 오브젝트")]
    public Light left_head_light;
    public Light right_head_light;

    void Start()
    {
        left_head_light.enabled = false;
        right_head_light.enabled = false;

        light_on = false;
    }

    public void ToggleLight()
    {
        light_on = !light_on;
        ApplyLight();
        Debug.Log("조명: " + light_on);
    }

    void ApplyLight()
    {
        if (left_head_light != null)
        {
            left_head_light.enabled = light_on;
        }

        if (right_head_light != null)
        {
            right_head_light.enabled = light_on;
        }
    }

    public void ToggleWiper()
    {
        wiper_on = !wiper_on;
        Debug.Log("와이퍼: " + wiper_on);
    }

    public void ToggleHazard()
    {
        hazard_on = !hazard_on;
        Debug.Log("비상등: " + hazard_on);
    }

    public void ToggleMenu()
    {
        menu_on = !menu_on;
        Debug.Log("메뉴: " + menu_on);
    }
}