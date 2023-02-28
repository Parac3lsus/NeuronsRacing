using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDrive : MonoBehaviour
{	
	[Header("Car Settings")]
	public float maxSteerAngle = 30f;
	public float maxSpeed = 15;
	public float maxBackSpeed = 6;
	public float brakeForce = 600;
	public float torque = 400;
	public float turnSensitivity = 1.0f;

	[SerializeField]
	private GameObject[] brakeLights;

	private WheelDrive[] wheels;
	private Rigidbody rb;

	public bool carEnabled = false;

	private void Start()
	{
		wheels = GetComponentsInChildren<WheelDrive>();
		rb = GetComponent<Rigidbody>();
	}

	private float GetAcceleration(float accel)
	{
		accel = Mathf.Clamp(accel, -1, 1);
		float thrustTorque = accel * torque;
		return thrustTorque;
	}

	private float GetSteer(float steer, float currentSteerAngle)
	{
		var _steerAngle = steer * turnSensitivity * maxSteerAngle;
		return Mathf.Lerp(currentSteerAngle, _steerAngle, 0.5f);
	}

	private void ProcessForwardInput(WheelDrive wheel, float accel)
	{
		if(rb.velocity.magnitude < maxSpeed)
		{
			wheel.Move(GetAcceleration(accel));
		}
		else
		{
			wheel.Move(0.00f);
		}
	}

	private void SetLightsActive(bool active)
	{
		foreach (GameObject light in brakeLights)
		{
			light.SetActive(active);
		}
	}

	private void ProcessBackwardInput(WheelDrive wheel, float accel, bool forward)
	{
		if (forward)
		{
			wheel.WheelBrake(brakeForce);
			SetLightsActive(true);
		}
		else
		{
			if (rb.velocity.magnitude < maxBackSpeed)
			{
				wheel.Move(GetAcceleration(accel));
			}
		}
	}

	private void ClearTorques(WheelDrive wheel)
	{
		wheel.ClearTorque();
		SetLightsActive(false);
	}

	public void ProcessInputs(float accel, float steer)
	{
		if (!carEnabled) return;
		float dotProduct = Vector3.Dot(this.gameObject.transform.forward, rb.velocity);
		bool goingForward = (dotProduct >= 0.01f);

		foreach (WheelDrive wheel in wheels)
		{
			ClearTorques(wheel);

			if (accel > 0.01)
			{
				ProcessForwardInput(wheel, accel);
			}
			if (accel < -0.01)
			{
				ProcessBackwardInput(wheel, accel, goingForward);
			}

			if (wheel.canTurn)
			{
				wheel.Turn(GetSteer(steer, wheel.GetSteerAngle()));
			}

			wheel.UpdateMeshPosition();
		}
	}
}
