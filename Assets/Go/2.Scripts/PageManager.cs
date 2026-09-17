using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    [Header("페이지")]
    public GameObject[] pages;

    [Header("버튼")]
    public Button prevButton;
    public Button nextButton;

    private int currentPage = 0;


    private void Start()
    {
        currentPage = 0;

        UpdatePage();
    }


    // 다음 버튼
    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;

            UpdatePage();
        }
    }


    // 이전 버튼
    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;

            UpdatePage();
        }
    }


    // 현재 페이지 표시
    private void UpdatePage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }


        // 이전 버튼
        // 첫 페이지라면 비활성화
        prevButton.interactable = currentPage > 0;


        // 다음 버튼
        // 마지막 페이지라면 비활성화
        nextButton.interactable =
            currentPage < pages.Length - 1;
    }
}