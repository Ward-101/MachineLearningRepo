using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxStearAngle = 42f;
    public float motorForce = 1000f;

    public WheelCollider wheelFrontLeftCollider, wheelFrontRightCollider, wheelBackLeftCollider, wheelBackRightCollider;
    public Transform wheelFrontLeft, wheelFrontRight, wheelBackLeft, wheelBackRight;

    public Rigidbody rb;
    public Transform centerOfMass;

    public float horizontalInput;
    public float verticalInput;
    private float steeringAngle;

    private void Start()
    {
        rb.centerOfMass = centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(wheelFrontRightCollider, wheelFrontRight);
        UpdateWheelPose(wheelFrontLeftCollider, wheelFrontLeft);
        UpdateWheelPose(wheelBackLeftCollider, wheelBackLeft);
        UpdateWheelPose(wheelBackRightCollider, wheelBackRight);
    }

    Vector3 pos;
    Quaternion quat;

    private void UpdateWheelPose(WheelCollider col, Transform tr)
    {
        pos = tr.position;
        quat = tr.rotation;

        col.GetWorldPose(out pos, out quat);

        tr.position = pos;
        tr.rotation = quat;
    }

    private void Accelerate()
    {
        wheelFrontLeftCollider.motorTorque = verticalInput * motorForce;
        wheelFrontRightCollider.motorTorque = verticalInput * motorForce;
    }

    private void Steer()
    {
        steeringAngle = horizontalInput * maxStearAngle;

        wheelFrontLeftCollider.steerAngle = steeringAngle;
        wheelFrontRightCollider.steerAngle = steeringAngle;
    }

    public void ResetInput()
    {
        horizontalInput = 0;
        verticalInput = 0;
    }
}
