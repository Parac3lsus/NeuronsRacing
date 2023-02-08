using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProgressDetector : MonoBehaviour
{
	//Total Checkpoints will be assigned by the RaceManager at Start
	public Transform nextCheckpointTransform;
	public int totalCheckpoints;
	public int laps;
	public int position;
	public int checkpoints;
	
	private int nextCheckPointNumber = 1;
	private Transform thisTransform;

	private void Start()
	{
		thisTransform = this.gameObject.transform;
	}



	public float DistanceToNextCheckpoint()
	{
		float distance = Vector3.Distance(thisTransform.position, nextCheckpointTransform.gameObject.transform.position)/10;
		return (float)(Math.Round(distance,2));
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Checkpoint")
		{
			if(other.gameObject.TryGetComponent<CheckPoint>(out var checkPoint))
			{
				int chkpNumber = checkPoint.GetCheckpointNumber();
				if(chkpNumber == nextCheckPointNumber)
				{
					checkpoints += 1;
					nextCheckPointNumber += 1;
					nextCheckpointTransform = checkPoint.GetNextCheckpointTransform();
					if(checkpoints >= totalCheckpoints)
					{
						laps += 1;
						checkpoints = 0;
						nextCheckPointNumber = 1;
					}
				}
			}
		}
	}
}
