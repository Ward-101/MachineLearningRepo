using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (Scr_Manager.instance.isVersusModeOn == true)
        {
            if (other.transform.parent.GetComponent<Scr_Agent>())
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
