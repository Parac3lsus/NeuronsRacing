﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCountDown : MonoBehaviour
{
	[SerializeField]
	private GameObject redLight;
	[SerializeField]
	private GameObject yellowLight;
	[SerializeField]
	private GameObject greenLight;

	private CarDrive[] carDriveControllers;


	private void Start()
	{
		DisableLights();
		carDriveControllers = FindObjectsOfType<CarDrive>();
		SetCarsActiveState(false);
		StartCoroutine(CountDownRoutine());
		//SetCarsActiveState(true);
	}

	private void SetCarsActiveState(bool enabled)
	{
		foreach(CarDrive driveController in carDriveControllers)
		{
			driveController.carEnabled =enabled;
			driveController.GetComponent<StuckDetector>().enabled = enabled;
		}
	}

	private void DisableLights()
	{
		redLight.SetActive(false);
		yellowLight.SetActive(false);
		greenLight.SetActive(false);
	}


	IEnumerator CountDownRoutine()
	{
		yield return new WaitForSeconds(1.5f);
		redLight.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		yellowLight.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		greenLight.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		DisableLights();
		SetCarsActiveState(true);
	}
}
