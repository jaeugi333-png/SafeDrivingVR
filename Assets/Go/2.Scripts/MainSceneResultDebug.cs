using UnityEngine;
using TMPro;

public class MainSceneResultDebug : MonoBehaviour
{
    public TMP_Text debugText;

    private void Update()
    {
        if (debugText == null)
            return;

        if (DrivingResultData.Instance == null)
        {
            debugText.text =
                "DrivingResultData 없음";

            return;
        }

        DrivingResultData data =
            DrivingResultData.Instance;

        debugText.text =
            "Has Result: " + data.hasResult + "\n\n" +

            "안전거리 위반: " +
            data.safeDistanceViolation + "\n" +

            "전방주시 위반: " +
            data.forwardAttentionViolation + "\n" +

            "신호 위반: " +
            data.trafficViolation + "\n" +

            "와이퍼 위반: " +
            data.wiperViolation + "\n" +

            "양손 운전 위반: " +
            data.twoHandViolation + "\n" +

            "방향지시등 위반: " +
            data.turnSignalViolation + "\n" +

            "차선 이탈: " +
            data.laneDepartureViolation + "\n" +

            "과속 위반: " +
            data.speedViolation + "\n" +

            "출발 전 확인 위반: " +
            data.startSafetyViolation;
    }
}