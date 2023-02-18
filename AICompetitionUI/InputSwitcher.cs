using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSwitcher : MonoBehaviour
{
    [SerializeField]
    private CameraSwitcher camSwitcher;
    [SerializeField]
    private RaceManager rManager;

    private void ProcessInputs()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            camSwitcher.FollowCar(0);
            rManager.AssignSelectedCar(0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            camSwitcher.FollowCar(1);
            rManager.AssignSelectedCar(1);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            camSwitcher.FollowCar(2);
            rManager.AssignSelectedCar(2);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            camSwitcher.FollowCar(3);
            rManager.AssignSelectedCar(3);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            camSwitcher.FollowCar(4);
            rManager.AssignSelectedCar(4);
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            camSwitcher.FollowCar(5);
            rManager.AssignSelectedCar(5);
        }
    }

    private void Update()
    {
        ProcessInputs();
    }
}
