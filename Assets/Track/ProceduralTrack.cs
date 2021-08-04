using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTrack : Track
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
    public Transform car;
    public int forwardBufferSections = 5;
    public int rearBufferSections = 3;
    [Header("Other")]
    public int splineAccuracy = 25;
    public float scale = 10;
    private List<TrackSection> currentTrackSections;
    public int g;


    void Start()
    {
        currentTrackSections = new List<TrackSection>();
        CreateStart();
        UpdateRenderer();
    }

    void FixedUpdate() {
        if (CreateSections()) {
            UpdateRenderer();
        }
    }

    /// <summary>
    /// If required by car position, create and delete sections
    /// returns whether change was made
    /// </summary>
    bool CreateSections() {
        int currentIndex = GetCarSectionIndex();
        int finalIndex = currentIndex + forwardBufferSections;
        int numGenerate = Mathf.Max(0, finalIndex - currentTrackSections.Count);
        bool change = false;
        for (int i = 0; i < numGenerate; i ++) {
            change = true;
            CreateRandomSection();
        }
        while (currentTrackSections.Count > forwardBufferSections + rearBufferSections) {
            currentTrackSections.RemoveAt(0);
        }
        return change;
    }

    /// <summary>
    /// What section is the car on (or closest to?)
    /// returned as index of currrentTrackSections
    /// </summary>
    int GetCarSectionIndex() {
        int closestIndex = 0;
        float closestDistance = -1;
        for (int i = 0; i < currentTrackSections.Count; i++) {
            Vector2 lastPoint = currentTrackSections[i].curvePoints[currentTrackSections[i].curvePoints.Length - 1];
            float distance = ((Vector2) car.position - lastPoint).magnitude;
            if (distance < closestDistance || closestDistance == -1) {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        return closestIndex;
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

    void CreateRandomSection() {
        AddNewSection(trackSections[Random.Range(0, trackSections.Length)]);
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
        // TODO what if curve 1 and 2 are same?
        for (int i = 0; i < instantiatedSection.curvePoints.Length; i++) {
            Vector2 transformedPoint = instantiatedSection.curvePoints[i] + offset;
            instantiatedSection.curvePoints[i] = RotateAround(transformedPoint, startPosition, 180-Vector2.SignedAngle(startGradient, lastEndDirection));
        }
    }

    TrackSection CopySection(TrackSection section) {
        TrackSection newSection = new TrackSection();

        newSection.curvePoints = new Vector2[section.curvePoints.Length];
        for (int i = 0; i < section.curvePoints.Length; i++) {
            newSection.curvePoints[i] = section.curvePoints[i] * scale;
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
                    points.Add(current);
                }
            }
        }
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    public override bool IsOnTrack(Vector2 point)
    {
        // TODO
        throw new System.NotImplementedException();
    }
}