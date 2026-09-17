using UnityEngine;

public class DrivingResultData : MonoBehaviour
{
    public static DrivingResultData Instance;

    [Header("결과 존재 여부")]
    public bool hasResult = false;

    // 위반 기록
    [Header("위반 기록")]
    public int safeDistanceViolation;
    public int forwardAttentionViolation;
    public int trafficViolation;
    public int wiperViolation;
    public int twoHandViolation;
    public int turnSignalViolation;
    public int laneDepartureViolation;
    public int speedViolation;
    public int startSafetyViolation;

    // 항목별 점수
    [Header("항목별 점수")]
    public int safeDistanceScore;
    public int forwardAttentionScore;
    public int trafficScore;
    public int wiperScore;
    public int twoHandScore;
    public int turnSignalScore;
    public int laneDepartureScore;
    public int speedScore;
    public int startSafetyScore;


    [Header("총점")]
    public int totalScore;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // DrivingScene 결과 저장
    public void SaveResult()
    {
        // 안전거리
        if (SafeDistanceRecordManager.Instance != null)
        {
            safeDistanceViolation =
                SafeDistanceRecordManager.Instance.violationCount;
        }

        // 전방주시
        if (ForwardAttentionRecordManager.Instance != null)
        {
            forwardAttentionViolation =
                ForwardAttentionRecordManager.Instance.violationCount;
        }

        // 신호 준수
        if (TrafficViolationRecordManager.Instance != null)
        {
            trafficViolation =
                TrafficViolationRecordManager.Instance.red_violation_count;
        }

        // 와이퍼
        if (WiperUsageRecordManager.Instance != null)
        {
            wiperViolation =
                WiperUsageRecordManager.Instance.violation_count;
        }

        // 양손 운전
        if (TwoHandDrivingRecordManager.Instance != null)
        {
            twoHandViolation =
                TwoHandDrivingRecordManager.Instance.violation_count;
        }

        // 방향지시등
        if (TurnSignalRecordManager.Instance != null)
        {
            turnSignalViolation =
                TurnSignalRecordManager.Instance.violation_count;
        }

        // 차선 이탈
        if (LaneDepartureRecordManager.Instance != null)
        {
            laneDepartureViolation =
                LaneDepartureRecordManager.Instance.violation_count;
        }

        // 과속
        if (SpeedRecordManager.Instance != null)
        {
            speedViolation =
                SpeedRecordManager.Instance.violation_count;
        }

        // 출발 전 확인
        if (StartSafetyRecordManager.Instance != null)
        {
            startSafetyViolation =
                StartSafetyRecordManager.Instance.violation_count;
        }


        // 점수 계산
        CalculateScore();

        // 결과 저장 완료
        hasResult = true;
    }


    // 점수 계산
    private void CalculateScore()
    {
        /*
         * 안전거리
         * 최대 10점
         * 위반 1회당 -1점
         */
        safeDistanceScore =
            Mathf.Max(0, 10 - safeDistanceViolation);


        /*
         * 전방주시
         * 최대 10점
         * 현재는 violation 값 1당 -1점.
         * 이후 "3초 이후 초당 -1점" 기록 방식으로 수정 예정.
         */
        forwardAttentionScore =
            Mathf.Max(0, 10 - forwardAttentionViolation);


        /*
         * 신호 준수
         * 최대 15점
         * 신호 위반 1회당 -5점
         */
        trafficScore =
            Mathf.Max(0, 15 - (trafficViolation * 5));


        /*
         * 와이퍼
         * 최대 10점
         * 현재는 violation 값 1당 -1점.
         * 이후 "3초 이후 초당 -1점" 기록 방식으로 수정 예정.
         */
        wiperScore =
            Mathf.Max(0, 10 - wiperViolation);


        /*
         * 양손 운전
         * 최대 10점
         * 현재는 violation 값 1당 -1점.
         * 이후 "3초 이후 초당 -1점" 기록 방식으로 수정 예정.
         */
        twoHandScore =
            Mathf.Max(0, 10 - twoHandViolation);


        /*
         * 방향지시등
         * 최대 15점
         * 잘못된 사용 / 미사용 1회당 -5점
         */
        turnSignalScore =
            Mathf.Max(0, 15 - (turnSignalViolation * 5));


        /*
         * 차선 이탈
         * 최대 10점
         * 차선 이탈 1회당 -1점
         */
        laneDepartureScore =
            Mathf.Max(0, 10 - laneDepartureViolation);


        /*
         * 과속
         * 최대 15점
         * 3초 이상 과속 위반 1회당 -1점
         */
        speedScore =
            Mathf.Max(0, 15 - speedViolation);


        /*
         * 출발 전 확인
         * 최대 5점
         * 미실시 시 -5점
         */
        startSafetyScore =
            Mathf.Max(0, 5 - (startSafetyViolation * 5));

        // 총점 계산
        totalScore =
            safeDistanceScore +
            forwardAttentionScore +
            trafficScore +
            wiperScore +
            twoHandScore +
            turnSignalScore +
            laneDepartureScore +
            speedScore +
            startSafetyScore;
    }
}