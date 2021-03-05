using UnityEngine;

public class FallCatcher : MonoBehaviour
{
    public float fitnessPenality;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.GetComponent<Scr_CarController>())
        {
            other.transform.parent.transform.position = other.transform.parent.GetComponent<Scr_CarController>().lastCheckpoint.transform.position + Vector3.up;
            other.transform.parent.transform.rotation = other.transform.parent.GetComponent<Scr_CarController>().lastCheckpoint.transform.rotation;
            other.transform.parent.GetComponent<Scr_CarController>().horizontalInput = 0;
            other.transform.parent.GetComponent<Scr_CarController>().verticalInput = 0;

            other.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            //other.transform.parent.GetComponent<Scr_Agent>().fallPenality = fitnessPenality;
        }
    }
}
