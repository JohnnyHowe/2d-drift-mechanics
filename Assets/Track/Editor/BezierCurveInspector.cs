using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{

    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const int lineSteps = 20;

    private void OnSceneGUI()
    {
        curve = target as BezierCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        // Get points in world space
        Vector3 p0 = handleTransform.TransformPoint(curve.p0);
        Vector3 p1 = handleTransform.TransformPoint(curve.p1);
        Vector3 p2 = handleTransform.TransformPoint(curve.p2);
        Vector3 p3 = handleTransform.TransformPoint(curve.p3);

        // Draw control lines
        Handles.color = Color.grey;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);

        // Draw labels
        Handles.Label(p0, "Start");
        Handles.Label(p1, "Control 1");
        Handles.Label(p2, "Control 2");
        Handles.Label(p3, "End");

        // Draw curve
        Handles.color = Color.white;
        Vector3 lineStart = curve.GetPoint(0f);
        Handles.color = Color.white;
        for (int i = 1; i <= lineSteps; i++)
        {
            Vector3 lineEnd = curve.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(handleTransform.TransformPoint(lineStart), handleTransform.TransformPoint(lineEnd));
            lineStart = lineEnd;
        }

        // Enable handles
        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            curve.p0 = handleTransform.InverseTransformPoint(p0);
        }
        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            curve.p1 = handleTransform.InverseTransformPoint(p1);
        }
        EditorGUI.BeginChangeCheck();
        p2 = Handles.DoPositionHandle(p2, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            curve.p2 = handleTransform.InverseTransformPoint(p2);
        }
        EditorGUI.BeginChangeCheck();
        p3 = Handles.DoPositionHandle(p3, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            curve.p3 = handleTransform.InverseTransformPoint(p3);
        }
    }
}