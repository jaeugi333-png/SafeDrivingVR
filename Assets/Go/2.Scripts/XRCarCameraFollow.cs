using UnityEngine;

public class XRCarCameraFollow : MonoBehaviour
{
    public Transform seat_point;
    public Transform car;

    void LateUpdate()
    {
        if (seat_point == null || car == null)
        {
            return;
        }

        transform.position = seat_point.position;
        transform.rotation = Quaternion.Euler(0f, car.eulerAngles.y, 0f);
    }
}