using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TrackSectionBuilder))]
public class TrackSectionBuilderEditor : Editor
{
    TrackSection section;
    TrackSection lastSection;
    TrackSectionBuilder builder;
    int numCurves;
    bool changed = false;

    void OnSceneGUI()
    {
        builder = (TrackSectionBuilder)target;
        section = builder.section;
        if (section)
        {
            Undo.RecordObject(section, "Zero Transform Position");
            numCurves = Mathf.FloorToInt((section.curvePoints.Length - 1) / 3);
            DrawSpline();
            DrawHandles();
        } else {
            numCurves = 0;
        }

        if (lastSection != section) changed = true;
        if (changed) UpdateLineRenderer();
        lastSection = section;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (section) {
            Undo.RecordObject(section, "Zero Transform Position");
            DrawTextInput();
        }
    }

    void DrawTextInput() {
        EditorGUILayout.LabelField("Curve Points", EditorStyles.boldLabel);
        for (int i = 0; i < section.curvePoints.Length; i++) {
            section.curvePoints[i] = EditorGUILayout.Vector2Field(GetPointName(i, section.curvePoints.Length), section.curvePoints[i]);
        }
    }

    void DrawHandles()
    {
        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < section.curvePoints.Length; i++)
        {
            Handles.Label((Vector3)section.curvePoints[i], GetPointName(i, section.curvePoints.Length));
            section.curvePoints[i] = Handles.PositionHandle((Vector3)section.curvePoints[i], Quaternion.identity);
        }
        if (EditorGUI.EndChangeCheck())
        {
            changed = true;
        }
    }

    void UpdateLineRenderer() {
        builder.lineRenderer.positionCount = numCurves * builder.splineAccuracy + 1;
        for (int j = 0; j < numCurves; j++) {
            int offset = j * 3;
            for (int i = 0; i <= builder.splineAccuracy; i++) {
                float t = (float)i / builder.splineAccuracy;
                Vector2 current = BezierCurve.GetPoint(section.curvePoints[0 + offset], section.curvePoints[1 + offset], section.curvePoints[2 + offset], section.curvePoints[3 + offset], t);
                int index = j * builder.splineAccuracy + i;
                builder.lineRenderer.SetPosition(index, (Vector3) current);

            }
        }
    }

    string GetPointName(int index, int pointsLen)
    {
        if (index == 0) return "Start Track Point";
        else if (index == pointsLen - 1) return "End Track Point";
        else if (index % 3 == 0) return "Track Point";
        else return "Control Point " + (index / 2 + 1).ToString();
    }

    void DrawSpline()
    {
        Vector2 last = section.curvePoints[0];
        for (int j = 0; j < numCurves; j++)
        {
            int offset = j * 3;
            for (int i = 1; i <= builder.splineAccuracy; i++)
            {
                float t = (float)i / builder.splineAccuracy;
                Vector2 current = BezierCurve.GetPoint(section.curvePoints[0 + offset], section.curvePoints[1 + offset], section.curvePoints[2 + offset], section.curvePoints[3 + offset], t);
                Handles.color = Color.white;
                Handles.DrawLine(last, current);
                Handles.color = Color.grey;
                Handles.DrawLine(section.curvePoints[0 + offset], section.curvePoints[1 + offset]);
                Handles.DrawLine(section.curvePoints[1 + offset], section.curvePoints[2 + offset]);
                Handles.DrawLine(section.curvePoints[2 + offset], section.curvePoints[3 + offset]);
                last = current;
            }
        }
    }
}
