using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Player : MovingEntity
{
    public Rigidbody rigidbody;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {

        //Vector2 WASDInput = GetInput();
        //Vector3 inputVec3 = Coordinate.InputToVector3(WASDInput);
        //Vector3 speed = inputVec3 * maxSpeed / GetComponent<Rigidbody>().mass;
        //rigidbody.velocity = Vector3.Dot(transform.forward, speed.normalized) * speed.normalized * maxSpeed;

        //Rotate(inputVec3);
        //ProcessAnim(Camera.main.transform.forward, inputVec3);

    }

    public void JoystickInput(Vector2 input)
    {
        if(input == Vector2.zero)
        {
            rigidbody.velocity -= rigidbody.velocity.normalized * (rigidbody.velocity.magnitude * Time.deltaTime / 3f);
            ProcessAnim(Camera.main.transform.forward, rigidbody.velocity);
            return;
        }

        Vector3 inputVec3 = Coordinate.InputToVector3(input);
        Vector3 speed = inputVec3 / GetComponent<Rigidbody>().mass;
        rigidbody.velocity = Vector3.Dot(transform.forward, speed.normalized) * speed * maxSpeed;

        Rotate(inputVec3);
        ProcessAnim(Camera.main.transform.forward, inputVec3);
    }
    private Vector2 GetInput()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        return new Vector2(h, v);
    }

    private void Rotate(Vector3 lookDir)
    {
        if (lookDir.magnitude > Mathf.Epsilon)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDir, Vector3.up),
                turnRate * Time.deltaTime);
    }
    private void ProcessAnim(Vector3 cameraForward, Vector3 inputVec3)
    {
        Vector3 worldCurrDir = transform.forward;
        float moveX = Vector3.Dot(inputVec3, Vector3.Cross(Vector3.up, worldCurrDir));
        float moveZ = Vector3.Dot(inputVec3, worldCurrDir);

        anim.SetFloat("MoveX", moveX);
        anim.SetFloat("MoveZ", moveZ);
    }

    
}
