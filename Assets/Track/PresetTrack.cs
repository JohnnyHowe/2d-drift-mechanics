using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PresetTrack : MonoBehaviour, Track
{
    public List<Vector2> points;
    public LineRenderer line;
    public float trackWidth = 8;
    public float d;

    public Vector2 GetCentreLinePoint(float distance)
    {
        int p1i = Mathf.FloorToInt(distance) % points.Count;
        if (p1i < 0) p1i += points.Count;
        int p2i = Mathf.CeilToInt(distance) % points.Count;
        if (p2i < 0) p2i += points.Count;
        Vector2 p1 = points[p1i];
        Vector2 p2 = points[p2i];
        return Vector2.Lerp(p1, p2, distance % 1);
    }

    public float GetDistance(Vector2 point)
    {
        int[] pts = GetClosestPointIndices(point);
        Vector2 mid = GetClosestInterpolatedPoint(point);
        float d1 = (points[pts[0]] - mid).magnitude;
        float d2 = (points[pts[1]] - mid).magnitude;
        float t = d1 / (d1 + d2);
        return pts[0] + t;
    }

    public bool IsOnTrack(Vector2 point)
    {
        return (GetClosestInterpolatedPoint(point) - point).magnitude < trackWidth / 2;
    }

    private Vector2 GetClosestInterpolatedPoint(Vector2 point) {
        Vector2[] points = GetClosestPoints(point);

        Vector2 onto = points[1] - points[0];
        Vector2 vec = (Vector2) point - points[0];
        return (Vector2) Vector3.Project(vec, onto) + points[0];
    }

    private int[] GetClosestPointIndices(Vector2 point) {
        float minDist = -1;
        int minIndex = 0;
        int i = 0;
        foreach (Vector2 position in points) {
            float dist = (point - position).magnitude;
            if (dist < minDist || minDist < 0) {
                minDist = dist;
                minIndex = i;
            }
            i++;
        }

        int leftIndex = minIndex - 1;
        int rightIndex = minIndex + 1;

        if (minIndex == 0) {leftIndex = points.Count - 1;}
        if (minIndex == points.Count - 1) {rightIndex = 0;}

        Vector2 minPoint = points[minIndex];

        Vector2 leftPoint = points[leftIndex];
        Vector2 leftVec = (leftPoint - minPoint).normalized;
        Vector2 normLeftPoint = minPoint + leftVec;
        float leftDist = (normLeftPoint - (Vector2) point).magnitude;

        Vector2 rightPoint = points[rightIndex];
        Vector2 rightVec = (rightPoint - minPoint).normalized;
        Vector2 normRightPoint = minPoint + rightVec;
        float rightDist = (normRightPoint - (Vector2) point).magnitude;

        if (leftDist < rightDist) {
            return new int[] {leftIndex, minIndex};
        } else {
            return new int[] {minIndex, rightIndex};
        }
    }

    private Vector2[] GetClosestPoints(Vector2 point) {
        int[] pts = GetClosestPointIndices(point); 
        return new Vector2[] {points[pts[0]], points[pts[1]]};
    }

    void Start()
    {
        line.startWidth = trackWidth;
        line.endWidth = trackWidth;
        line.positionCount = transform.childCount + 1;
        int i = 0;
        foreach (Transform trans in transform) {
            points.Add((Vector2) trans.position);
            line.SetPosition(i, trans.position - Vector3.forward);
            i ++;
        }
            line.SetPosition(i, transform.GetChild(0).position);
    }
}
