
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViolationNotificationManager : MonoBehaviour
{
    public static ViolationNotificationManager Instance { get; private set; }

    [Header("알림 UI")]
    public TMP_Text notificationText;

    [Header("알림 설정")]
    public bool notificationEnabled = true;
    public float displayDuration = 2f;
    public float repeatInterval = 5f;

    private const string SettingKey = "ViolationNotificationEnabled";

    // 위반 종류
    public enum ViolationType
    {
        SafeDistance,
        ForwardAttention,
        Traffic,
        Wiper,
        TwoHand,
        TurnSignal,
        LaneDeparture,
        Speed,
        StartSafety
    }

    // 알림 정보
    private class Notification
    {
        public ViolationType type;
        public string message;
        public int priority;
    }

    // 알림 대기열
    private readonly List<Notification> notificationQueue =
        new List<Notification>();

    // 위반 종류별 마지막 알림 시각
    private readonly Dictionary<ViolationType, float> lastNotificationTime =
        new Dictionary<ViolationType, float>();

    private bool isDisplaying;
    private bool hasCurrentNotification;

    private ViolationType currentNotificationType;
    private Coroutine notificationCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 저장된 알림 설정 불러오기
        notificationEnabled =
            PlayerPrefs.GetInt(SettingKey, 1) == 1;
    }

    private void Start()
    {
        // 시작할 때 알림 텍스트 숨기기
        HideNotification();
    }

    // 설정창의 Toggle과 연결
    public void SetNotificationEnabled(bool enabled)
    {
        notificationEnabled = enabled;

        // 설정 저장
        PlayerPrefs.SetInt(SettingKey, enabled ? 1 : 0);
        PlayerPrefs.Save();

        if (!enabled)
        {
            // 대기 중인 알림 삭제
            notificationQueue.Clear();

            // 실행 중인 알림 코루틴 중지
            if (notificationCoroutine != null)
            {
                StopCoroutine(notificationCoroutine);
                notificationCoroutine = null;
            }

            isDisplaying = false;
            hasCurrentNotification = false;

            HideNotification();
        }
    }

    // 다른 위반 판정 스크립트에서 호출
    public void ShowViolation(ViolationType type)
    {
        // 알림이 OFF라면 표시하지 않음
        if (!notificationEnabled)
        {
            return;
        }

        // 현재 표시 중인 알림과 동일하면 무시
        if (hasCurrentNotification &&
            currentNotificationType == type)
        {
            return;
        }

        // 대기열에 동일한 위반이 있다면 무시
        foreach (Notification notification in notificationQueue)
        {
            if (notification.type == type)
            {
                return;
            }
        }

        // 동일한 위반의 재알림 간격 확인
        if (lastNotificationTime.TryGetValue(type, out float lastTime))
        {
            if (Time.time - lastTime < repeatInterval)
            {
                return;
            }
        }

        // 새로운 알림 생성
        Notification newNotification = new Notification
        {
            type = type,
            message = GetViolationMessage(type),
            priority = GetViolationPriority(type)
        };

        // 대기열에 추가
        notificationQueue.Add(newNotification);

        // 우선순위 정렬
        // 숫자가 작을수록 먼저 표시
        notificationQueue.Sort((a, b) =>
            a.priority.CompareTo(b.priority));

        lastNotificationTime[type] = Time.time;

        // 알림이 표시 중이지 않으면 코루틴 실행
        if (!isDisplaying)
        {
            notificationCoroutine =
                StartCoroutine(DisplayNotifications());
        }
    }

    // 대기열에 있는 알림을 순서대로 표시
    private IEnumerator DisplayNotifications()
    {
        isDisplaying = true;

        while (notificationQueue.Count > 0 && notificationEnabled)
        {
            // 우선순위가 가장 높은 알림 가져오기
            Notification notification = notificationQueue[0];

            notificationQueue.RemoveAt(0);

            currentNotificationType = notification.type;
            hasCurrentNotification = true;

            // 텍스트 표시
            if (notificationText != null)
            {
                notificationText.text = notification.message;
                notificationText.gameObject.SetActive(true);
            }

            Debug.Log("[위반 알림] " + notification.message);

            // 지정된 시간 동안 표시
            yield return new WaitForSeconds(displayDuration);

            // 텍스트 숨기기
            HideNotification();

            hasCurrentNotification = false;
        }

        isDisplaying = false;
        notificationCoroutine = null;
    }

    // 알림 텍스트 숨기기
    private void HideNotification()
    {
        if (notificationText != null)
        {
            notificationText.gameObject.SetActive(false);
        }
    }

    // 위반 종류에 따른 메시지
    private string GetViolationMessage(ViolationType type)
    {
        switch (type)
        {
            case ViolationType.SafeDistance:
                return "안전거리 위반!\n앞차와의 거리를 유지해주세요.";

            case ViolationType.ForwardAttention:
                return "전방주시 위반!\n전방을 주시해주세요.";

            case ViolationType.Traffic:
                return "신호 위반!\n교통신호를 준수해주세요.";

            case ViolationType.Wiper:
                return "와이퍼 미작동!\n와이퍼를 작동해주세요.";

            case ViolationType.TwoHand:
                return "양손 운전 위반!\n양손으로 핸들을 잡아주세요.";

            case ViolationType.TurnSignal:
                return "방향지시등 위반!\n회전 전 방향지시등을 켜주세요.";

            case ViolationType.LaneDeparture:
                return "차선 이탈!\n차선을 유지해주세요.";

            case ViolationType.Speed:
                return "과속 위반!\n속도를 줄여주세요.";

            case ViolationType.StartSafety:
                return "출발 전 확인 위반!\n출발 전 좌우를 확인해주세요.";

            default:
                return "안전수칙을 준수해주세요.";
        }
    }

    // 위반 종류에 따른 우선순위
    // 숫자가 작을수록 우선순위가 높음
    private int GetViolationPriority(ViolationType type)
    {
        switch (type)
        {
            case ViolationType.ForwardAttention:
            case ViolationType.SafeDistance:
            case ViolationType.StartSafety:
                return 1;

            case ViolationType.Traffic:
            case ViolationType.Speed:
                return 2;

            case ViolationType.LaneDeparture:
            case ViolationType.TwoHand:
                return 3;

            case ViolationType.TurnSignal:
            case ViolationType.Wiper:
                return 4;

            default:
                return 10;
        }
    }

    // 현재 알림 ON/OFF 상태 반환
    public bool IsNotificationEnabled()
    {
        return notificationEnabled;
    }
}
