using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
	[SerializeField]
	private AudioSource gameMusic;

	public static GameMusic instance;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
			gameMusic.Play();
		}
		else if(instance != null)
		{
			Destroy(this.gameObject);
		}
	}

	/*private void Start()
	{
		if (!gameMusic.isPlaying)
		{
			gameMusic.Play();
		}
		
	}*/
}
