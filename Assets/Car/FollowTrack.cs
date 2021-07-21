using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTrack : MonoBehaviour
{
    public PresetTrack track;
    public Transform car;
    public float lookAhead = 2;
    public float speed = 1f;
    public float rotSpeed = 1f;

        public Transform mark;
    void FixedUpdate()
    {
        Vector2 trackPt = track.GetCentreLinePoint(track.GetDistance(car.position) + lookAhead);
        mark.position = (Vector3) trackPt + Vector3.forward * -5;
        // transform.position = Vector3.forward * transform.position.z + (Vector3) trackPt; 
        SmoothFollow(trackPt);
    }

    void SmoothFollow(Vector2 next)
    {
        Vector2 current = transform.position;
        float distance = (current - next).magnitude;
        transform.position = (Vector3) Vector2.Lerp(current, next, speed * Time.fixedDeltaTime * distance * distance) + Vector3.forward * transform.position.z;

        Quaternion currentRot = transform.rotation;
        Quaternion nextRot = car.transform.rotation;
        float distanceRot = Quaternion.Angle(currentRot, nextRot);
        transform.rotation = Quaternion.Lerp(currentRot, nextRot, rotSpeed * Time.fixedDeltaTime * distanceRot * distanceRot);
    }
}
