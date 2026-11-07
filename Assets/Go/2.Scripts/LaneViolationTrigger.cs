
using UnityEngine;

public class LaneViolationTrigger : MonoBehaviour
{
    [Header("현재 상태")]
    public bool lane_check_inside;

    private void OnTriggerEnter(Collider other)
    {
        // LaneCheck 태그가 아니라면 무시
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
            return;
        }

        // 기존 차선 이탈 위반 기록
        manager.AddViolation();

        // 차선 이탈 위반 알림 요청
        ShowLaneDepartureNotification();
    }

    // 차선 이탈 위반 알림
    private void ShowLaneDepartureNotification()
    {
        if (ViolationNotificationManager.Instance != null)
        {
            ViolationNotificationManager.Instance.ShowViolation(
                ViolationNotificationManager.ViolationType.LaneDeparture
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // LaneCheck 태그가 아니라면 무시
        if (!other.CompareTag("LaneCheck"))
        {
            return;
        }

        lane_check_inside = false;
    }
}
