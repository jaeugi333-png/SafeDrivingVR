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
    public GameObject explainUI;
    public GameObject resultUI;
    public GameObject mainUI_1;
    public GameObject mainUI_2;

    [Header("소리 설정")]
    public Slider volumeSlider;

    private const string VolumeKey = "MasterVolume";


    void Start()
    {
        if (DrivingResultData.Instance != null &&
            DrivingResultData.Instance.hasResult)
        {
            // 주행 완료 후 MainScene으로 돌아온 경우
            ShowResultUI();
        }
        else
        {
            // 앱을 처음 실행한 경우
            ShowExplainUI();
        }

        //UI 1번 불러오기
        MainUI1Road();

        // 저장된 음량 불러오기
        LoadAudioSettings();
    }

    // 시뮬레이션 시작
    public void StartSimulation()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void StartSimulation2()
    {
        SceneManager.LoadScene("Load2");
    }

    // 설명 UI
    public void ShowExplainUI()
    {
        explainUI.SetActive(true);
        mainUI.SetActive(false);
        safetyRulesUI.SetActive(false);
        settingsUI.SetActive(false);
        controllerUI.SetActive(false);
        resultUI.SetActive(false);
    }

    // 결과 UI
    public void ShowResultUI()
    {
        explainUI.SetActive(false);
        mainUI.SetActive(false);
        safetyRulesUI.SetActive(false);
        settingsUI.SetActive(false);
        controllerUI.SetActive(false);

        resultUI.SetActive(true);
    }

    // 메인 메뉴
    public void OpenMainUI()
    {
        explainUI.SetActive(false);
        mainUI.SetActive(true);
        safetyRulesUI.SetActive(false);
        settingsUI.SetActive(false);
        controllerUI.SetActive(false);
        resultUI.SetActive(false);
    }

    // 안전 수칙
    public void OpenSafetyRules()
    {
        explainUI.SetActive(false);
        mainUI.SetActive(false);
        settingsUI.SetActive(false);
        safetyRulesUI.SetActive(true);
        controllerUI.SetActive(false);
        resultUI.SetActive(false);
    }

    // 설정
    public void OpenSettings()
    {
        explainUI.SetActive(false);
        mainUI.SetActive(false);
        safetyRulesUI.SetActive(false);
        settingsUI.SetActive(true);
        controllerUI.SetActive(false);
        resultUI.SetActive(false);
    }

    // 조작법
    public void OpenController()
    {
        explainUI.SetActive(false);
        mainUI.SetActive(false);
        safetyRulesUI.SetActive(false);
        settingsUI.SetActive(false);
        controllerUI.SetActive(true);
        resultUI.SetActive(false);
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

    public void MainUI1Road()
    {
        mainUI_1.SetActive(true);
        mainUI_2.SetActive(false);
    }

    public void MainUI2Road()
    {
        mainUI_1.SetActive(false);
        mainUI_2.SetActive(true);
    }

    // 저장된 음량 불러오기
    private void LoadAudioSettings()
    {
        float savedVolume =
            PlayerPrefs.GetFloat(VolumeKey, 1f);

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }

        AudioListener.volume = savedVolume;
    }
}