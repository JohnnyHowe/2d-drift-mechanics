using UnityEngine;

public class BezierCurve : MonoBehaviour
{

    public Vector3 p0, p1, p2, p3;

    public void Reset()
    {
        p0 = new Vector3(0f, 0f, 0f);
        p1 = new Vector3(1f, 0f, 0f);
        p2 = new Vector3(2f, 0f, 0f);
        p3 = new Vector3(3f, 0f, 0f);
    }

    public Vector3 GetPoint(float t)
    {
        Vector3 b1 = Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
        Vector3 b2 = Vector3.Lerp(Vector3.Lerp(p1, p2, t), Vector3.Lerp(p2, p3, t), t);
        return Vector3.Lerp(b1, b2, t);
    }
}