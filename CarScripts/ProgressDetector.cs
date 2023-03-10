using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProgressDetector : MonoBehaviour
{
	public String carName;
	//Total Checkpoints will be assigned by the RaceManager at Start
	public Transform nextCheckpointTransform;
	public Transform previousCheckpointTransform;
	public int totalCheckpoints;
	public int laps;
	public int position;
	public int checkpoints;
	public int totLaps = 4;
	public bool checkpointPassed = false;
	
	private int nextCheckPointNumber = 1;
	private Transform thisTransform;
	private CarRespawner respawner;

	public delegate void RaceFinished();
	public static event RaceFinished OnRaceFinished;
	public bool HumanPlayer = false;

	private void Start()
	{
		thisTransform = this.gameObject.transform;
		respawner = GetComponent<CarRespawner>();
	}


	public void ResetDetector()
	//To be used by ANNBrain during training
	{
		nextCheckPointNumber = 1;
		checkpoints = 0;
		laps = 0;
		checkpointPassed = false;
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
					checkpointPassed = true;
					checkpoints += 1;
					nextCheckPointNumber += 1;
					previousCheckpointTransform = nextCheckpointTransform;
					nextCheckpointTransform = checkPoint.GetNextCheckpointTransform();
					if(checkpoints >= totalCheckpoints)
					{
						laps += 1;
						checkpoints = 0;
						nextCheckPointNumber = 1;
						if (laps >= totLaps)
						{
							OnRaceFinished();
							if (HumanPlayer)
								HumanPlayerRaceFinished();
						}
					}
				}
			}
		}
	}

	private void HumanPlayerRaceFinished()
	{
		PlayerUI playerUI;
		playerUI = GetComponent<PlayerUI>();
		playerUI.ShowFlag();
	}
}
