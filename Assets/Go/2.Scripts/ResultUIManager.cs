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

    [Header("와이퍼 사용 결과")]
    public TMP_Text wiperUsageResultText;

    [Header("양손 운전 결과")]
    public TMP_Text twoHandDrivingResultText;

    [Header("방향지시등 결과")]
    public TMP_Text turnSignalResultText;

    [Header("차선 이탈 결과")]
    public TMP_Text laneDepartureResultText;

    [Header("속도 준수 결과")]
    public TMP_Text speedResultText;

    [Header("출발 전 확인 결과")]
    public TMP_Text startSafetyResultText;

    public void ShowResult()
    {
        safeDistanceResultText.gameObject.SetActive(true);
        forwardAttentionResultText.gameObject.SetActive(true);
        trafficViolationResultText.gameObject.SetActive(true);
        wiperUsageResultText.gameObject.SetActive(true);
        twoHandDrivingResultText.gameObject.SetActive(true);
        turnSignalResultText.gameObject.SetActive(true);
        laneDepartureResultText.gameObject.SetActive(true);
        speedResultText.gameObject.SetActive(true);
        startSafetyResultText.gameObject.SetActive(true);
        ShowSafeDistanceResult();
        ShowForwardAttentionResult();
        ShowTrafficViolationResult();
        ShowWiperUsageResult();
        ShowTwoHandDrivingResult();
        ShowTurnSignalResult();
        ShowLaneDepartureResult();
        ShowSpeedResult();
        ShowStartSafetyResult();
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

    private void ShowWiperUsageResult()
    {
        WiperUsageRecordManager manager =
            WiperUsageRecordManager.Instance;

        if (manager == null ||
            wiperUsageResultText == null)
        {
            return;
        }

        wiperUsageResultText.text =
            $"[와이퍼 사용 평가]\n" +
            $"최종 판정: {manager.GetFinalResult()}\n" +
            $"비 구간 주행: {manager.total_rain_time:F1}초\n" +
            $"와이퍼 미사용: {manager.total_wiper_off_time:F1}초\n" +
            $"최장 미사용: {manager.longest_wiper_off_time:F1}초\n" +
            $"미사용 위반: {manager.violation_count}회";
    }

    private void ShowTwoHandDrivingResult()
    {
        TwoHandDrivingRecordManager manager =
            TwoHandDrivingRecordManager.Instance;

        if (manager == null ||
            twoHandDrivingResultText == null)
        {
            return;
        }

        twoHandDrivingResultText.gameObject.SetActive(true);

        twoHandDrivingResultText.text =
            "[양손 운전 평가]\n" +
            "위반 횟수: "
            + manager.violation_count
            + "회\n" +
            "최종 판정: "
            + manager.GetFinalResult();
    }

    private void ShowTurnSignalResult()
    {
        TurnSignalRecordManager manager =
            TurnSignalRecordManager.Instance;

        if (manager == null || turnSignalResultText == null)
            return;

        turnSignalResultText.gameObject.SetActive(true);

        turnSignalResultText.text =
            "[방향지시등 평가]\n" +
            "전체 회전: " + manager.total_turn_count + "회\n" +
            "정상 사용: " + manager.correct_count + "회\n" +
            "위반 횟수: " + manager.violation_count + "회\n" +
            "최종 판정: " + manager.GetFinalResult();
    }

    private void ShowLaneDepartureResult()
    {
        LaneDepartureRecordManager manager =
            LaneDepartureRecordManager.Instance;

        if (manager == null ||
            laneDepartureResultText == null)
        {
            return;
        }

        laneDepartureResultText.gameObject.SetActive(true);

        laneDepartureResultText.text =
            "[차선 이탈 평가]\n" +
            "위반 횟수: " +
            manager.violation_count +
            "회\n" +
            "최종 판정: " +
            manager.GetFinalResult();
    }

    private void ShowSpeedResult()
    {
        SpeedRecordManager manager =
            SpeedRecordManager.Instance;

        if (manager == null ||
            speedResultText == null)
        {
            return;
        }

        speedResultText.gameObject.SetActive(true);

        speedResultText.text =
            "[속도 준수 평가]\n" +
            "최고 속도: " +
            manager.max_speed.ToString("F1") +
            " km/h\n" +
            "과속 위반: " +
            manager.violation_count +
            "회\n" +
            "최종 판정: " +
            manager.GetFinalResult();
    }

    private void ShowStartSafetyResult()
    {
        StartSafetyRecordManager manager =
            StartSafetyRecordManager.Instance;

        if (manager == null ||
            startSafetyResultText == null)
        {
            return;
        }

        startSafetyResultText.gameObject.SetActive(true);

        startSafetyResultText.text =
            "[출발 전 확인 평가]\n" +
            "왼쪽 확인: " +
            (manager.left_checked ? "완료" : "미확인") +
            "\n" +
            "오른쪽 확인: " +
            (manager.right_checked ? "완료" : "미확인") +
            "\n" +
            "위반: " +
            manager.violation_count +
            "회\n" +
            "최종 판정: " +
            manager.GetFinalResult();
    }
}