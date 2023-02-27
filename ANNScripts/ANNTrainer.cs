using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Replay
{
    public List<double> states;
    public double reward;

    public Replay(double PathForward, double PathLeft, double PathRight,
                   double r)
    {
        states = new List<double>();
        states.Add(PathForward); states.Add(PathLeft); states.Add(PathRight);
        //states.Add(Crashforward); states.Add(CrashLeft); states.Add(CrashRight);
        reward = r;
    }
}

public class ANNTrainer : MonoBehaviour
{
    [HideInInspector]
    public CarDrive carController;


    //We use the same Sensory system used by the Genetic Algorithm car
    //but instead of using boolenas for freepath or imminent crash
    //we use the hit distance of the sensors and -1 in case of free path
    private BrainSensorySystem sensorySystem;

    private ANN ann;
    public float reward = 0.0f;                                //reward to associate with actions
    private List<Replay> replayMemory = new List<Replay>();     //memory - list of past actions and rewards
    private int mCapacity = 10000;                              // memory capacity

    public float discount = 0.99f;                             //how much future states affect rewards
    public float exploreRate = 15.0f;                         //chance of picking random action
    public float maxExploreRate = 100.0f;                      //max chance value
    public float minExploreRate = 0.01f;                       //min chance value
    public float exploreDecay = 0.0001f;                       //chance decay amount for each update

    public int crashCount = 0;                                  //We count when car crashes
    public float timer = 0;                                     //Timer to keep

    private Vector3 initialPosition;                            //We store initial position in order to respawn when car crashes
    private Quaternion initialRotation;                         //Same for initial rotation
    private bool crashed = false;                               //Flag used in case of crash

    private Rigidbody rb;                                       //We grab the rigid body so we can respawn and reset velocity in case of crash during training
    private ProgressDetector pDetector;                         //We make use of progress detector in order to reward the progress 





    private void Start()
    {
        carController = GetComponent<CarDrive>();
        sensorySystem = GetComponent<BrainSensorySystem>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        pDetector = GetComponent<ProgressDetector>();

        //We initialize our Neural Network with:
        //3 inputs - 3 Outputs (turn left or right)
        //1 Hidden Layers - 3 Neurons on each layer
        //Learning rate of 0.2
        ann = new ANN(3, 3, 1, 3, 0.2f);

        //We scale the time to accelerate training
        Time.timeScale = 5.0f;
    }


    private void Update()
    {
        sensorySystem.GetEnviormentData();
        if (Input.GetKeyDown(KeyCode.S))
            SaveWeightsToFile();
    }


    private void FixedUpdate()
    {
        float steer = 0.0f;
        timer += Time.deltaTime;
        List<double> states = new List<double>();
        List<double> qs = new List<double>();

        states.Add(sensorySystem.HitDistanceForward);
        states.Add(sensorySystem.HitDistanceLeft);
        states.Add(sensorySystem.HitDistanceRight);

        qs = SoftMax(ann.CalcOutput(states));
        double maxQ = qs.Max();
        int maxQIndex = qs.ToList().IndexOf(maxQ);
        exploreRate = Mathf.Clamp(exploreRate - exploreDecay, minExploreRate, maxExploreRate);

        if (Random.Range(0, 100) < exploreRate)
            maxQIndex = Random.Range(0, 2);

        switch (maxQIndex)
        {
            case 0: steer = -1; break;
            case 1: steer = 1; break;
            case 2: steer = 0; break;
            default: steer = 0; break;
        }

        CalculateReward();


        Replay lastMemory = new Replay(sensorySystem.HitDistanceForward, sensorySystem.HitDistanceLeft, sensorySystem.HitDistanceRight,
                                        reward);

        //We check if our memory is at full capacity, if so we remove the oldest memory
        if (replayMemory.Count > mCapacity)
            replayMemory.RemoveAt(0);
        //we add the current memory to the last of the list
        replayMemory.Add(lastMemory);


        //When we crash we do the actual training
        if (crashed)
        {
            //We loop through all the memories backwards so the last decisions receive more "blame" than the first ones
            for (int i = replayMemory.Count - 1; i >= 0; i--)
            {
                //List for storing Q values of the current Memory
                List<double> tOutputsOld = new List<double>();
                //List for storing Q values for next memory in the sequence
                List<double> tOutputsNew = new List<double>();
                tOutputsOld = SoftMax(ann.CalcOutput(replayMemory[i].states));

                //We store the best action of the current memory
                double maxQOld = tOutputsOld.Max();
                int action = tOutputsOld.ToList().IndexOf(maxQOld);

                double feedback;
                //If we are at the last memory (first one of our iteration)
                //Theres no more future actions to take so feedback is just the last reward
                if (i == replayMemory.Count - 1 || replayMemory[i].reward == -1)
                    feedback = replayMemory[i].reward;
                else
                {
                    //We calculate the Q values of next memory
                    tOutputsNew = SoftMax(ann.CalcOutput(replayMemory[i + 1].states));
                    //We calculate the max Q value of next memory
                    maxQ = tOutputsNew.Max();
                    //We use Bellman´s Equation (current reward + discount * max Q of next memory)
                    feedback = (replayMemory[i].reward + discount * maxQ);
                }
                //We use our feedback to train our network
                tOutputsOld[action] = feedback;
                ann.Train(replayMemory[i].states, tOutputsOld);
            }
            timer = 0;
            crashed = false;
            ResetCar();
            replayMemory.Clear();
            crashCount++;

        }
        carController.ProcessInputs(1, steer);
    }

    private void CalculateReward()
    {
        if (crashed)
            reward = -1.0f;
        else
        {
            if (pDetector.checkpointPassed)
            {
                pDetector.checkpointPassed = false;
            }
            else
                reward = 0.001f;
        }
    }

    private void ResetCar()
    {
        this.transform.position = initialPosition;
        this.transform.rotation = initialRotation;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
        pDetector.ResetDetector();
        reward = 0;
    }
    private void SaveWeightsToFile()
    {
        string path = Application.dataPath + "/Weights2.txt";
        StreamWriter gf = File.CreateText(path);
        gf.WriteLine(ann.PrintWeights());
        gf.Close();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Track")) return;

        //In case of crash we restablish original position and rotation
        Debug.Log("Crashed!");
        crashed = true;

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
}
