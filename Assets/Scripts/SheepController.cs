using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : Controller
{

    [SerializeField] Vector2 moveDir;
    [SerializeField] LayerMask evadeLayer;
    [SerializeField] float panicRange;
    [SerializeField] float wallDetectionRange;
    [SerializeField] LayerMask wallLayer;

    // Update is called once per frame
    void Update()
    {
        Vector2 desiredDirection = Vector2.zero;

        // Evade 0.4
        desiredDirection += GetEvadeDir().normalized;
        Debug.Log("GetEvadeDir " + GetEvadeDir().normalized);

        // Wall Avoidance 0.4
        //desiredDirection += GetWallAvoidanceDir().normalized * 0.4f;
        //Debug.Log("GetWallAvoidanceDir " + GetWallAvoidanceDir().normalized);

        // Cohesion 0.2


        mover.ProcessInput(desiredDirection);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, panicRange);

        Debug.DrawLine(transform.position, transform.position + transform.forward * wallDetectionRange, Color.blue);
    }

    private Vector2 GetEvadeDir()
    {
        Collider[] evadeTargets = Physics.OverlapSphere(transform.position, panicRange, evadeLayer);

        if (evadeTargets.Length == 0) return Vector2.zero;

        // get nearest
        Collider resultTarget = evadeTargets[0];
        foreach(var target in evadeTargets)
        {
            float distFromResultTarget = Vector3.Distance(transform.position, resultTarget.transform.position);
            float distFromCurrTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distFromResultTarget > distFromCurrTarget) resultTarget = target;
        }

        Vector3 fromTarget = transform.position - resultTarget.transform.position;
        Quaternion toLocal = Quaternion.FromToRotation(transform.forward, Vector3.forward);
        Vector3 localDirFromTarget = toLocal * fromTarget;

        return new Vector2(localDirFromTarget.x, localDirFromTarget.z);
    }

    private Vector2 GetWallAvoidanceDir()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, wallDetectionRange, wallLayer))
        {
            Vector3 toWall = hit.transform.position - transform.position;
            Vector3 normal = hit.normal;
            Vector3 perp1 = Vector3.Cross(normal, Vector3.up);
            Vector3 perp2 = Vector3.Cross(normal, Vector3.down);

            float dot1 = Vector3.Dot(toWall.normalized, perp1.normalized);
            float dot2 = Vector3.Dot(toWall.normalized, perp2.normalized);

            Quaternion toLocal = Quaternion.FromToRotation(Vector3.forward, transform.forward);
            Vector2 desiredLocalDir;
            if (dot1 > 0)
            {
                desiredLocalDir = toLocal * perp1;
            }
            else if(dot2 > 0)
            {
                desiredLocalDir = toLocal * perp2;
            }
            else
            {
                desiredLocalDir = toLocal * perp1;
            }

            return desiredLocalDir.normalized;

        }
        return Vector2.zero;
    }
}
