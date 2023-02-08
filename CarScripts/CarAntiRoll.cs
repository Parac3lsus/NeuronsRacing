using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAntiRoll : MonoBehaviour
{
	public float antiRoll = 5000.0f;
	public WheelCollider wheelLFront;
	public WheelCollider wheelRFront;
	public WheelCollider wheelLBack;
	public WheelCollider wheelRBack;

	private Rigidbody rb;
	private float maxFloorDistance = 0.5f;

	private void Start()
	{
		rb = this.GetComponent<Rigidbody>();
	}

	private void GroundWheel(WheelCollider W)
	{
		RaycastHit rHit;
		Ray downRay = new Ray(W.transform.position, -Vector3.up);

		if(Physics.Raycast(downRay, out rHit))
		{
			if (rHit.distance > maxFloorDistance)
			{
				rb.AddForceAtPosition(W.transform.up * -antiRoll, W.transform.position);
			}
		}
	}

	private void FixedUpdate()
	{
		GroundWheel(wheelLFront);
		GroundWheel(wheelRFront);
		GroundWheel(wheelLBack);
		GroundWheel(wheelRBack);
	}
}
