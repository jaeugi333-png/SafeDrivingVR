using UnityEngine;
using TMPro;

public class ResultUIManager : MonoBehaviour
{
    [Header("결과 UI")]
    public TMP_Text safeDistanceResultText;
    public TMP_Text forwardAttentionResultText;

    public void ShowResult()
    {
        ShowSafeDistanceResult();
        ShowForwardAttentionResult();
    }

    private void ShowSafeDistanceResult()
    {
        SafeDistanceRecordManager manager =
            SafeDistanceRecordManager.Instance;

        if (manager == null || safeDistanceResultText == null)
        {
            return;
        }

        string minimumDistanceText =
            manager.hasMeasurement
                ? $"{manager.minimumDistance:F1}m"
                : "측정되지 않음";

        safeDistanceResultText.text =
            $"[안전거리 평가]\n" +
            $"최종 판정: {manager.GetFinalResult()}\n" +
            $"최소 거리: {minimumDistanceText}\n" +
            $"주의 상태 유지: {manager.cautionDuration:F1}초\n" +
            $"위험 상태 유지: {manager.dangerDuration:F1}초\n" +
            $"안전거리 위반: {manager.violationCount}회";
    }

    private void ShowForwardAttentionResult()
    {
        ForwardAttentionRecordManager manager =
            ForwardAttentionRecordManager.Instance;

        if (manager == null ||
            forwardAttentionResultText == null)
        {
            return;
        }

        forwardAttentionResultText.text =
            $"[전방주시 평가]\n" +
            $"최종 판정: {manager.GetFinalResult()}\n" +
            $"전방주시 이탈 시간: {manager.totalLookAwayTime:F1}초\n" +
            $"최장 이탈 시간: {manager.longestLookAwayTime:F1}초\n" +
            $"전방주시 위반: {manager.violationCount}회";
    }
}