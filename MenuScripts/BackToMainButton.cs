using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainButton : MonoBehaviour
{
	[SerializeField]
	private string mainMenu = "MainMenu";
	public void BackToMain()
	{
		SceneManager.LoadScene(mainMenu);
	}
}
