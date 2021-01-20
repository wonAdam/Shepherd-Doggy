using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Vector3 offsetFromTarget;
    [SerializeField] Transform target;
    // Start is called before the first frame update
    void Start()
    {
        offsetFromTarget = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offsetFromTarget;
    }
}
