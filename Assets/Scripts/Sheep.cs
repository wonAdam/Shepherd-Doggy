using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SteeringBehavior), typeof(Rigidbody))]
public class Sheep : MovingEntity
{
    public SteeringBehavior steeringBehavior;
    public Rigidbody rigidbody;

    [SerializeField] SteeringBehavior.SteeringBehaviorState state;
    // Start is called before the first frame update
    void Start()
    {
        steeringBehavior = GetComponent<SteeringBehavior>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public Vector3 steeringForce;
    public Vector3 acceleration;
    void Update()
    {
        Vector3 steeringForce = steeringBehavior.Calculate(state, target.transform.position);
        if (steeringForce.magnitude < Mathf.Epsilon)
        {
            rigidbody.velocity -= rigidbody.velocity.normalized * (rigidbody.velocity.magnitude / rigidbody.mass);
            return;
        }

        steeringForce = new Vector3(steeringForce.x, 0f, steeringForce.z);
        this.steeringForce = steeringForce;
        Vector3 acceleration = steeringForce / 1f;
        this.acceleration = acceleration;
        rigidbody.AddForce(acceleration * Time.deltaTime, ForceMode.VelocityChange);
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);

        Rotate(rigidbody.velocity);
    }

    private void Rotate(Vector3 lookDir)
    {
        if (lookDir.magnitude > Mathf.Epsilon)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDir, Vector3.up),
                turnRate * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + GetComponent<Rigidbody>().velocity, Color.blue);
    }
}
