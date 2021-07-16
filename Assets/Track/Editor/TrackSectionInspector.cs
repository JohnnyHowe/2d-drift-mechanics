using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackSection))]
public class TrackSectionInspector : Editor
{
    private TrackSection track;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private bool changedFlag = false;

    private void OnSceneGUI()
    {
        changedFlag = false;
        track = target as TrackSection;
        handleTransform = track.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        // Draw curves
        int tl = track.curves.Length;
        for (int i = 0; i < tl; i++)
        {
            DrawCurve(ref track.curves[i]);
        }
        track.curves[tl - 1].p3 = handleTransform.InverseTransformPoint(DrawPoint(handleTransform.TransformPoint(track.curves[tl - 1].p3), "Track Point"));
        if (changedFlag)
        {
            track.Validate();
        }
    }

    public override void OnInspectorGUI()
    {
        track = target as TrackSection;
        if (GUILayout.Button("Update Line Renderer")) {
            track.Validate();
        };
        base.OnInspectorGUI();
    }

    private void DrawCurve(ref Curve curve)
    {
        Vector3 start = handleTransform.TransformPoint(curve.p0);
        Vector3 c1 = handleTransform.TransformPoint(curve.p1);
        Vector3 c2 = handleTransform.TransformPoint(curve.p2);
        Vector3 end = handleTransform.TransformPoint(curve.p3);
        curve.p0 = handleTransform.InverseTransformPoint(DrawPoint(start, "Track Point"));
        curve.p1 = handleTransform.InverseTransformPoint(DrawPoint(c1, "Control 1"));
        curve.p2 = handleTransform.InverseTransformPoint(DrawPoint(c2, "Control 2"));

        // Draw control lines
        Handles.color = Color.gray;
        Handles.DrawLine(start, c1);
        Handles.DrawLine(c1, c2);
        Handles.DrawLine(c2, end);

        // Draw actual curve
        Handles.color = Color.white;
        Vector3 lineStart = track.transform.TransformPoint(curve.GetPoint(0f));
        for (float i = 0; i <= track.pointsPerCurve; i++)
        {
            float t = i / track.pointsPerCurve;
            Vector3 lineEnd = track.transform.TransformPoint(curve.GetPoint(t));
            Handles.DrawLine(lineStart, lineEnd);
            lineStart = lineEnd;
        }
    }

    private Vector3 DrawPoint(Vector3 point, string label)
    {
        EditorGUI.BeginChangeCheck();
        Vector3 p = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            changedFlag = true;
        }
        Handles.Label(p, label);
        return p;
    }
}