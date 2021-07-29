using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrackSection))]
public class TrackSectionEditor : Editor
{

    public override void OnInspectorGUI()
    {
        TrackSection section = (TrackSection)target;
        int numCurves = Mathf.Max(EditorGUILayout.IntField("Number of Curves", NumCurves(section)), 1);

        int numPts = numCurves * 3 + 1;
        PadCurve(section, numPts);

        for (int i = 0; i < numPts; i++)
        {
            string name;
            if (i == 0) {
                name = "Start Track Point";
            } else if (i == numPts - 1) {
                name = "End Track Point";
            } else {
                name = (i % 3 == 0) ? "Track Point: " +  (i / 3).ToString(): "Control Point";
            }
            section.curvePoints[i] = EditorGUILayout.Vector2Field(name, section.curvePoints[i]);
        }
    }

    int NumCurves(TrackSection section)
    {
        return Mathf.FloorToInt((section.curvePoints.Length - 1) / 3);
    }

    void PadCurve(TrackSection section, int newLength)
    {
        Vector2[] points = new Vector2[newLength];
        int currentLength = section.curvePoints.Length;
        if (newLength != currentLength)
        {
            for (int i = 0; i < Mathf.Min(currentLength, newLength); i++)
            {
                if (i < currentLength)
                {
                    points[i] = section.curvePoints[i];
                }
                else
                {
                    points[i] = Vector2.zero;
                }
            }
            section.curvePoints = points;
        }
    }
}

