using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Transform checkpoints;

    List<Checkpoint> checkpoint_list;
    uint next_checkpoint;

    void Awake()
    {
        checkpoint_list = new List<Checkpoint>();
        next_checkpoint = 0u;

        foreach(Transform checkpoint_transform in checkpoints)
        {
            Checkpoint checkpoint = checkpoint_transform.GetComponent<Checkpoint>();

            checkpoint.SetCheckPoints(this);
            checkpoint_list.Add(checkpoint);
        }
    }

    public void PassCheckpoint(Checkpoint checkpoint)
    {
        if(checkpoint_list.IndexOf(checkpoint) == next_checkpoint)
        {
            ++next_checkpoint;
        }
    }
}
