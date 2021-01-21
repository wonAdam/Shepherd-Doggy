using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SphericalCoordinates
{
    private float _radius;
    public float _minRadius = 3f;
    public float _maxRadius = 20f;
    public float radius
    {
        get { return _radius; }
        set { _radius = Mathf.Clamp(value, _minRadius, _maxRadius); }
    }

    // 방위각
    private float _azimuth;
    public float _minAzimuth = 0f;
    public float _maxAzimuth = 360f;
    public float azimuth
    {
        get { return _azimuth; }
        set { _azimuth = Mathf.Repeat(value, _maxAzimuth - _minAzimuth); }
    }

    // 앙각
    private float _elevation;
    public float _minElevation = -80f;
    public float _maxElevation = 80f;

    public float elevation
    {
        get { return _elevation; }
        set { _elevation = Mathf.Clamp(value, _minElevation, _maxElevation); }
    }

    public SphericalCoordinates(Vector3 cartesianCoordinate)
    {
        // degree -> radian
        _minAzimuth *= Mathf.Deg2Rad;
        _maxAzimuth *= Mathf.Deg2Rad;
        _minElevation *= Mathf.Deg2Rad;
        _maxElevation *= Mathf.Deg2Rad;

        radius = cartesianCoordinate.magnitude;

        azimuth = Mathf.Atan2(cartesianCoordinate.z, cartesianCoordinate.x);
        elevation = Mathf.Asin(cartesianCoordinate.y / radius);
    }

    public SphericalCoordinates Rotate(float azimuthOffset, float elevationOffset)
    {
        azimuth -= azimuthOffset;
        elevation -= elevationOffset;
        return this;
    }

    public SphericalCoordinates TranslateRadius(float x)
    {
        radius += x;
        return this;
    }

    public Vector3 ToCartesian()
    {
        float x = Mathf.Cos(elevation) * radius * Mathf.Cos(azimuth);
        float y = Mathf.Sin(elevation) * radius;
        float z = Mathf.Cos(elevation) * radius * Mathf.Sin(azimuth);
        return new Vector3(x, y, z);
    }

}

public class CameraWork : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float scrollSpeed;
    [SerializeField] Transform pivot;
    [SerializeField] Transform cam;
    public SphericalCoordinates sphericalCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        sphericalCoordinates = new SphericalCoordinates(cam.position);
        cam.position = sphericalCoordinates.ToCartesian() + pivot.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        if (Input.GetMouseButton(1))
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            RotateCamera(h, v);
        }

        float scroll = (-1f) * Input.GetAxis("Mouse ScrollWheel");
        CloseUp(scroll);


        
        SetCamera();
    }

    public void RotateCamera(float h, float v)
    {
        if (Mathf.Abs(h) > Mathf.Epsilon || Mathf.Abs(v) > Mathf.Epsilon)
        {
            float aOffset = h * rotateSpeed * Time.deltaTime;
            float eOffset = v * rotateSpeed * Time.deltaTime;
            sphericalCoordinates.Rotate(aOffset, eOffset);
            SetCamera();
        }
    }

    public void CloseUp(float amount)
    {
        if (Mathf.Abs(amount) > Mathf.Epsilon)
        {
            float scrollOffset = amount * scrollSpeed * Time.deltaTime;
            sphericalCoordinates.TranslateRadius(scrollOffset);
            SetCamera();
        }

    }

    public void SetCamera() { cam.position = sphericalCoordinates.ToCartesian() + pivot.position; cam.LookAt(pivot); }
    }