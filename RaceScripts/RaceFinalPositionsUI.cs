using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceFinalPositionsUI : MonoBehaviour
{
	[SerializeField]
	private TMPro.TextMeshProUGUI[] driverNameText;
	//[SerializeField]
	//private GameObject RacingDataUI;
	[SerializeField]
	private GameObject FinalPositionsUI;

	private RaceManager raceManager;
	private List<ProgressDetector> finalPositions;


	private void Start()
	{
		raceManager = GetComponent<RaceManager>();
		FinalPositionsUI.SetActive(false);
	}


	public void ShowFinalPositions()
	{
		finalPositions = raceManager.GetPositions();

		for(int i = 0; i < finalPositions.Count; i++)
		{
			Debug.Log("Position: " + i+1 + "Car Name: " + finalPositions[i].carName);
			driverNameText[i].text = finalPositions[i].carName;
		}
		//RacingDataUI.SetActive(false);
		FinalPositionsUI.SetActive(true);


	}
}
