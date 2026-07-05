using UnityEngine;

public enum WeatherType
{
    Clear,
    Night,
    Rain,
    Fog
}

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager instance;

    [Header("현재 날씨")]
    public WeatherType current_weather = WeatherType.Clear;

    [Header("환경 효과")]
    public GameObject rain_effect;
    public GameObject fog_effect;
    public Light sun_light;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetWeather(WeatherType.Clear);
    }

    public void EnterWeatherZone(WeatherType weather)
    {
        SetWeather(weather);
    }

    public void ExitWeatherZone(WeatherType weather)
    {
        if (current_weather == weather)
        {
            SetWeather(WeatherType.Clear);
        }
    }

    void SetWeather(WeatherType weather)
    {
        current_weather = weather;

        if (rain_effect != null)
        {
            rain_effect.SetActive(weather == WeatherType.Rain);
        }

        if (fog_effect != null)
        {
            fog_effect.SetActive(weather == WeatherType.Fog);
        }

        if (sun_light != null)
        {
            if (weather == WeatherType.Night)
            {
                sun_light.intensity = 0.2f;
            }
            else
            {
                sun_light.intensity = 1f;
            }
        }

        Debug.Log("현재 날씨: " + current_weather);
    }
}