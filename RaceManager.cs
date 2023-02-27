using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaceManager : MonoBehaviour
{
	[SerializeField]
	private ProgressDetector playerProgress;
    [SerializeField]
    private ProgressDetector[] carsProgress;
	[SerializeField]
	private int totalCheckpoints;
	[SerializeField]
	private int totalLaps;
	[SerializeField]
	private bool ExpectatorMode = false;
	[SerializeField]
	private PlayerUI uI;
	
	private int currentPlayeerPosition;
	private int currentPlayerLap;
	private Rigidbody rb;
	private int speed;
	public Scores[] scores;
	private bool positionChange;

	private void Start()
	{
		currentPlayeerPosition = 1;
		if (ExpectatorMode)
		{
			playerProgress = carsProgress[0];
			playerProgress.position = 1;
		}
		
		rb = playerProgress.gameObject.GetComponent<Rigidbody>();
		scores = new Scores[carsProgress.Length];
		AssignTotalCheckpoints();
		uI.UpdatePosition(currentPlayeerPosition, carsProgress.Length);
		//DebugScores();
	}

	private void DebugScores()
	{
		Debug.Log("#######################################################");
		for (int i =0; i < scores.Length; i++)
		{
			Debug.Log("Element: " + i + " Index: " + scores[i].index + " Score: " + scores[i].score);
		}
		Debug.Log("#######################################################");
	}

	private void DebugDistance()
	{
		Debug.Log("#######################################################");
		Debug.Log("Distance to next Checkpoint: " + playerProgress.DistanceToNextCheckpoint());
		Debug.Log("#######################################################");
	}

	private void Update()
	{
		if (currentPlayerLap != playerProgress.laps)
		{
			currentPlayerLap = playerProgress.laps;
			uI.UpdateLapsText(currentPlayerLap);
		}
		uI.UpdateSpeed(speed);
		//DebugDistance();
	}

	private void AssignTotalCheckpoints()
	{
		int i = 0;
		foreach (ProgressDetector p in carsProgress)
		{
			p.totalCheckpoints = totalCheckpoints;
			scores[i].index = i;
			i++;
		}
	}

	public struct Scores
	{
		public int index;
		public float score;
	}

	private void AssignScores()
	{
		for(int i=0; i < scores.Length; i++)
		{
			scores[i].score = carsProgress[scores[i].index].laps * 1000 + carsProgress[scores[i].index].checkpoints * 100
							- carsProgress[scores[i].index].DistanceToNextCheckpoint();
		}
		//DebugScores();
	}
	
	private void SortScores()
	{
		positionChange = false;
		for(int i=0; i< scores.Length-1; i++)
		{
			for(int k=i+1; k < scores.Length; k++)
			{
				if(scores[i].score < scores[k].score)
				{
					Scores auxScore = scores[i];
					scores[i] = scores[k];
					scores[k] = auxScore;
					positionChange = true;
				}
			}
		}
	}

	private void UpdatePositions()
	{
		for(int i =0; i < scores.Length; i++)
		{
			carsProgress[scores[i].index].position = i + 1;
		}
		//DebugScores();

	}
	private void AssignPositions()
	{
		AssignScores();
		SortScores();
		if (positionChange)
		{
			UpdatePositions();
		}
	}

	private void FixedUpdate()
	{
		speed = (int)playerProgress.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 10;
		AssignPositions();
		uI.UpdatePosition(playerProgress.position, carsProgress.Length);
	}

	public void AssignSelectedCar(int index)
	{
		if (index < carsProgress.Length)
			playerProgress = carsProgress[index];
		UpdatePositions();
	}
}
