using UnityEngine;
using TMPro;

public class ResultUIManager : MonoBehaviour
{
    [Header("안전거리 결과")]
    public TMP_Text safeDistanceResultText;

    [Header("전방주시 결과")]
    public TMP_Text forwardAttentionResultText;

    [Header("신호 준수 결과")]
    public TMP_Text trafficViolationResultText;

    public void ShowResult()
    {
        ShowSafeDistanceResult();
        ShowForwardAttentionResult();
        ShowTrafficViolationResult();
    }

    private void ShowSafeDistanceResult()
    {
        SafeDistanceRecordManager manager =
            SafeDistanceRecordManager.Instance;

        if (manager == null ||
            safeDistanceResultText == null)
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
            $"총 측정 시간: {manager.totalMeasurementTime:F1}초\n" +
            $"전방주시 이탈 시간: {manager.totalLookAwayTime:F1}초\n" +
            $"최장 이탈 시간: {manager.longestLookAwayTime:F1}초\n" +
            $"전방주시 위반: {manager.violationCount}회";
    }

    private void ShowTrafficViolationResult()
    {
        TrafficViolationRecordManager manager =
            TrafficViolationRecordManager.Instance;

        if (manager == null ||
            trafficViolationResultText == null)
        {
            return;
        }

        trafficViolationResultText.text =
            $"[신호 준수 평가]\n" +
            $"최종 판정: {manager.GetFinalResult()}\n" +
            $"전체 통과: {manager.total_pass_count}회\n" +
            $"정상 통과: {manager.green_pass_count}회\n" +
            $"황색 신호 통과: {manager.yellow_pass_count}회\n" +
            $"적색 신호 위반: {manager.red_violation_count}회";
    }
}