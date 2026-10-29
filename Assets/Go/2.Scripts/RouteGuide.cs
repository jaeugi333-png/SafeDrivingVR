using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RouteGuide : MonoBehaviour
{
    [Header("경로 지점")]
    public Transform[] points;

    [Header("경로 설정")]
    public float lineWidth = 0.3f;
    public float heightOffset = 0.05f;
    public int smoothness = 10;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.numCapVertices = 8;
        lineRenderer.numCornerVertices = 12;

        DrawRoute();
    }

    void DrawRoute()
    {
        if (points == null || points.Length < 2)
            return;

        int segments = points.Length - 1;

        lineRenderer.positionCount =
            segments * smoothness + 1;

        int index = 0;

        for (int i = 0; i < segments; i++)
        {
            Vector3 p0 = points[Mathf.Max(i - 1, 0)].position;
            Vector3 p1 = points[i].position;
            Vector3 p2 = points[i + 1].position;
            Vector3 p3 = points[Mathf.Min(i + 2, points.Length - 1)].position;

            for (int j = 0; j < smoothness; j++)
            {
                float t = (float)j / smoothness;

                Vector3 position =
                    CatmullRom(p0, p1, p2, p3, t);

                position.y += heightOffset;

                lineRenderer.SetPosition(index, position);
                index++;
            }
        }

        Vector3 lastPosition =
            points[points.Length - 1].position;

        lastPosition.y += heightOffset;

        lineRenderer.SetPosition(index, lastPosition);
    }

    Vector3 CatmullRom(
    Vector3 p0,
    Vector3 p1,
    Vector3 p2,
    Vector3 p3,
    float t)
    {
        float alpha = 0.5f;

        float t0 = 0f;
        float t1 = t0 + Mathf.Pow(
            Vector3.Distance(p0, p1), alpha);
        float t2 = t1 + Mathf.Pow(
            Vector3.Distance(p1, p2), alpha);
        float t3 = t2 + Mathf.Pow(
            Vector3.Distance(p2, p3), alpha);

        // 중복 지점으로 인한 0 나누기 방지
        if (t1 <= t0) t1 = t0 + 0.001f;
        if (t2 <= t1) t2 = t1 + 0.001f;
        if (t3 <= t2) t3 = t2 + 0.001f;

        float time = Mathf.Lerp(t1, t2, t);

        Vector3 A1 = Vector3.LerpUnclamped(
            p0, p1, (time - t0) / (t1 - t0));

        Vector3 A2 = Vector3.LerpUnclamped(
            p1, p2, (time - t1) / (t2 - t1));

        Vector3 A3 = Vector3.LerpUnclamped(
            p2, p3, (time - t2) / (t3 - t2));

        Vector3 B1 = Vector3.LerpUnclamped(
            A1, A2, (time - t0) / (t2 - t0));

        Vector3 B2 = Vector3.LerpUnclamped(
            A2, A3, (time - t1) / (t3 - t1));

        return Vector3.LerpUnclamped(
            B1, B2, (time - t1) / (t2 - t1));
    }
}