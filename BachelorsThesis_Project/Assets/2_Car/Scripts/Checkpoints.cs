using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Transform checkpoints;
    public List<Transform> cars;
    
    List<Checkpoint> checkpoint_list;
    List<uint> next_checkpoint_list;

    public event EventHandler on_correct_checkpoint;
    public event EventHandler on_wrong_checkpoint;

    void Awake()
    {
        checkpoint_list = new List<Checkpoint>();
        next_checkpoint_list = new List<uint>();

        foreach (Transform car in cars)
        {
            next_checkpoint_list.Add(0);
        }

        foreach (Transform checkpoint_transform in checkpoints)
        {
            Checkpoint checkpoint = checkpoint_transform.GetComponent<Checkpoint>();

            checkpoint.SetCheckPoints(this);
            checkpoint_list.Add(checkpoint);
        }
    }

    void Start()
    {
        on_correct_checkpoint += OnCorrectCheckpoint;
        on_wrong_checkpoint += OnWrongCheckpoint;
    }

    public void PassCheckpoint(Transform car, Checkpoint checkpoint)
    {
        uint next_checkpoint = next_checkpoint_list[cars.IndexOf(car)];
        if(checkpoint_list.IndexOf(checkpoint) == next_checkpoint)
        {
            Debug.Log("Correct Checkpoint, car:" + car.gameObject.name);

            next_checkpoint_list[cars.IndexOf(car)] = (next_checkpoint + 1u) % (uint)checkpoint_list.Count;
            on_correct_checkpoint?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("Incorrect Checkpoint, car:" + car.gameObject.name);

            on_wrong_checkpoint?.Invoke(this, EventArgs.Empty);
        }
    }

    void OnCorrectCheckpoint(object sender, System.EventArgs e)
    {
        int i = 0;
    }

    void OnWrongCheckpoint(object sender, System.EventArgs e)
    {
        int i = 0;
    }
}
