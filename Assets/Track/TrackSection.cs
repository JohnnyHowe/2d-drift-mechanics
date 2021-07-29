using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "TrackSection", menuName = "Track/TrackSection", order = 1)]
public class TrackSection : ScriptableObject {
    public Vector2[] curvePoints;
}