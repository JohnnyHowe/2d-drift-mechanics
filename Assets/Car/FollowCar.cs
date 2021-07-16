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
        Vector3 current = transform.position;
        Vector3 next = car.transform.position + car.transform.up * forwardOffset;
        float distance = (current - next).magnitude;
        next.z = current.z;
        transform.position = Vector3.Lerp(current, next, speed * Time.fixedDeltaTime * distance * distance);

        Quaternion currentRot = transform.rotation;
        Quaternion nextRot = car.transform.rotation;
        float distanceRot = Quaternion.Angle(currentRot, nextRot);
        transform.rotation = Quaternion.Lerp(currentRot, nextRot, rotSpeed * Time.fixedDeltaTime * distanceRot * distanceRot);
    }
}
