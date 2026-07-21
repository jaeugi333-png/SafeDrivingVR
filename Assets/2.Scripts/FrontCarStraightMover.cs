using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FrontCarStraightMover : MonoBehaviour
{
    [Header("앞차 속도")]
    public float speedKmh = 30f;

    [Header("주행 상태")]
    public bool isDriving = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnEnable()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (!isDriving)
        {
            return;
        }

        float speedMs = speedKmh / 3.6f;

        Vector3 nextPosition =
            rb.position +
            transform.forward * speedMs * Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);
    }

    public void StartDriving()
    {
        isDriving = true;
    }

    public void StopDriving()
    {
        isDriving = false;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}