using UnityEngine;
using TMPro;

public class ResultUIManager : MonoBehaviour
{
    [Header("결과 UI")]
    public TMP_Text totalScoreText;
    public TMP_Text feedbackText;

    void Start()
    {
        ShowResult();
    }

    void ShowResult()
    {
        if (DrivingResultData.Instance == null)
        {
            totalScoreText.text = "결과 없음";
            feedbackText.text = "";
            return;
        }

        DrivingResultData data = DrivingResultData.Instance;

        // 총점 표시
        totalScoreText.text =
            "총점: " +
            data.totalScore + "점 / 100점";


        // 항목별 점수 표시
        string feedback = "";

        feedback +=
            "안전거리 유지: " +
            data.safeDistanceScore + "/10점\n\n";

        feedback +=
            "전방주시: " +
            data.forwardAttentionScore + "/10점\n\n";

        feedback +=
            "신호 준수: " +
            data.trafficScore + "/15점\n\n";

        feedback +=
            "와이퍼 사용: " +
            data.wiperScore + "/10점\n\n";

        feedback +=
            "양손 운전: " +
            data.twoHandScore + "/10점\n\n";

        feedback +=
            "방향지시등 사용: " +
            data.turnSignalScore + "/15점\n\n";

        feedback +=
            "차선 유지: " +
            data.laneDepartureScore + "/10점\n\n";

        feedback +=
            "제한속도 준수: " +
            data.speedScore + "/15점\n\n";

        feedback +=
            "출발 전 확인: " +
            data.startSafetyScore + "/5점\n\n";


        // 마지막 전체 피드백
        feedback += "\n";

        if (data.totalScore == 100)
        {
            feedback +=
                "모든 안전수칙을 잘 지켰습니다!\n" +
                "현재의 안전운전 습관을 유지해주세요.";
        }
        else
        {
            feedback +=
                "감점된 안전수칙을 확인하고 " +
                "다음 주행에서 조금 더 주의해보세요.";
        }

        feedbackText.text = feedback;
    }
}