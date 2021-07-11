using UnityEngine;

public class CarController : MonoBehaviour
{
    public float throttle = 0;
    public float steering = 0;
    public float engineForce = 10;
    public float maxSteeringAngle = 30;
    public float maxWheelGrip = 3;
    public float perpDrag = 1;
    public float parrDrag = 1;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.up.normalized * GetAppliedForce());
        transform.eulerAngles -= Vector3.forward * GetSteeringChange() * Time.fixedDeltaTime;
        // Debug.Log(GetPerpendicularForce());
        if (IsDrift()) {
            rb.velocity = transform.up.normalized * rb.velocity.magnitude;
        }
        float clockwiseMul = Mathf.Sign(Vector3.Cross(transform.up.normalized, rb.velocity.normalized).z);
        rb.velocity += (Vector2) transform.right * Mathf.Pow(Vector3.Project(rb.velocity, transform.right).magnitude, 2) * perpDrag * clockwiseMul;
        float clockwiseMul2 = Mathf.Sign(Vector3.Cross(transform.right.normalized, rb.velocity.normalized).z);
        rb.velocity -= (Vector2) transform.up * Mathf.Pow(Vector3.Project(rb.velocity, transform.up).magnitude, 2) * parrDrag * clockwiseMul2;
    }

    public bool IsDrift() {
        return GetPerpendicularForce() < maxWheelGrip;
    }

    float GetPerpendicularForce() {
        return Vector3.Project(rb.velocity, transform.right).magnitude;
    }

    float GetAppliedForce()
    {
        return throttle * engineForce;
    }

    float GetSteeringChange()
    {
        return steering * maxSteeringAngle;
    }
}
