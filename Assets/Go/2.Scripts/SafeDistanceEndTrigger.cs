using UnityEngine;

public class SafeDistanceEndTrigger : MonoBehaviour
{
    [Header("안전거리 이벤트")]
    public SafeDistanceEvent safeDistanceEvent;

    [Header("앞차")]
    public GameObject frontCar;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
            return;

        if (frontCar == null || safeDistanceEvent == null)
            return;

        // Trigger에 들어온 오브젝트가 앞차인지 확인
        if (other.transform != frontCar.transform &&
            !other.transform.IsChildOf(frontCar.transform))
            return;

        // 안전거리 평가가 시작되지 않았다면 무시
        if (!safeDistanceEvent.eventStarted)
            return;

        hasTriggered = true;

        safeDistanceEvent.StopSafeDistanceEvent();

        Debug.Log("앞차가 종료 지점에 도착했습니다.");
    }
}