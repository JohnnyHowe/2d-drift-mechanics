using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Control")]
    public float throttle = 0;
    public float steering = 0;
    [Header("Engine")]
    public float engineForce = 25;
    [Header("Steering")]
    public float maxSteeringAngle = 100;
    public float maxDriftSteeringAngle = 150;
    [Header("Tires")]
    public float driftAccelMul = 0.6f;
    public float perpendicularDrag = 1f;
    public float AcceleratingPerpendicularDrag = 0.3f;
    public float parallelDrag = 0.15f;
    [Header("Tires (Drift Parameters)")]
    public float driftAngleWeight = 0.05f;
    public float driftSteeringWeight = 3f;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        transform.eulerAngles -= Vector3.forward * GetSteeringChange() * Time.fixedDeltaTime;
        if (ShouldDrift()) {
            rb.AddForce(transform.up.normalized * GetWheelDriveForce() * driftAccelMul);
        } else {
            rb.AddForce(transform.up.normalized * GetWheelDriveForce());
            rb.velocity = transform.up.normalized * rb.velocity.magnitude;
        }
        rb.AddForce(GetDrag());
    }

    public bool ShouldDrift() {
        return GetDriftAngle() * driftAngleWeight + Mathf.Abs(steering) * driftSteeringWeight >= 1;
    }

    float GetDriftAngle() {
        return Vector3.Angle(rb.velocity.normalized, transform.up);
    }

    float GetWheelPerpendicularForce() {
        return GetVelocityInDir(transform.right).magnitude;
    }

    float GetWheelDriveForce()
    {
        return throttle * engineForce;
    }

    float GetSteeringChange()
    {
        if (ShouldDrift()) {
            return steering * maxDriftSteeringAngle;
        } else {
            return steering * maxSteeringAngle;
        }
    }

    Vector3 GetVelocityInDir(Vector3 dir) {
        return Vector3.Project(rb.velocity, dir);
    }

    Vector3 GetDrag() {
        return GetPerpindicularDrag() + GetParallelDrag();
    }

    Vector3 GetPerpindicularDrag() {
        Vector3 v = GetVelocityInDir(transform.right);
        float drag;
        if (Mathf.Abs(v.magnitude) < 1) {
            drag = Mathf.Sqrt(Mathf.Abs(v.magnitude));
        } else {
            drag = v.magnitude * v.magnitude;
        }
        if (throttle > 0.1f) {
            drag *= AcceleratingPerpendicularDrag;
        } else {
            drag *= perpendicularDrag;
        }
        float clockwiseMul = Mathf.Sign(Vector3.Cross(transform.up.normalized, rb.velocity.normalized).z);
        return transform.right * drag * clockwiseMul;
    }
    Vector3 GetParallelDrag() {
        Vector3 v = GetVelocityInDir(transform.up);
        float drag = v.magnitude * v.magnitude;
        float clockwiseMul = Mathf.Sign(Vector3.Cross(transform.right.normalized, rb.velocity.normalized).z);
        return -transform.up * drag * parallelDrag * clockwiseMul;
    }
}
