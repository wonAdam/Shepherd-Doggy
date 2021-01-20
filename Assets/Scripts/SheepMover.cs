using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMover : Mover
{
    Rigidbody rigidbody;
    [SerializeField] float acceleration;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public Vector2 _dir;
    public override void ProcessInput(Vector2 dir)
    {
        dir.Normalize();
        _dir = dir;

        if(dir.magnitude > Mathf.Epsilon)
            MoveForward(Mathf.Max(0f, Vector2.Dot(Vector2.up, dir)));

        Rotate(dir);

    }
    private void OnDrawGizmos()
    {
        //Move(dir);
        Vector2 v = (new Vector2(transform.forward.x, transform.forward.z)).normalized;
        //Debug.DrawLine(transform.position, transform.position + new Vector3(v.x, 0f, v.y), Color.cyan);
        Debug.DrawLine(transform.position, transform.position + new Vector3(_dir.x, 0f, _dir.y), Color.yellow);
    }
    public void MoveForward(float ratioOfSpeed)
    {
        rigidbody.velocity += transform.forward * ratioOfSpeed * acceleration * Time.deltaTime;
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, speed);
    }
    public void Move(Vector2 dir) => rigidbody.velocity = (transform.forward * dir.y + transform.right * dir.x) * speed * Time.deltaTime;
    private void Rotate(Vector2 dir)
    {
        Vector3 desiredDir = transform.forward * dir.y + transform.right * dir.x;
        if (dir.magnitude > Mathf.Epsilon)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(desiredDir, Vector3.up),
                rot * Time.deltaTime);
    }
}
