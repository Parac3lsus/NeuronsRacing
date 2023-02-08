using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFlip : MonoBehaviour
{
    [SerializeField]
    private float secondsToFlip = 3.0f;

    private Rigidbody rb;
    private float lastTimeChecked;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void FlipCar()
	{
        this.transform.position += Vector3.up;
        this.transform.rotation = Quaternion.LookRotation(this.transform.forward);
	}

    void Update()
    {
        if(transform.up.y > 0.5f || rb.velocity.magnitude > 1)
		{
            lastTimeChecked = Time.time;
		}

        if(Time.time > lastTimeChecked + secondsToFlip)
		{
            FlipCar();
		}
    }
}
