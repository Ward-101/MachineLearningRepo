using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CarController : MonoBehaviour
{
    public float maxStearAngle = 42f;
    public float motorForce = 1000f;
    public int driveType = 0;

    public WheelCollider wheelFrontLeftCollider, wheelFrontRightCollider, wheelBackLeftCollider, wheelBackRightCollider;
    public Transform wheelFrontLeft, wheelFrontRight, wheelBackLeft, wheelBackRight;

    public Rigidbody rb;
    public Transform centerOfMass;

    public float horizontalInput;
    public float verticalInput;
    private float steeringAngle;

    public Scr_Checkpoint lastCheckpoint;

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
        if (driveType == 0)
        {
            wheelFrontLeftCollider.motorTorque = verticalInput * motorForce;
        }
        else if (driveType == 1)
        {
            wheelFrontRightCollider.motorTorque = verticalInput * motorForce;
        }
        else
        {
            wheelFrontLeftCollider.motorTorque = verticalInput * motorForce;
            wheelFrontRightCollider.motorTorque = verticalInput * motorForce;
        }
        

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

        lastCheckpoint = null;
    }
}
