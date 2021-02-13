using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public List<Axle> axles;
    public float max_motor_torque;
    public float max_steering_angle;

    void FixedUpdate()
    {
        float motor = Input.GetAxisRaw("Vertical") * max_motor_torque;
        float steering = Input.GetAxisRaw("Horizontal") * max_steering_angle;

        foreach(Axle axle in axles)
        {
            if(axle.is_motor)
            {
                axle.left_wheel_collider.motorTorque = motor;
                axle.right_wheel_collider.motorTorque = motor;
            }

            if(axle.is_steering)
            {
                axle.left_wheel_collider.steerAngle = steering;
                axle.right_wheel_collider.steerAngle = steering;
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

    public void SetInputs()
    {

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