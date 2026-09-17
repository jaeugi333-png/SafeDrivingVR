using System.Collections;
using UnityEngine;

public class CarDeviceControl : MonoBehaviour
{
    public enum TurnSignalState
    {
        Off,
        Left,
        Right
    }

    [Header("차량 장치 상태")]
    public bool light_on = false;
    public bool wiper_on = false;
    public bool hazard_on = false;
    public bool menu_on = false;

    [Header("방향지시등 상태")]
    public TurnSignalState turn_signal_state = TurnSignalState.Off;

    [Header("전조등 오브젝트")]
    public Light left_head_light;
    public Light right_head_light;

    [Header("방향지시등 오브젝트")]
    public Light left_signal_light;
    public Light right_signal_light;

    [Header("방향지시등 설정")]
    public float blink_interval = 0.5f;

    [Header("방향지시등 사운드")]
    public AudioSource signal_audio_source;

    private bool blink_on = false;
    private Coroutine blink_coroutine;

    void Start()
    {
        light_on = false;
        wiper_on = false;
        hazard_on = false;
        menu_on = false;

        turn_signal_state = TurnSignalState.Off;

        ApplyLight();
        ApplySignalLights(false);

        blink_coroutine = StartCoroutine(BlinkSignal());
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

        if (hazard_on)
        {
            turn_signal_state = TurnSignalState.Off;
        }

        Debug.Log("비상등: " + hazard_on);
    }

    public void ToggleMenu()
    {
        menu_on = !menu_on;

        Debug.Log("메뉴: " + menu_on);
    }

    public void ToggleLeftSignal()
    {
        hazard_on = false;

        if (turn_signal_state == TurnSignalState.Left)
        {
            turn_signal_state = TurnSignalState.Off;
        }
        else
        {
            turn_signal_state = TurnSignalState.Left;
        }

        UpdateBlinkerSound();

        Debug.Log("방향지시등: " + turn_signal_state);
    }

    public void ToggleRightSignal()
    {
        hazard_on = false;

        if (turn_signal_state == TurnSignalState.Right)
        {
            turn_signal_state = TurnSignalState.Off;
        }
        else
        {
            turn_signal_state = TurnSignalState.Right;
        }

        UpdateBlinkerSound();

        Debug.Log("방향지시등: " + turn_signal_state);
    }

    public void TurnOffSignal()
    {
        turn_signal_state = TurnSignalState.Off;
        hazard_on = false;

        ApplySignalLights(false);

        Debug.Log("방향지시등 꺼짐");
    }

    IEnumerator BlinkSignal()
    {
        while (true)
        {
            blink_on = !blink_on;

            ApplySignalLights(blink_on);

            yield return new WaitForSeconds(blink_interval);
        }
    }

    void ApplySignalLights(bool blink_state)
    {
        bool left_on = false;
        bool right_on = false;

        if (hazard_on)
        {
            left_on = blink_state;
            right_on = blink_state;
        }
        else
        {
            if (turn_signal_state == TurnSignalState.Left)
            {
                left_on = blink_state;
            }
            else if (turn_signal_state == TurnSignalState.Right)
            {
                right_on = blink_state;
            }
        }

        if (left_signal_light != null)
        {
            left_signal_light.enabled = left_on;
        }

        if (right_signal_light != null)
        {
            right_signal_light.enabled = right_on;
        }
    }

    public bool IsLeftSignalOn()
    {
        return !hazard_on &&
               turn_signal_state == TurnSignalState.Left;
    }

    public bool IsRightSignalOn()
    {
        return !hazard_on &&
               turn_signal_state == TurnSignalState.Right;
    }

    void UpdateBlinkerSound()
    {
        bool signal_is_active =
            hazard_on ||
            turn_signal_state != TurnSignalState.Off;

        if (signal_audio_source == null)
        {
            return;
        }

        if (signal_is_active)
        {
            if (!signal_audio_source.isPlaying)
            {
                signal_audio_source.Play();
            }
        }
        else
        {
            if (signal_audio_source.isPlaying)
            {
                signal_audio_source.Stop();
            }
        }
    }
}