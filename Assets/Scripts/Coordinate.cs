using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Coordinate
{
    public static Vector3 InputToVector3(Vector2 input)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward = new Vector3(cameraForward.x, 0f, cameraForward.z);
        Quaternion toCameraForward = Quaternion.FromToRotation(Vector3.forward, cameraForward);
        Vector3 resultVector = toCameraForward * (new Vector3(input.x, 0f, input.y));

        return resultVector;
    }

    public static Vector3 LocalToWorld(Vector3 v, Transform tr)
    {
        Matrix4x4 TRS = Matrix4x4.TRS(
            tr.position, 
            Quaternion.FromToRotation(Vector3.forward, tr.forward), 
            Vector3.one);

        return TRS.MultiplyPoint3x4(v);
    }

    public static Vector3 WorldToLocal(Vector3 v, Transform tr)
    {
        Matrix4x4 TRS = Matrix4x4.TRS(
            -tr.position,
            Quaternion.FromToRotation(tr.forward, Vector3.forward),
            Vector3.one);

        return TRS.MultiplyPoint3x4(v);
    }
}
