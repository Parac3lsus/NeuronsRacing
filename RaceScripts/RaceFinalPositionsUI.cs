using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceFinalPositionsUI : MonoBehaviour
{
	private RaceManager raceManager;
	private List<ProgressDetector> finalPositions;

	private void Start()
	{
		raceManager = GetComponent<RaceManager>();
	}


	public void ShowFinalPositions()
	{
		finalPositions = raceManager.GetPositions();

		for(int i = 0; i < finalPositions.Count; i++)
		{
			Debug.Log("Position: " + i+1 + "Car Name: " + finalPositions[i].carName);
		}
	}
}
