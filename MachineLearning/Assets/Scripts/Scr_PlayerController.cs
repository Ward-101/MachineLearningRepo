using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerController : MonoBehaviour
{
    public Scr_CarController carController;

    private void Update()
    {
        carController.horizontalInput = Input.GetAxis("Horizontal");
        carController.verticalInput = Input.GetAxis("Vertical");
    }

}
