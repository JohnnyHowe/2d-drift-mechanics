using UnityEngine;

public interface Track
{
    /// <summary>
    /// Is the point on the track
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    bool IsOnTrack(Vector2 point);

    /// <summary>
    /// What is the distance from the start of the track to the closest position to point on the track?
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    float GetDistance(Vector2 point);

    /// <summary>
    /// What is the position along the centre line of the track at distance from the start of the track
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    Vector2 GetCentreLinePoint(float distance);
}
