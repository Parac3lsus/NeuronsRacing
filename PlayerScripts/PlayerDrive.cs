using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrive : MonoBehaviour
{
    private CarDrive carController;
    private float inputX, inputY;

    void Start()
    {
        carController = GetComponent<CarDrive>();
    }

    void Update()
    {
        GetInputs();
    }

    private void FixedUpdate()
    {
        carController.ProcessInputs(inputY, inputX);
    }

    private void GetInputs()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }
}
