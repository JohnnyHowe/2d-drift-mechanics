using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackSection : MonoBehaviour
{

    public int pointsPerCurve = 1;
    public LineRenderer lineRenderer;

    public Curve[] curves;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Validate()
    {
        OnValidate();
    }

    void OnValidate()
    {
        for (int i = 0; i < curves.Length - 1; i++)
        {
            curves[i].p3 = curves[i + 1].p0;
        }
        SetTrackPoints();
    }

    void SetTrackPoints()
    {
        if (lineRenderer)
        {
            int numPoints = curves.Length * pointsPerCurve + 1;
            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = numPoints;

            for (int i = 0; i < curves.Length; i++)
            {
                for (int j = 0; j < pointsPerCurve; j++)
                {
                    int index = pointsPerCurve * i + j;
                    float t = ((float) j) / pointsPerCurve;
                    // lineRenderer.SetPosition(index, curves[i].GetPoint(t));
                    lineRenderer.SetPosition(index, transform.TransformPoint(curves[i].GetPoint(t)));
                }
            }
            lineRenderer.SetPosition(numPoints - 1, transform.TransformPoint(curves[curves.Length - 1].GetPoint(1f)));
        }
    }
}


[Serializable]
public class Curve
{
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;

    public Vector3 GetPoint(float t)
    {
        Vector3 b1 = Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
        Vector3 b2 = Vector3.Lerp(Vector3.Lerp(p1, p2, t), Vector3.Lerp(p2, p3, t), t);
        return Vector3.Lerp(b1, b2, t);
    }


}