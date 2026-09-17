using UnityEngine;

public class LaneViolationTrigger : MonoBehaviour
{
    [Header("현재 상태")]
    public bool lane_check_inside;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("LaneCheck"))
        {
            return;
        }

        // 같은 Trigger에서 중복 기록 방지
        if (lane_check_inside)
        {
            return;
        }

        lane_check_inside = true;

        LaneDepartureRecordManager manager =
            LaneDepartureRecordManager.Instance;

        if (manager == null)
        {
            Debug.LogWarning(
                "LaneDepartureRecordManager가 없습니다."
            );

            return;
        }

        manager.AddViolation();

        Debug.Log(
            gameObject.name +
            "에 차량 측정 콜라이더가 닿았습니다."
        );
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("LaneCheck"))
        {
            return;
        }

        lane_check_inside = false;
    }
}