using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public class CheckpointEventArgs : EventArgs
    {
        public Transform car_transform;

        public CheckpointEventArgs(Transform transform)
        {
            car_transform = transform;
        }
    }

    public delegate void CheckpointSystemHandler(object sender, CheckpointEventArgs e);

    public Transform checkpoints;
    public List<Transform> cars;
    
    List<Checkpoint> checkpoint_list;
    List<int> next_checkpoint_list;

    public event CheckpointSystemHandler OnCorrectCheckpointEvent;
    public event CheckpointSystemHandler OnWrongCheckpointEvent;

    void Awake()
    {
        checkpoint_list = new List<Checkpoint>();
        next_checkpoint_list = new List<int>();

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

    public void PassCheckpoint(Transform car, Checkpoint checkpoint)
    {
        int next_checkpoint = next_checkpoint_list[cars.IndexOf(car)];
        if(checkpoint_list.IndexOf(checkpoint) == next_checkpoint)
        {
            Debug.Log("Correct Checkpoint, car:" + car.gameObject.name);

            next_checkpoint_list[cars.IndexOf(car)] = (next_checkpoint + 1) % checkpoint_list.Count;
            OnCorrectCheckpointEvent?.Invoke(this, new CheckpointEventArgs(car));
        }
        else
        {
            Debug.Log("Incorrect Checkpoint, car:" + car.gameObject.name);

            OnWrongCheckpointEvent?.Invoke(this, new CheckpointEventArgs(car));
        }
    }

    public void ResetCheckpoint(Transform car)
    {
        next_checkpoint_list[cars.IndexOf(car)] = 0;
    }

    public Checkpoint GetNextCheckpoint(Transform car)
    {
        return checkpoint_list[(cars.IndexOf(car) + 1) % checkpoint_list.Count];
    }
}
