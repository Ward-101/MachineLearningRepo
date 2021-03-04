using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (Manager.instance.isVersusModeOn == true)
        {
            if (other.transform.parent.GetComponent<Agent>())
            {
                Debug.Log("You LOSE!");
            }
            else
            {
                Debug.Log("You WIN!");
            }
        }
    }
}
