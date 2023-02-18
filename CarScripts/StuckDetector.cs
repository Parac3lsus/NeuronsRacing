using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckDetector : MonoBehaviour
{
	private Rigidbody rb;
	public float timer = 0.0f;
	private CarRespawner respawner;

	private void Start()
	{
		rb = this.GetComponent<Rigidbody>();
		respawner = GetComponent<CarRespawner>();
	}


	private void Update()
	{
		if (rb.velocity.magnitude < 3)
			timer += Time.deltaTime;
		else
			timer = 0;

		if (timer >= 3.0f)
		{
			respawner.Respawn();
			timer = 0;
		}
	}
}
