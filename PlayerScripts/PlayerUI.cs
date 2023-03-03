using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	[SerializeField]
	private TMPro.TextMeshProUGUI lapsText;
	[SerializeField]
	private TMPro.TextMeshProUGUI speedText;
	[SerializeField]
	private TMPro.TextMeshProUGUI positionText;
	[SerializeField]
	private TMPro.TextMeshProUGUI driverNameText;
	[SerializeField]
	private GameObject flag;

	private void Start()
	{
		flag.SetActive(false);
	}

	public void UpdateLapsText(int lapNr)
	{
		lapsText.text = lapNr.ToString() + "/3";
	}
	public void UpdateSpeed(int speed)
	{
		speedText.text = speed.ToString() + "/MPH";
	}

	public void UpdatePosition(int pos, int nrOfPlayers)
	{
		positionText.text = pos.ToString() + "/" + nrOfPlayers.ToString();
	}

	public void ShowFlag()
	{
		flag.SetActive(true);
	}

	public void UpdateDriverName(string name)
	{
		driverNameText.text = name;
	}

}
