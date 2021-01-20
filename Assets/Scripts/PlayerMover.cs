using UnityEngine;

public class PlayerMover : Mover
{
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public override void ProcessInput(Vector2 wasd)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward = new Vector3(cameraForward.x, 0f, cameraForward.z);
        Quaternion toCameraForward = Quaternion.FromToRotation(Vector3.forward, cameraForward);
        Vector3 desiredDir = toCameraForward * (new Vector3(wasd.x, 0f, wasd.y));

        Move(desiredDir);
        Rotate(desiredDir);
        ProcessAnim(cameraForward, desiredDir);
    }

    private void Move(Vector3 dir) => characterController.Move(dir * speed * Time.deltaTime);

    private void Rotate(Vector3 lookDir)
    {
        if (lookDir.magnitude > Mathf.Epsilon)
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                Quaternion.LookRotation(lookDir, Vector3.up), 
                rot * Time.deltaTime);
    }
    private void ProcessAnim(Vector3 cameraForward, Vector3 moveDir)
    {
        Quaternion ToLocal = Quaternion.FromToRotation(transform.forward, cameraForward);
        anim.SetFloat("MoveX", (ToLocal * moveDir).x);
        anim.SetFloat("MoveZ", (ToLocal * moveDir).z);
    }
}
