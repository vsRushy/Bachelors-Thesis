using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField]
    Transform target_transform;

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move_x = actions.ContinuousActions[0];
        float move_z = actions.ContinuousActions[1];

        float move_speed = 5.0f;
        transform.localPosition += new Vector3(move_x, 0.0f, move_z) * Time.deltaTime * move_speed;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target_transform.localPosition);
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-4.5f, 1.5f), 1.0f, Random.Range(-4.5f, 4.5f));
        target_transform.localPosition = new Vector3(Random.Range(-4.5f, 4.5f), 1.0f, Random.Range(-4.5f, 4.5f));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuous_actions = actionsOut.ContinuousActions;
        continuous_actions[0] = Input.GetAxisRaw("Horizontal");
        continuous_actions[1] = Input.GetAxisRaw("Vertical");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(1.0f);
            EndEpisode();
        }

        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }
}
