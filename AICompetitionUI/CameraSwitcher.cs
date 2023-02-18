using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    private CameraFollowController camFollow;

    [SerializeField]
    private Transform[] cars;

    private int carSelected = 0;

    private void Start()
    {
        camFollow = GetComponent<CameraFollowController>();
        if(cars.Length >0)
            camFollow.objectToFollow = cars[0];
    }

    public void FollowCar(int index)
	{
        if(index!= carSelected && index < cars.Length)
		{
            camFollow.objectToFollow = cars[index];
            carSelected = index;
		}
	}
}
