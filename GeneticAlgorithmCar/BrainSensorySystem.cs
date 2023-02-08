using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainSensorySystem : MonoBehaviour
{
	[Header("Path Detectors")]
	[SerializeField]
	private GameObject leftEye;
	[SerializeField]
	private GameObject rightEye;
	public LayerMask raycastIgnore;
	[Header("Path Distance Visibility Settings")]
	public float driveFrontVisibleDistance;
	public float driveDiagonalVisibleDistance;

	[Header("Crash Detectors")]
	[SerializeField]
	private GameObject frontalCrashDetector;
	[SerializeField]
	private GameObject leftCrashDetector;
	[SerializeField]
	private GameObject rightCrashDetector;
	public LayerMask raycastCrashIgnore;
	[Header("Crash Distance Visibility Settings")]
	[SerializeField]
	private float frontCrashDetectorDistance = 1.0f;
	[SerializeField]
	private float lateralCrashDetectorDistance = 1.0f;

	[HideInInspector]
	public bool freePathForward;
	[HideInInspector]
	public bool freePathLeft;
	[HideInInspector]
	public bool freePathRight;
	[HideInInspector]
	public bool crashFront;
	[HideInInspector]
	public bool crashLeft;
	[HideInInspector]
	public bool crashRight;

	private void GetPathRaycast(ref RaycastHit forwardL, ref RaycastHit forwardR, ref RaycastHit hitLeft, ref RaycastHit hitRight)
	{
		// We have 2 raycast hits for the forward path
		// and we cast them using a small angle in order
		// to avoid the car getting to close to walls
		Physics.Raycast(leftEye.transform.position, Quaternion.AngleAxis(-5, Vector3.up) * leftEye.transform.forward, out forwardL, driveFrontVisibleDistance, ~raycastIgnore);
		Physics.Raycast(rightEye.transform.position, Quaternion.AngleAxis(5, Vector3.up) * rightEye.transform.forward, out forwardR, driveFrontVisibleDistance, ~raycastIgnore);

		//45 left
		Physics.Raycast(leftEye.transform.position, Quaternion.AngleAxis(45, Vector3.up) * -leftEye.transform.right,
						out hitLeft, driveDiagonalVisibleDistance, ~raycastIgnore);
		//45 Right
		Physics.Raycast(rightEye.transform.position, Quaternion.AngleAxis(-45, Vector3.up) * rightEye.transform.right,
						out hitRight, driveDiagonalVisibleDistance, ~raycastIgnore);
	}


	private void ProcessPathForward(RaycastHit hForwardL, RaycastHit hForwardR, RaycastHit hLeft, RaycastHit hRight)
	{
		if (hForwardL.collider == null && hForwardR.collider == null)
		{
			freePathForward = true;
			Debug.DrawRay(leftEye.transform.position, Quaternion.AngleAxis(-5, Vector3.up) * leftEye.transform.forward * driveFrontVisibleDistance, Color.green);
			Debug.DrawRay(rightEye.transform.position, Quaternion.AngleAxis(5, Vector3.up) * rightEye.transform.forward * driveFrontVisibleDistance, Color.green);
		}
		else
		{
			Debug.DrawRay(leftEye.transform.position, Quaternion.AngleAxis(-5, Vector3.up) * leftEye.transform.forward * driveFrontVisibleDistance, Color.red);
			Debug.DrawRay(rightEye.transform.position, Quaternion.AngleAxis(5, Vector3.up) * rightEye.transform.forward * driveFrontVisibleDistance, Color.red);
		}

		if (hLeft.collider == null)
		{
			freePathLeft = true;
			Debug.DrawRay(leftEye.transform.position, Quaternion.AngleAxis(45, Vector3.up) * -leftEye.transform.right * driveDiagonalVisibleDistance, Color.green);
		}
		else
		{
			Debug.DrawRay(leftEye.transform.position, Quaternion.AngleAxis(45, Vector3.up) * -leftEye.transform.right * driveDiagonalVisibleDistance, Color.red);
		}

		if (hRight.collider == null)
		{
			freePathRight = true;
			Debug.DrawRay(rightEye.transform.position, Quaternion.AngleAxis(-45, Vector3.up) * rightEye.transform.right * driveDiagonalVisibleDistance, Color.green);
		}
		else
		{
			Debug.DrawRay(rightEye.transform.position, Quaternion.AngleAxis(-45, Vector3.up) * rightEye.transform.right * driveDiagonalVisibleDistance, Color.red);
		}
	}
	private void GetCrashRaycast(ref RaycastHit forwardC, ref RaycastHit leftC, ref RaycastHit rightC)
	{
		Physics.Raycast(frontalCrashDetector.transform.position, frontalCrashDetector.transform.forward, 
						out forwardC, frontCrashDetectorDistance, ~raycastCrashIgnore);
		Physics.Raycast(leftCrashDetector.transform.position, -leftCrashDetector.transform.right, 
						out leftC, lateralCrashDetectorDistance, ~raycastCrashIgnore);
		Physics.Raycast(rightCrashDetector.transform.position, rightCrashDetector.transform.right, 
						out rightC, lateralCrashDetectorDistance, ~raycastCrashIgnore);
	}

	private void ProcessCrashDanger(RaycastHit hForward, RaycastHit hLeft, RaycastHit hRight)
	{
		if (hForward.collider == null)
		{
			Debug.DrawRay(frontalCrashDetector.transform.position, frontalCrashDetector.transform.forward * frontCrashDetectorDistance, Color.blue);
		}
		else
		{
			crashFront = true;
			Debug.DrawRay(frontalCrashDetector.transform.position, frontalCrashDetector.transform.forward * frontCrashDetectorDistance, Color.yellow);
		}
		if (hLeft.collider == null)
		{
			Debug.DrawRay(leftCrashDetector.transform.position, -leftCrashDetector.transform.right * lateralCrashDetectorDistance, Color.blue);
		}
		else
		{
			crashLeft = true;
			Debug.DrawRay(leftCrashDetector.transform.position, -leftCrashDetector.transform.right * lateralCrashDetectorDistance, Color.yellow);
		}
		if (hRight.collider == null)
		{
			Debug.DrawRay(rightCrashDetector.transform.position, rightCrashDetector.transform.right * lateralCrashDetectorDistance, Color.blue);
		}
		else
		{
			crashRight = true;
			Debug.DrawRay(rightCrashDetector.transform.position, rightCrashDetector.transform.right * lateralCrashDetectorDistance, Color.yellow);
		}
	}

	public void GetEnviormentData()
	{
		freePathForward = false;
		freePathLeft = false;
		freePathRight = false;
		crashFront = false;
		crashLeft = false;
		crashRight = false;

		RaycastHit hitPathForwardL =	new RaycastHit(), hitPathForwardR = new RaycastHit(), 
										hitPathLeft = new RaycastHit(), hitPathRight = new RaycastHit();
		GetPathRaycast(ref hitPathForwardL, ref hitPathForwardR, ref hitPathLeft, ref hitPathRight);
		

		RaycastHit hitCrashForward = new RaycastHit(), hitCrashLeft = new RaycastHit(), hitCrashRight = new RaycastHit();
		GetCrashRaycast(ref hitCrashForward, ref hitCrashLeft, ref hitCrashRight);
	
		ProcessPathForward(hitPathForwardL, hitPathForwardR, hitPathLeft, hitPathRight);
		ProcessCrashDanger(hitCrashForward, hitCrashLeft, hitCrashRight);
	}
}
