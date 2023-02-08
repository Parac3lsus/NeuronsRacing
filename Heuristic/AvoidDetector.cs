using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidDetector : MonoBehaviour
{
	public float avoidPath = 0;
	public float avoidTime = 0;
	public float wanderDistance = 4;
	public float avoidLength = 1;
	public bool reverse = false;
	Rigidbody rb;

	private void Start()
	{
		rb = this.GetComponent<Rigidbody>();
	}

	private void OnTriggerExit(Collider other)
	{
		reverse = false;
		if (other.gameObject.tag != "car") return;
		avoidTime = 0;
	}

	private void OnTriggerStay(Collider other)
	{
		//We check if the object detected is ahead of us
		Vector3 collisionDir = this.transform.InverseTransformPoint(other.gameObject.transform.position);
		if(collisionDir.x>0 && collisionDir.z > 0)
		{

			if (rb.velocity.magnitude < 1)
			{
				reverse = true;
			}
			else if (other.gameObject.tag == "car")
			{
				Rigidbody otherCar = other.GetComponent<Rigidbody>();
				avoidTime = Time.time + avoidLength;

				Vector3 otherCarLocalTarget = transform.InverseTransformPoint(other.gameObject.transform.position);
				float otherCarAngle = Mathf.Atan2(otherCarLocalTarget.x, otherCarLocalTarget.z);
				avoidPath = wanderDistance * -Mathf.Sign(otherCarAngle);
			}
		}

	}

}
