using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
    private CarController car_controller;

    [SerializeField]
    private Checkpoints checkpoints;

    [SerializeField]
    private Transform spawn_position;

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

    private void OnCorrectCheckpoint(object sender, Checkpoints.CheckpointEventArgs e)
    {
        if (e.car_transform == transform)
        {
            AddReward(1.0f);
        }
    }

    private void OnWrongCheckpoint(object sender, Checkpoints.CheckpointEventArgs e)
    {
        if(e.car_transform == transform)
        {
            AddReward(-1.0f);
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.position = spawn_position.position + new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f));
        transform.forward = spawn_position.forward;
        //checkpoints.ResetCheckpoint(transform);
        //car_controller.Stop();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Vector3 checkpoint_forward = checkpoints.GetNextCheckpoint(transform).transform.forward;
        //float direction_dot = Vector3.Dot(transform.forward, checkpoint_forward);
        //sensor.AddObservation(direction_dot);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float forward_amount = 0.0f;
        float turn_amount = 0.0f;

        switch(vectorAction[0])
        {
            case 0: forward_amount = 0.0f; break;
            case 1: forward_amount = 1.0f; break;
            case 2: forward_amount = -1.0f; break;

            default: break;
        }

        switch (vectorAction[1])
        {
            case 0: turn_amount = 0.0f; break;
            case 1: turn_amount = 1.0f; break;
            case 2: turn_amount = -1.0f; break;

            default: break;
        }

        car_controller.SetInputs(forward_amount, turn_amount);
    }

    public override void Heuristic(float[] actionsOut)
    {
        int forward_action = 0;
        if (Input.GetKey(KeyCode.UpArrow)) forward_action = 1;
        if (Input.GetKey(KeyCode.DownArrow)) forward_action = 2;

        int turn_action = 0;
        if (Input.GetKey(KeyCode.RightArrow)) turn_action = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) turn_action = 2;

        float[] discrete_actions = actionsOut;
        discrete_actions[0] = forward_action;
        discrete_actions[1] = turn_action;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-0.5f);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-0.1f);
        }
    }
}
