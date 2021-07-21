using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PresetTrack : MonoBehaviour, Track
{
    public List<Vector2> points;
    public LineRenderer line;
    public float trackWidth = 8;
    public Transform car;
    public Transform c1;
    public Transform c2;
    public Transform mp;

    public Vector2 GetCentreLinePoint(float distance)
    {
        throw new System.NotImplementedException();
    }

    public float GetDistance(Vector2 point)
    {
        throw new System.NotImplementedException();
    }

    public bool IsOnTrack(Vector2 point)
    {
        return (GetClosestInterpolatedPoint(point) - point).magnitude < trackWidth / 2;
    }

    private Vector2 GetClosestInterpolatedPoint(Vector2 point) {
        Vector2[] points = GetClosestPoints(car.position);

        Vector2 onto = points[1] - points[0];
        Vector2 vec = (Vector2) car.transform.position - points[0];
        return (Vector2) Vector3.Project(vec, onto) + points[0];
    }

    private Vector2[] GetClosestPoints(Vector2 point) {
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
        float leftDist = (normLeftPoint - (Vector2) car.transform.position).magnitude;

        Vector2 rightPoint = points[rightIndex];
        Vector2 rightVec = (rightPoint - minPoint).normalized;
        Vector2 normRightPoint = minPoint + rightVec;
        float rightDist = (normRightPoint - (Vector2) car.transform.position).magnitude;

        if (leftDist < rightDist) {
            return new Vector2[2] {points[leftIndex], points[minIndex]};
        } else {
            return new Vector2[2] {minPoint, points[rightIndex]};
        }
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

    void Update() {
        Vector2[] points = GetClosestPoints(car.position);
        c1.position = (Vector3) points[0] + Vector3.forward * c1.position.z;
        c2.position = (Vector3) points[1] + Vector3.forward * c1.position.z;
        mp.position = (Vector3) GetClosestInterpolatedPoint(car.position) + Vector3.forward * mp.position.z;
        if (!IsOnTrack(car.position)) {Camera.main.backgroundColor = Color.red;}
        else {Camera.main.backgroundColor = Color.green;}
    }
}
