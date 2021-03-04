using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Checkpoint : MonoBehaviour
{
    public Transform nextCheckpoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.GetComponent<Scr_Agent>())
        {
            if (other.transform.parent.GetComponent<Scr_Agent>().nextCheckpoint == transform)
            {
                other.transform.parent.GetComponent<Scr_Agent>().CheckpointReached(nextCheckpoint);
            }
        }

        if (other.transform.parent.GetComponent<Scr_CarController>())
        {
            other.transform.parent.GetComponent<Scr_CarController>().lastCheckpoint = this;
        }
    }

}
