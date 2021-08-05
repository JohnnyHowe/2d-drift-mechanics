using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "TrackSection", menuName = "Track/TrackSection", order = 1)]
public class TrackSection : ScriptableObject {
    public Vector2[] curvePoints;

    /// <summary>
    /// Returns a copy of this
    /// </summary>
    public TrackSection Copy() {
        TrackSection newSection = new TrackSection();
        newSection.curvePoints = new Vector2[curvePoints.Length];
        for (int i = 0; i < curvePoints.Length; i++) {
            newSection.curvePoints[i] = curvePoints[i];
        }
        return newSection;
    }

    /// <summary>
    /// Scale all the points of the track section up in place
    /// Multiples all the components of the positions
    /// Returns the section (also modifies this)
    /// </summary>
    public TrackSection Scale(float scale) {
        for (int i = 0; i < curvePoints.Length; i++) {
            curvePoints[i] *= scale;
        }
        return this;
    }

    /// <summary>
    /// Align this such that the start point is at startPosition and the start gradient is startGradient
    /// returns this
    /// </summary>
    public TrackSection AlignSection(Vector2 startPosition, Vector2 startGradient) {
        SetStartGradient(startGradient);
        SetStartPosition(startPosition);
        return this;
    }

    public TrackSection SetStartPosition(Vector2 startPosition) {
        Vector2 offset = startPosition - curvePoints[0];
        for (int i = 0; i < curvePoints.Length; i++) {
            curvePoints[i] += offset;
        }
        return this;
    }

    /// <summary>
    /// Rotate the track section so that the start gradient is startGradient
    /// Rotates about this start point
    /// </summary>
    public TrackSection SetStartGradient(Vector2 startGradient) {
        Vector2 currentStartGradient = (curvePoints[0] - curvePoints[1]).normalized;
        float angle = 180-Vector2.SignedAngle(currentStartGradient, startGradient);
        RotateAround(curvePoints[0], angle);
        return this;
    }

    /// <summary>
    /// Rotate the section around a point
    /// Just rotates all curve points about the origin
    /// returns reference to this
    /// </summary>
    public TrackSection RotateAround(Vector2 origin, float angle) {
        for (int i = 0; i < curvePoints.Length; i++) {
            Vector2 transformedPoint = curvePoints[i];
            curvePoints[i] = RotatePointAround(transformedPoint, origin, angle);
        }
        return this;
    }

    Vector2 RotatePointAround(Vector2 point, Vector2 origin, float degreesClockWise) {
        float ox = origin.x;
        float oy = origin.y;
        float px = point.x;
        float py = point.y;

        float angle = -degreesClockWise * Mathf.PI / 180;

        float qx = ox + Mathf.Cos(angle) * (px - ox) - Mathf.Sin(angle) * (py - oy);
        float qy = oy + Mathf.Sin(angle) * (px - ox) + Mathf.Cos(angle) * (py - oy);
        return new Vector2(qx, qy);
    }
}