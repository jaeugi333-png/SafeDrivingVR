using UnityEngine;

public class WeatherZone : MonoBehaviour
{
    [Header("이 구역의 날씨")]
    public WeatherType weather_type;

    [Header("플레이어 차량")]
    public Transform player_car;

    [Header("구역 크기")]
    public Vector3 zone_size = new Vector3(50f, 20f, 100f);

    private bool is_inside = false;

    void Update()
    {
        if (player_car == null || WeatherManager.instance == null)
        {
            return;
        }

        Vector3 local_pos = transform.InverseTransformPoint(player_car.position);

        bool inside =
            Mathf.Abs(local_pos.x) <= zone_size.x / 2f &&
            Mathf.Abs(local_pos.y) <= zone_size.y / 2f &&
            Mathf.Abs(local_pos.z) <= zone_size.z / 2f;

        if (inside && !is_inside)
        {
            is_inside = true;
            WeatherManager.instance.EnterWeatherZone(weather_type);
        }

        if (!inside && is_inside)
        {
            is_inside = false;
            WeatherManager.instance.ExitWeatherZone(weather_type);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, zone_size);
    }
}