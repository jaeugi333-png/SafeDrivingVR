using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCarController : MonoBehaviour
{
    [Header("차량 속도")]
    public float move_speed = 10f;

    void Update()
    {
        transform.Translate(Vector3.forward * move_speed * Time.deltaTime);
    }
}