using UnityEngine;

public class BezierCurve {
    /// <summary>
    /// Quadratic Bezier curve (just a line)
    /// </summary>
    /// <param name="start"></param>
    /// <param name="control1"></param>
    /// <param name="control2"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static Vector2 GetPoint(Vector2 start, Vector2 control1, Vector2 control2, Vector2 end, float t) {
        return GetPoint(GetPoint(start, control1, control2, t), GetPoint(control1, control2, end, t), t);
    }

    /// <summary>
    /// Quadratic Bezier curve (just a line)
    /// </summary>
    /// <param name="start"></param>
    /// <param name="control1"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static Vector2 GetPoint(Vector2 start, Vector2 control1, Vector2 end, float t) {
        return GetPoint(GetPoint(start, control1, t), GetPoint(control1, end, t), t);
    }

    /// <summary>
    /// Linear Bezier curve (just a line)
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static Vector2 GetPoint(Vector2 start, Vector2 end, float t) {
        return Vector2.Lerp(start, end, t);
    }
}