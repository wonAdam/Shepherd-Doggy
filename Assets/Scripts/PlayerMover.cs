using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private Rigidbody rigidbody;
    private MovingEntity movingEntity;
    private Animator anim;

    private void Start()
    {
        movingEntity = GetComponent<MovingEntity>();
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    public void ProcessInput(Vector2 wasd)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward = new Vector3(cameraForward.x, 0f, cameraForward.z);
        Quaternion toCameraForward = Quaternion.FromToRotation(Vector3.forward, cameraForward);
        Vector3 desiredDir = toCameraForward * (new Vector3(wasd.x, 0f, wasd.y));

        Move(desiredDir);
        Rotate(desiredDir);
        ProcessAnim(cameraForward, desiredDir, wasd);
    }

    private void Move(Vector3 dir) => rigidbody.velocity = 
        Vector3.Dot(transform.forward, dir.normalized) * dir * movingEntity.maxSpeed * Time.deltaTime;

    private void Rotate(Vector3 lookDir)
    {
        if (lookDir.magnitude > Mathf.Epsilon)
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                Quaternion.LookRotation(lookDir, Vector3.up), 
                movingEntity.turnRate * Time.deltaTime);
    }
    private void ProcessAnim(Vector3 cameraForward, Vector2 desiredDir , Vector2 wasd)
    {
        Vector3 worldCurrDir = new Vector3(transform.forward.x, 0f, transform.forward.z);
        Quaternion toLocal = Quaternion.FromToRotation(worldCurrDir, cameraForward);

        Vector3 wasdTo3D = new Vector3(wasd.x, 0f, wasd.y);

        anim.SetFloat("MoveX", (toLocal * wasdTo3D.normalized).x);
        anim.SetFloat("MoveZ", (toLocal * wasdTo3D.normalized).z);
    }
}
