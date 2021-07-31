using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTrack : MonoBehaviour
{
    [Header("Start of track")]
    public Vector2 startPosition = Vector2.zero;
    private Vector2 lastEndPosition;
    public Vector2 startDirection = Vector2.up;
    private Vector2 lastEndDirection;
    public TrackSection[] startTrackSections;
    [Header("Procedural generation")]
    public TrackSection[] trackSections;
    public LineRenderer lineRenderer;
    [Header("Other")]
    public int splineAccuracy = 25;
    public float scale = 10;
    private List<TrackSection> currentTrackSections;


    void Start()
    {
        currentTrackSections = new List<TrackSection>();
        CreateStart();
        UpdateRenderer();
    }

    void CreateStart() {
        // Create inital section
        // TODO what if start track sections empty?
        lastEndPosition = startPosition;
        lastEndDirection = startDirection;
        foreach (TrackSection section in startTrackSections) {
            AddNewSection(section);
        }
    }

    void AddNewSection(TrackSection section) {
        TrackSection instantiatedSection = CopySection(section);
        AlignSection(instantiatedSection, lastEndPosition, lastEndDirection);
        currentTrackSections.Add(instantiatedSection);

        int nPts = instantiatedSection.curvePoints.Length;
        lastEndPosition = instantiatedSection.curvePoints[nPts - 1];
        lastEndDirection = (instantiatedSection.curvePoints[nPts - 1] - instantiatedSection.curvePoints[nPts - 2]).normalized;
    }

    void AlignSection(TrackSection instantiatedSection, Vector2 startPosition, Vector2 startDirection) {
        Vector2 offset = startPosition - instantiatedSection.curvePoints[0];
        Vector2 startGradient = (instantiatedSection.curvePoints[0] - instantiatedSection.curvePoints[1]).normalized;

        for (int i = 0; i < instantiatedSection.curvePoints.Length; i++) {
            Vector2 transformedPoint = instantiatedSection.curvePoints[i] + offset;
            instantiatedSection.curvePoints[i] = RotateAround(transformedPoint, startPosition, 180-Vector2.SignedAngle(startGradient, lastEndDirection));
        }
    }

    TrackSection CopySection(TrackSection section) {
        TrackSection newSection = new TrackSection();

        newSection.curvePoints = new Vector2[section.curvePoints.Length];
        for (int i = 0; i < section.curvePoints.Length; i++) {
            newSection.curvePoints[i] = section.curvePoints[i];
        }
        return newSection;
    }

    Vector2 RotateAround(Vector2 point, Vector2 origin, float degreesClockWise) {
        float ox = origin.x;
        float oy = origin.y;
        float px = point.x;
        float py = point.y;

        float angle = -degreesClockWise * Mathf.PI / 180;

        float qx = ox + Mathf.Cos(angle) * (px - ox) - Mathf.Sin(angle) * (py - oy);
        float qy = oy + Mathf.Sin(angle) * (px - ox) + Mathf.Cos(angle) * (py - oy);
        return new Vector2(qx, qy);
    }

    void UpdateRenderer()
    {
        lineRenderer.startWidth = scale;
        lineRenderer.endWidth = scale;
        List<Vector3> points = new List<Vector3>();
        foreach (TrackSection section in currentTrackSections)
        {
            int numCurves = Mathf.FloorToInt((section.curvePoints.Length - 1) / 3);
            for (int j = 0; j < numCurves; j++)
            {
                int offset = j * 3;
                for (int i = 0; i < splineAccuracy; i++)
                {
                    float t = (float)i / splineAccuracy;
                    Vector2 current = BezierCurve.GetPoint(section.curvePoints[0 + offset], section.curvePoints[1 + offset], section.curvePoints[2 + offset], section.curvePoints[3 + offset], t);
                    points.Add(current * scale);
                }
            }
        }
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    // Update is called once per frame
    void Update()
    {

    }
}