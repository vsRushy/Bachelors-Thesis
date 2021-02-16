using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class CarAgent : Agent
{
    public CarController car_controller;

    [SerializeField]
    Checkpoints checkpoints;

    void Awake()
    {
        car_controller = GetComponent<CarController>();
    }

    void Start()
    {
        checkpoints.OnCorrectCheckpointEvent += OnCorrectCheckpoint;
        checkpoints.OnWrongCheckpointEvent += OnWrongCheckpoint;
    }

    void Update()
    {
        
    }

    void OnCorrectCheckpoint(object sender, CheckpointEventArgs e)
    {
        
    }

    void OnWrongCheckpoint(object sender, CheckpointEventArgs e)
    {
        
    }
}
