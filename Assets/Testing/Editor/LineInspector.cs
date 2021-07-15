using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Line))]
public class LineInspector : Editor
{

    void OnSceneGUI()
    {
        Line line = target as Line;
        Transform handleTransform = line.transform;

        // Transform points from local to world space
        Vector3 p0 = handleTransform.TransformPoint(line.p0);
        Vector3 p1 = handleTransform.TransformPoint(line.p1);

        // Draw lines and labels
        Handles.DrawLine(p0, p1);
        Handles.Label(p0, "Start");
        Handles.Label(p1, "End");

        // Do unity editor control things
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        // Show handle
        Handles.PositionHandle(p0, handleRotation);
        Handles.PositionHandle(p1, handleRotation);

        // Do handle things
        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            line.p0 = handleTransform.InverseTransformPoint(p0);
        }
        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            line.p1 = handleTransform.InverseTransformPoint(p1);
        }
    }
}