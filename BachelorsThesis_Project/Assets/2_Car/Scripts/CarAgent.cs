using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class CarAgent : MonoBehaviour
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
        
    }

    void Update()
    {
        
    }
}
