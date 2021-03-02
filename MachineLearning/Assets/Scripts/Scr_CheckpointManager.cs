using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CheckpointManager : MonoBehaviour
{
    public static Scr_CheckpointManager instance;

    public Transform firstCheckpoint;

    private void Awake()
    {
        instance = this;
    }

    [ContextMenu("SetCheckpoints")]
    public void Init()
    {
        firstCheckpoint = transform.GetChild(0);

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).GetComponent<Scr_Checkpoint>().nextCheckpoint = transform.GetChild(i + 1);
        }

        transform.GetChild(transform.childCount - 1).GetComponent<Scr_Checkpoint>().nextCheckpoint = transform.GetChild(0);
    }
}
