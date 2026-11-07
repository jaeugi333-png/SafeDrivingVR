
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class NotificationToggleController : MonoBehaviour
{
    private Toggle notificationToggle;

    private const string SettingKey = "ViolationNotificationEnabled";

    private void Awake()
    {
        notificationToggle = GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        // 저장된 설정 불러오기
        bool savedState =
            PlayerPrefs.GetInt(SettingKey, 1) == 1;

        // Toggle 상태 복원
        notificationToggle.SetIsOnWithoutNotify(savedState);

        // 이벤트 중복 연결 방지
        notificationToggle.onValueChanged.RemoveListener(OnToggleChanged);
        notificationToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnDisable()
    {
        notificationToggle.onValueChanged.RemoveListener(OnToggleChanged);
    }

    // 사용자가 Toggle을 변경했을 때
    private void OnToggleChanged(bool isOn)
    {
        // 메인 화면에서 설정 저장
        PlayerPrefs.SetInt(SettingKey, isOn ? 1 : 0);
        PlayerPrefs.Save();

        // 알림 관리자가 현재 존재한다면 즉시 적용
        if (ViolationNotificationManager.Instance != null)
        {
            ViolationNotificationManager.Instance
                .SetNotificationEnabled(isOn);
        }
    }
}
