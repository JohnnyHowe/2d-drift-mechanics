using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackSection))]
public class TrackSectionInspector : Editor
{
    private TrackSection track;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private void OnSceneGUI()
    {
        track = target as TrackSection;
        handleTransform = track.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;
        
        // Draw curves
        int tl = track.curves.Length;
        for (int i = 0; i < tl; i ++) {
            DrawCurve(ref track.curves[i]);
        }
        track.curves[tl - 1].p3 = handleTransform.InverseTransformPoint(DrawPoint(handleTransform.TransformPoint(track.curves[tl - 1].p3), "Track Point"));
        track.Validate();
    }

    private void DrawCurve(ref Curve curve) {
        Vector3 start = handleTransform.TransformPoint(curve.p0);
        Vector3 end = handleTransform.TransformPoint(curve.p3);

        curve.p0 = handleTransform.InverseTransformPoint(DrawPoint(start, "Track Point"));
        Handles.DrawLine(start, end);
    }

    private Vector3 DrawPoint(Vector3 point, string label) {
        EditorGUI.BeginChangeCheck();
        Vector3 p = Handles.DoPositionHandle(point, handleRotation);
        Handles.Label(p, label);
        // if (EditorGUI.EndChangeCheck())
        // {
        //     p = handleTransform.InverseTransformPoint(p);
        // }
        return p;
    }
}