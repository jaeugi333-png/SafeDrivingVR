using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeDistanceChecker : MonoBehaviour
{
    [Header("앞차")]
    public Transform Front_car;

    [Header("거리 기준")]
    public float danger_distance = 8f;

    [Header("위험 지속 시간")]
    public float close_time = 0f;

    private bool is_close = false;
    private bool is_checked = false;

    void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        if (Front_car == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, Front_car.position);

        if (distance <= danger_distance && is_checked == false)
        {
            is_close = true;
            close_time += Time.deltaTime;
        }
        else
        {
            if (is_close == true && is_checked == false)
            {
                ApplyPenalty();
            }

            is_close = false;
            close_time = 0f;
        }

        if (close_time >= 3f && is_checked == false)
        {
            ApplyPenalty();
        }
    }

    void ApplyPenalty()
    {
        if (close_time >= 3f)
        { 
            is_checked = true;
        }
        else if (close_time >= 2f)
        {
            is_checked = true;
        }
        else if (close_time >= 1f)
        {
            is_checked = true;
        }
    }
}