using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEndDetector : MonoBehaviour
{
    private RaceFinalPositionsUI positionsUI;
    private int carsFinished = 0;

    private void OnEnable()
    {
        ProgressDetector.OnRaceFinished += CheckRaceFinished;
    }

	private void Start()
	{
        positionsUI = GetComponent<RaceFinalPositionsUI>();
	}

	private void CheckRaceFinished()
    {
        carsFinished += 1;
        if(carsFinished >= 3)
		{
            positionsUI.ShowFinalPositions();
		}
        Debug.Log("Check race Finished");
    }
    private void OnDisable()
    {
        ProgressDetector.OnRaceFinished -= CheckRaceFinished;
    }
}
