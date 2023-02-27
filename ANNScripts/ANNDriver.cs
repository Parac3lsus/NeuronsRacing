using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class ANNDriver : MonoBehaviour
{
    [HideInInspector]
    public CarDrive carController;

    //We use the same Sensory system used by the Genetic Algorithm car
    //but instead of using boolenas for freepath or imminent crash
    //we use the hit distance of the sensors and -1 in case of free path
    private BrainSensorySystem sensorySystem;

    //We make use of progress detector in order to reward the progress 
    private ProgressDetector pDetector;                        

    private ANN ann;
    
    private void Start()
    {
        carController = GetComponent<CarDrive>();
        sensorySystem = GetComponent<BrainSensorySystem>();
        ann = new ANN(3, 3, 1, 3, 0.2f);
        LoadWeightsFromFile();
    }

    // Update is called once per frame
    private void Update()
    {
        sensorySystem.GetEnviormentData();
    }

	private void FixedUpdate()
	{
        List<double> states = new List<double>();
        List<double> qs = new List<double>();
        float steer = 0.0f;

        states.Add(sensorySystem.HitDistanceForward);
        states.Add(sensorySystem.HitDistanceLeft);
        states.Add(sensorySystem.HitDistanceRight);

        qs = SoftMax(ann.CalcOutput(states));
        double maxQ = qs.Max();
        int maxQIndex = qs.ToList().IndexOf(maxQ);

        switch (maxQIndex)
        {
            case 0: steer = -1; break;
            case 1: steer = 1; break;
            case 2: steer = 0; break;
            default: steer = 0; break;
        }

        carController.ProcessInputs(1, steer);
    }

	List<double> SoftMax(List<double> values)
    {
        double max = values.Max();

        float scale = 0.0f;

        for (int i = 0; i < values.Count; i++)
            scale += Mathf.Exp((float)(values[i] - max));

        List<double> result = new List<double>();
        for (int i = 0; i < values.Count; i++)
            result.Add(Mathf.Exp((float)(values[i] - max)) / scale);

        return result;
    }

    private void LoadWeightsFromFile()
    {
        string path = Application.dataPath + "/Weights2.txt";
        StreamReader wf = File.OpenText(path);

        if (File.Exists(path))
        {
            string line = wf.ReadLine();
            //Debug.Log("Weights Line: " + line);
            ann.LoadWeights(line);
        }
    }
}
