using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CarController controller;
    public TrailRenderer trail;

    void Update()
    {
        controller.throttle = 0;
        if (Input.GetKey(KeyCode.W)) controller.throttle += 1;
        if (Input.GetKey(KeyCode.S)) controller.throttle -= 1;
        controller.steering = 0;
        if (Input.GetKey(KeyCode.D)) controller.steering += 1;
        if (Input.GetKey(KeyCode.A)) controller.steering -= 1;
        // trail.SetActive(controller.IsDrift());
        trail.emitting = controller.ShouldDrift();
    }
}
