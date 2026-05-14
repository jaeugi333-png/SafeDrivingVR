using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("점수")]
    public int score = 100;

    [Header("감점 기록")]
    public List<string> wrong_list = new List<string>();

    void Awake()
    {
        instance = this;
    }

    public void MinusScore(int minus_score, string reason)
    {
        score -= minus_score;

        if (score < 0)
        {
            score = 0;
        }

        wrong_list.Add(reason);

        Debug.Log("감점: -" + minus_score);
        Debug.Log("이유: " + reason);
        Debug.Log("현재 점수: " + score);
    }
}