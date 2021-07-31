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
        // TrackSection copy = new TrackSection();
        // copy = trackSections[0];
        // currentTrackSections.Add(copy);
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
        // Create new section
        TrackSection instantiatedSection = new TrackSection();
        instantiatedSection = section;
        // Align section
        AlignSection(instantiatedSection, lastEndPosition, lastEndPosition);
        currentTrackSections.Add(instantiatedSection);
    }

    void AlignSection(TrackSection instantiatedSection, Vector2 startPosition, Vector2 startDirection) {
        Vector2 offset = startPosition - instantiatedSection.curvePoints[0];
        // foreach (Vector2 point in instantiatedSection.curvePoints) {
        for (int i = 0; i < instantiatedSection.curvePoints.Length; i++) {
            // Translate
            Vector2 transformedPoint = instantiatedSection.curvePoints[i] + offset;
            // Rotate
            // instantiatedSection.curvePoints[i] = RotateAround(transformedPoint, startPosition, Vector2.Angle(startDirection, lastEndDirection));
            instantiatedSection.curvePoints[i] = transformedPoint;
        }
    }

    Vector2 RotateAround(Vector2 point, Vector2 origin, float degreesClockWise) {
        float ox = origin.x;
        float oy = origin.y;
        float px = point.x;
        float py = point.y;

        float qx = ox + Mathf.Cos(degreesClockWise) * (px - ox) - Mathf.Sin(degreesClockWise) * (py - oy);
        float qy = oy + Mathf.Sin(degreesClockWise) * (px - ox) + Mathf.Cos(degreesClockWise) * (py - oy);
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