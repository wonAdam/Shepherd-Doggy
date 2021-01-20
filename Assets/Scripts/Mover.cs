using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rot;
    [SerializeField] Transform body;
    [SerializeField] Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // translate
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 moveVel = Vector3.forward * h + Vector3.left * v;
        GetComponent<CharacterController>().Move(moveVel * Time.deltaTime);

        // rotation
        if(moveVel != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVel, Vector3.up), rot);


        // anim
        Quaternion ToLocal = Quaternion.FromToRotation(transform.forward, Vector3.forward);
        anim.SetFloat("MoveX", (ToLocal * moveVel).x);
        anim.SetFloat("MoveZ", (ToLocal * moveVel).z);
    }
}
