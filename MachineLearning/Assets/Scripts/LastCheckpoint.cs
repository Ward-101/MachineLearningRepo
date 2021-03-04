using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastCheckpoint : MonoBehaviour
{
    public bool isVersusModeOn;
    public Manager _ScriptManager;

    private void Start()
    {
        isVersusModeOn = _ScriptManager.isVersusModeOn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isVersusModeOn == true)
        {
            if (other.tag == "CarIA")
            {
                Debug.Log("You LOSE!");
            }
            else if (other.tag == "CarPlayer")
            {
                Debug.Log("You WIN!");
            }
        }
    }
}
