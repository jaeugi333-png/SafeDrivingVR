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

        turn_signal_state = TurnSignalState.Off;

        ApplyLight();
        ApplySignalLights(false);

        blink_coroutine = StartCoroutine(BlinkSignal());
    }

    // 전조등 켜기 / 끄기
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

    // 와이퍼 켜기 / 끄기
    public void ToggleWiper()
    {
        wiper_on = !wiper_on;

        Debug.Log("와이퍼: " + wiper_on);
    }

    // 왼쪽 방향지시등 켜기 / 끄기
    public void ToggleLeftSignal()
    {
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

    // 오른쪽 방향지시등 켜기 / 끄기
    public void ToggleRightSignal()
    {
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

    // 방향지시등 끄기
    public void TurnOffSignal()
    {
        turn_signal_state = TurnSignalState.Off;

        ApplySignalLights(false);
        UpdateBlinkerSound();

        Debug.Log("방향지시등 꺼짐");
    }

    // 방향지시등 깜빡임
    IEnumerator BlinkSignal()
    {
        while (true)
        {
            blink_on = !blink_on;

            ApplySignalLights(blink_on);

            yield return new WaitForSeconds(blink_interval);
        }
    }

    // 실제 방향지시등 Light 제어
    void ApplySignalLights(bool blink_state)
    {
        bool left_on = false;
        bool right_on = false;

        if (turn_signal_state == TurnSignalState.Left)
        {
            left_on = blink_state;
        }
        else if (turn_signal_state == TurnSignalState.Right)
        {
            right_on = blink_state;
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

    // 왼쪽 방향지시등 상태 확인
    public bool IsLeftSignalOn()
    {
        return turn_signal_state == TurnSignalState.Left;
    }

    // 오른쪽 방향지시등 상태 확인
    public bool IsRightSignalOn()
    {
        return turn_signal_state == TurnSignalState.Right;
    }

    // 방향지시등 사운드 제어
    void UpdateBlinkerSound()
    {
        bool signal_is_active =
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

    public bool IsSignalBlinkOn()
    {
        return blink_on;
    }
}