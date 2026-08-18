using UnityEngine;

public class SchoolZone : MonoBehaviour
{
    [Header("플레이어 차량")]
    public Transform player_car;

    private void OnTriggerEnter(Collider other)
    {
        if (player_car == null)
        {
            return;
        }

        if (other.transform.root != player_car.root)
        {
            return;
        }

        if (SpeedLimitManager.Instance != null)
        {
            SpeedLimitManager.Instance.EnterSchoolZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player_car == null)
        {
            return;
        }

        if (other.transform.root != player_car.root)
        {
            return;
        }

        if (SpeedLimitManager.Instance != null)
        {
            SpeedLimitManager.Instance.ExitSchoolZone();
        }
    }
}