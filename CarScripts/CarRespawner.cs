using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRespawner : MonoBehaviour
{
	private ProgressDetector pDetector;

	private void Start()
	{
		pDetector = GetComponent<ProgressDetector>();
	}

	public void Respawn()
	{
		this.gameObject.transform.position = pDetector.previousCheckpointTransform.position;
		this.gameObject.transform.rotation = pDetector.previousCheckpointTransform.rotation;
	}

}
