using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Brain : MonoBehaviour
{
	private int DNALenght = 6;

	private bool firstFrame = true;

	private bool alive = true;
	private Rigidbody rb;
	private Collider collider;
	private MeshCollider meshCollider;

	private BrainSensorySystem sensorySystem;
	private GeneSelector geneSelector;
	private StuckDetector stuckDetector;

	[HideInInspector]
	public DNA dna;
	[HideInInspector]
	public float timeAlive = 0.0f;
	[HideInInspector]
	public PopulationManager popManager;
	[HideInInspector]
	public CarDrive carController;


	public void Init(bool loadF)
	{
		//Initialize DNA
		//0 Brake
		//1 Crash Left
		//2 Crash Right
		//3 Free Path Forward
		//4 Free Path Left
		//5 Free Path Right

		dna = new DNA(DNALenght, -1,1);
		if (loadF)
		{
			LoadGenesFromFile();
		}

		rb = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();
		meshCollider = GetComponentInChildren<MeshCollider>();
		carController = GetComponent<CarDrive>();
		sensorySystem = GetComponent<BrainSensorySystem>();
		geneSelector = GetComponent<GeneSelector>();
		popManager = FindObjectOfType<PopulationManager>();
		stuckDetector = GetComponent<StuckDetector>();
	}

	public void Start()
	{
		Init(true);
	}

	private void DisableCar() {
		//this function is not implemented during gameplay
		popManager.DeadAgent();
		rb.constraints = RigidbodyConstraints.FreezeAll;
		alive = false;
		collider.enabled = false;
		meshCollider.enabled = false;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Track")) return;
		if (popManager)
		{
			//If we are Evolving(training)
			//in case of crash the car gets disabled
			DisableCar();		
		}
	}

	private void Update()
	{
		if (!alive) return;

		sensorySystem.GetEnviormentData();

		timeAlive += Time.deltaTime;

	}
	
	private void FixedUpdate()
	{
		if (firstFrame) { firstFrame = false; return; }
		if (!alive) { }

		float accel = 1.0f;
		float steer;
		int activeGene;

		activeGene = geneSelector.GetGeneActivation();
		
		steer = dna.GetGene(activeGene);

		if (steer > -0.25f && steer < 0.25f) //We consider any value between -0.2 and 0.2 to be straight
			steer = 0;

		if (geneSelector.CrashFront())
		{
			accel = dna.GetGene(0);
		}


		carController.ProcessInputs(accel, steer);
	}

	public void SaveGenesToFile()
	{
		string path = Application.dataPath + "/NewGenes2.txt";
		StreamWriter gf = File.CreateText(path);
		gf.WriteLine(dna.PrintGenes());
		gf.Close();		
	}

	private void LoadGenesFromFile()
	{
		string path = Application.dataPath + "/NewGenes2.txt";
		StreamReader wf = File.OpenText(path);

		if (File.Exists(path))
		{
			string line = wf.ReadLine();
			//Debug.Log("Gene Line: " + line);
			dna.LoadGenes(line);
		}
	}

}
