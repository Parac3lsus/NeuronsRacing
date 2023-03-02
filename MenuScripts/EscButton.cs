using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscButton : MonoBehaviour
{
	[SerializeField]
	private GameObject ExitMenu;

	private RaceManager raceManager;
	private LoadGameScene sceneLoader;

	private void Start()
	{
		raceManager = FindObjectOfType<RaceManager>();
		sceneLoader = GetComponent<LoadGameScene>();
	}
	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			ExitMenu.SetActive(true);
			Time.timeScale = 0.0f;
		}
	}

	public void SelectedOption(int opt)
	{
		Time.timeScale = raceManager.timeScale;
		if (opt == 1)
		{
			sceneLoader.LoadSlectedScene(0);
		}
		else
		{
			ContinueExcecution();
		}
	}

	public void ContinueExcecution()
	{
		ExitMenu.SetActive(false);
	}
}
