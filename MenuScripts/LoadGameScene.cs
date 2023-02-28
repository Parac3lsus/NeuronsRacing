using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
	[SerializeField]
	private string playerRace = "PlayerRace";
	[SerializeField]
	private string iaRace = "IACompetition";
	public void LoadSlectedScene(int selectedScene)
	{
		if (selectedScene == 1)
			SceneManager.LoadScene(playerRace);
		if (selectedScene == 2)
			SceneManager.LoadScene(iaRace);
	}
}
