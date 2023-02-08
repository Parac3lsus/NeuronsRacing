using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class PopulationManager : MonoBehaviour
{
    public GameObject botPrefab;
    public int populationSize = 100;
    public float elapsed = 0;

    private StartPositions positions;
    private List<GameObject> population = new List<GameObject>();
    private int generation = 1;
    private int deadAgents = 0;

    private bool saveGenes = false;

    GUIStyle guiStyle = new GUIStyle();

    private void OnGUI()
    {
        guiStyle.fontSize = 45;
        guiStyle.normal.textColor = Color.green;
        GUI.BeginGroup(new Rect(10, 10, 350, 250));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 35, 100, 20), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 75, 100, 20), "Time: " + (int)elapsed, guiStyle);
        GUI.Label(new Rect(10, 115, 300, 20), "Population:" + (population.Count-deadAgents), guiStyle);
        GUI.EndGroup();
    }

    private void Start()
    {
        positions = FindObjectOfType<StartPositions>();
        int x = 0;
        for (int i = 0; i < populationSize; i++)
        {
            
            GameObject b = Instantiate(botPrefab, positions.transforms[x].position, positions.transforms[x].rotation);
            //GameObject b = Instantiate(botPrefab, this.transform.position, this.transform.rotation);
            b.GetComponent<Brain>().Init(true);
            population.Add(b);
            x++;
            if (x >= positions.transforms.Length) x = 0;
        }
    }


    GameObject Breed(GameObject parent1, GameObject parent2, int index)
    {

        GameObject offspring = Instantiate(botPrefab, positions.transforms[index].position, positions.transforms[index].rotation);
        Brain b = offspring.GetComponent<Brain>();
        if (Random.Range(0, 10) == 1) //Mutate 1 in 10
        {
            Debug.Log("Mutant!");
            b.Init(false);
            b.dna.Mutate(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }
        else
        {
            b.Init(false);
            b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }
        return offspring;
    }

    void BreeedNewPopulation()
    {
        List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<ProgressDetector>().checkpoints).ToList();
        population.Clear();
        int Mom = 0;
        int x = 0;
        for (int i = 0; i < populationSize; i++)
        {
            //Randomly select one parent from the top 25%
            int luckyDad = Random.Range(0, (int)populationSize / 4);
            population.Add(Breed(sortedList[luckyDad], sortedList[Mom],x));
            Mom++;
            if (Mom > (int)populationSize / 10)
                Mom = 0;
            x++;
            if (x >= positions.transforms.Length) x = 0;

        }
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    private void Update()
    {
        if (deadAgents>= populationSize)
		{
            deadAgents = 0;
            if (saveGenes)
			{
                Debug.Log("Saving Genes");
                SaveGenes();
			}
            BreeedNewPopulation();
            elapsed = 0;
        }
		if (Input.GetKeyDown(KeyCode.S))
		{
            saveGenes = true;
        }

        elapsed += Time.deltaTime;
    }

    private void SaveGenes()
	{
        List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<Brain>().timeAlive).ToList();
        List<DNA> dnaList = new List<DNA>();
        sortedList[0].GetComponent<Brain>().SaveGenesToFile(); //We save the Genes of the best performer
        for (int i = 0; i < (int)populationSize/4; i++)
        {
            //Save top 25% DNAs to DNA list
            dnaList.Add(sortedList[i].GetComponent<Brain>().dna);
        }
    }

    public void DeadAgent()
	{
        deadAgents += 1;
	}
}
