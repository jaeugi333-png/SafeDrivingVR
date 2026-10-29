
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUIManager : MonoBehaviour
{
    [System.Serializable]
    public class ResultRow
    {
        [Header("평가 항목")]
        public TMP_Text categoryText;

        [Header("점수")]
        public TMP_Text scoreText;

        [Header("위반 횟수")]
        public TMP_Text violationText;

        [Header("점수 배경")]
        public Image scoreBackground;
    }

    [Header("종합 결과 UI")]
    public TMP_Text totalScoreText;
    public TMP_Text feedbackText;

    [Header("평가 항목 9개")]
    public ResultRow[] rows = new ResultRow[9];

    [Header("점수별 색상")]
    public Color goodColor =
        new Color32(0, 184, 36, 255);

    public Color normalColor =
        new Color32(242, 181, 0, 255);

    public Color badColor =
        new Color32(236, 52, 52, 255);


    private readonly string[] categoryNames =
    {
        "안전거리 유지",
        "전방주시",
        "신호 준수",
        "와이퍼 작동",
        "양손 운전",
        "방향지시등 사용",
        "차선 유지",
        "제한속도 준수",
        "출발 전 확인"
    };


    private readonly int[] maximumScores =
    {
        10, 10, 15, 10, 10, 15, 10, 15, 5
    };


    private void Start()
    {
        ShowResult();
    }


    public void ShowResult()
    {
        DrivingResultData data = DrivingResultData.Instance;

        if (data == null || !data.hasResult)
        {
            if (totalScoreText != null)
            {
                totalScoreText.text = "결과 없음";
            }

            if (feedbackText != null)
            {
                feedbackText.text = "";
            }

            Debug.LogWarning(
                "저장된 주행 결과가 없습니다."
            );

            return;
        }


        // 총점 표시
        if (totalScoreText != null)
        {
            totalScoreText.text =
                "종합 안전 점수 : " +
                data.totalScore + " / 100점";
        }


        // 항목별 점수
        int[] scores =
        {
            data.safeDistanceScore,
            data.forwardAttentionScore,
            data.trafficScore,
            data.wiperScore,
            data.twoHandScore,
            data.turnSignalScore,
            data.laneDepartureScore,
            data.speedScore,
            data.startSafetyScore
        };


        // 항목별 위반 횟수
        int[] violations =
        {
            data.safeDistanceViolation,
            data.forwardAttentionViolation,
            data.trafficViolation,
            data.wiperViolation,
            data.twoHandViolation,
            data.turnSignalViolation,
            data.laneDepartureViolation,
            data.speedViolation,
            data.startSafetyViolation
        };


        // 9개 평가 항목 표시
        for (int i = 0; i < 9; i++)
        {
            if (rows == null || i >= rows.Length)
            {
                break;
            }

            ResultRow row = rows[i];

            if (row == null)
            {
                continue;
            }


            // 평가 항목명
            if (row.categoryText != null)
            {
                row.categoryText.text =
                    categoryNames[i];
            }


            // 점수
            if (row.scoreText != null)
            {
                row.scoreText.text =
                    scores[i] + "/" +
                    maximumScores[i] + "점";
            }


            // 위반 횟수
            if (row.violationText != null)
            {
                row.violationText.text =
                    violations[i] + "회";
            }


            // 점수에 따른 배경색
            if (row.scoreBackground != null)
            {
                float scoreRatio =
                    (float)scores[i] /
                    maximumScores[i];

                if (scoreRatio >= 0.9f)
                {
                    row.scoreBackground.color =
                        goodColor;
                }
                else if (scoreRatio >= 0.6f)
                {
                    row.scoreBackground.color =
                        normalColor;
                }
                else
                {
                    row.scoreBackground.color =
                        badColor;
                }
            }
        }


        // 종합 피드백
        if (feedbackText != null)
        {
            if (data.totalScore == 100)
            {
                feedbackText.text =
                    "모든 안전수칙을 잘 지켰습니다! " +
                    "현재의 안전운전 습관을 유지해주세요.";
            }
            else
            {
                feedbackText.text =
                    "감점된 안전수칙을 확인하고 " +
                    "다음 주행에서 조금 더 주의해보세요.";
            }
        }

        Debug.Log("안전운전 결과표 출력 완료");
    }
}
