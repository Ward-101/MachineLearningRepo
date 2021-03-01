using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 cameraLocalPos;
    public Vector3 localTargetlookAtPos;

    public float posLerpSpeed = 0.02f;
    public float lookLerpSpeed = 0.01f;

    private Vector3 wantedPos;

    private void Update()
    {
        wantedPos = target.TransformPoint(cameraLocalPos);
        wantedPos.y = cameraLocalPos.y;

        transform.position = Vector3.Lerp(transform.position, wantedPos, posLerpSpeed);

        Quaternion look = Quaternion.LookRotation(target.TransformPoint(localTargetlookAtPos) - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, look, lookLerpSpeed);
    }

}
