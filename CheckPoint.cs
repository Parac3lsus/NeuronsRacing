using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	[SerializeField]
	private int checkpointNumber;

	[SerializeField]
	private Transform nextCheckPoint;

	public int GetCheckpointNumber()
	{
		return checkpointNumber;
	}

	public Transform GetNextCheckpointTransform()
	{
		return nextCheckPoint;
	}
}
