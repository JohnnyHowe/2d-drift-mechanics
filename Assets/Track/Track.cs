using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements TrackInterface and MonoBehaviour
/// Only exists so we can nicely use the track interface in the inspector
/// 
/// So for track implementations, they should inherit from this.
/// </summary>
public abstract class Track : MonoBehaviour, TrackInterface
{
    public abstract bool IsOnTrack(Vector2 point);
}
