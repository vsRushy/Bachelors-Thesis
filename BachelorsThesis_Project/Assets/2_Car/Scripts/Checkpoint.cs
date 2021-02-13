using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Checkpoints checkpoints;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent<CarController>(out CarController car_controller))
        {
            checkpoints.PassCheckpoint(this);
        }
    }

    public void SetCheckPoints(Checkpoints checkpoints)
    {
        this.checkpoints = checkpoints;
    }
}
