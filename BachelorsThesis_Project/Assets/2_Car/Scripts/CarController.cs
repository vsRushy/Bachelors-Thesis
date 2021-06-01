﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public List<Axle> axles;
    public float max_motor_torque;
    public float max_steering_angle;
    
    private Rigidbody rb;

    private float forward_amount;
    private float turn_amount;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        foreach(Axle axle in axles)
        {
            if(axle.is_motor)
            {
                axle.left_wheel_collider.motorTorque = forward_amount;
                axle.right_wheel_collider.motorTorque = forward_amount;
            }

            if(axle.is_steering)
            {
                axle.left_wheel_collider.steerAngle = turn_amount;
                axle.right_wheel_collider.steerAngle = turn_amount;
            }

            UpdateWheelTransform(axle.left_wheel, axle.left_wheel_collider);
            UpdateWheelTransform(axle.right_wheel, axle.right_wheel_collider);
        }
    }

    private void UpdateWheelTransform(GameObject wheel, WheelCollider wheel_collider)
    {
        Vector3 position;
        Quaternion rotation;
        wheel_collider.GetWorldPose(out position, out rotation);

        wheel.transform.position = position;
        wheel.transform.rotation = rotation;
    }

    public void SetInputs(float fa, float ta)
    {
        forward_amount = fa * max_motor_torque;
        turn_amount = ta * max_steering_angle;
    }

    public void Stop()
    {
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
}

[System.Serializable]
public class Axle
{
    public GameObject left_wheel;
    public GameObject right_wheel;

    public WheelCollider left_wheel_collider;
    public WheelCollider right_wheel_collider;

    public bool is_motor;
    public bool is_steering;
}