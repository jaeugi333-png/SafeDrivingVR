using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartSimulation()
    {
        SceneManager.LoadScene("SampleScene");
    }
}