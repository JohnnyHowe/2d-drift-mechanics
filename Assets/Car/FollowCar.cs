using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    public Transform car;
    public float speed = 1f;
    public float rotSpeed = 1f;
    public float forwardOffset = 4f;

    // Update is called once per frame
    void FixedUpdate()
    {
        SmoothFollow(car.transform.position + car.transform.up * forwardOffset);
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
