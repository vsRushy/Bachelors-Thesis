using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3CarController : MonoBehaviour
{
    public List<Axle> axles;
    public float max_motor_torque;
    public float max_steering_angle;
    public float max_braque_torque;
    public float max_speed;

    private Rigidbody rb;

    private float current_speed;

    private float forward_amount;
    private float turn_amount;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        current_speed = 0.0f;
    }

    void FixedUpdate()
    {
        current_speed = rb.velocity.sqrMagnitude;

        foreach(Axle axle in axles)
        {
            if(axle.is_motor)
            {
                if (forward_amount < 0.0f)
                {
                    rb.drag = 1;
                }
                else
                {
                    rb.drag = 0;
                }

                if (current_speed < max_speed)
                {
                    axle.left_wheel_collider.motorTorque = forward_amount * max_motor_torque;
                    axle.right_wheel_collider.motorTorque = forward_amount * max_motor_torque;
                }
                else
                {
                    axle.left_wheel_collider.motorTorque = 0.0f;
                    axle.right_wheel_collider.motorTorque = 0.0f;
                }
            }
            
            if(axle.is_steering)
            {
                axle.left_wheel_collider.steerAngle = turn_amount * max_steering_angle;
                axle.right_wheel_collider.steerAngle = turn_amount * max_steering_angle;
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
        forward_amount = fa;
        turn_amount = ta;
    }

    public void Stop()
    {
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
}

[System.Serializable]
public class P3Axle
{
    public GameObject left_wheel;
    public GameObject right_wheel;

    public WheelCollider left_wheel_collider;
    public WheelCollider right_wheel_collider;

    public bool is_motor;
    public bool is_steering;
}