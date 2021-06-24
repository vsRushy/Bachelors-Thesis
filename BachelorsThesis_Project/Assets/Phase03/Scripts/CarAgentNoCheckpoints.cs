using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgentNoCheckpoints : Agent
{
    private CarControllerNoCheckpoints car_controller;

    [SerializeField]
    private Transform spawn_position;

    void Awake()
    {
        car_controller = GetComponent<CarControllerNoCheckpoints>();
    }

    void Start()
    {

    }

    void Update()
    {
        Vector3 ray_position_offset = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        RaycastHit hit_left;
        if (Physics.Raycast(ray_position_offset, transform.TransformDirection(Vector3.left), out hit_left, Mathf.Infinity, LayerMask.GetMask("Wall")))
        {
            Debug.DrawRay(ray_position_offset, transform.TransformDirection(Vector3.left) * hit_left.distance, Color.yellow);
        }

        RaycastHit hit_right;
        if (Physics.Raycast(ray_position_offset, transform.TransformDirection(Vector3.right), out hit_right, Mathf.Infinity, LayerMask.GetMask("Wall")))
        {
            Debug.DrawRay(ray_position_offset, transform.TransformDirection(Vector3.right) * hit_right.distance, Color.yellow);
        }

        AddReward(-hit_left.distance * 0.05f);
        AddReward(-hit_right.distance * 0.05f);

        float hit_difference = Mathf.Abs(hit_left.distance - hit_right.distance);
        if(hit_difference >= 0.25f)
            AddReward(0.1f / hit_difference);

        AddReward(car_controller.GetLocalVelocity.z * 0.5f);
    }

    public override void OnEpisodeBegin()
    {
        transform.position = spawn_position.position + new Vector3(Random.Range(-3.0f, 3.0f), 0, Random.Range(-3.0f, 3.0f));
        transform.forward = spawn_position.forward;
        car_controller.Stop();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(car_controller.GetSteer);
        sensor.AddObservation(car_controller.GetLocalVelocity);
        sensor.AddObservation(car_controller.GetLocalAngularVelocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forward_amount = 0.0f;
        float turn_amount = 0.0f;

        switch(actions.DiscreteActions[0])
        {
            case 0: forward_amount = 0.0f; break;
            case 1: forward_amount = 1.0f; break;
            case 2: forward_amount = -1.0f; break;

            default: break;
        }

        switch (actions.DiscreteActions[1])
        {
            case 0: turn_amount = 0.0f; break;
            case 1: turn_amount = 1.0f; break;
            case 2: turn_amount = -1.0f; break;

            default: break;
        }

        car_controller.SetInputs(forward_amount, turn_amount);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forward_action = 0;
        if (Input.GetKey(KeyCode.UpArrow)) forward_action = 1;
        if (Input.GetKey(KeyCode.DownArrow)) forward_action = 2;

        int turn_action = 0;
        if (Input.GetKey(KeyCode.RightArrow)) turn_action = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) turn_action = 2;

        ActionSegment<int> discrete_actions = actionsOut.DiscreteActions;

        discrete_actions[0] = forward_action;
        discrete_actions[1] = turn_action;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-0.5f);
            EndEpisode();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-0.1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<SpeedZone>(out SpeedZone speed_zone))
        {
            car_controller.Boost();
            AddReward(0.25f);
        }
    }
}
