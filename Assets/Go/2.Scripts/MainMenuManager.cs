using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("메뉴 UI")]
    public GameObject mainUI;
    public GameObject safetyRulesUI;
    public GameObject settingsUI;
    public GameObject controllerUI;

    [Header("소리 설정")]
    public Slider volumeSlider;

    private const string VolumeKey = "MasterVolume";


    void Start()
    {
        // 처음에는 메인 UI만 표시
        ShowMainUI();

        // 저장된 음량 불러오기
        LoadAudioSettings();
    }

    // 시뮬레이션 시작
    public void StartSimulation()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // 메인 메뉴
    public void ShowMainUI()
    {
        mainUI.SetActive(true);
        safetyRulesUI.SetActive(false);
        settingsUI.SetActive(false);
        controllerUI.SetActive(false);
    }

    // 안전 수칙
    public void OpenSafetyRules()
    {
        mainUI.SetActive(false);
        settingsUI.SetActive(false);
        safetyRulesUI.SetActive(true);
        controllerUI.SetActive(false);
    }

    // 설정
    public void OpenSettings()
    {
        mainUI.SetActive(false);
        safetyRulesUI.SetActive(false);
        settingsUI.SetActive(true);
        controllerUI.SetActive(false);
    }

    // 조작법
    public void OpenController()
    {
        mainUI.SetActive(false);
        safetyRulesUI.SetActive(false);
        settingsUI.SetActive(false);
        controllerUI.SetActive(true);
    }

    // 음량 설정
    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;

        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    // 종료
    public void QuitApplication()
    {
        Application.Quit();
    }


    private void LoadAudioSettings()
    {
        // 저장된 값이 없으면 기본 음량 100%
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }

        AudioListener.volume = savedVolume;
    }


}