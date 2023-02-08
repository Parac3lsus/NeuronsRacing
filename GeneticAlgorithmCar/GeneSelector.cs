using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneSelector : MonoBehaviour
{
    private BrainSensorySystem sensorySystem;
    

    void Start()
    {
        sensorySystem = GetComponent<BrainSensorySystem>();
    }

	public int GetGeneActivation()
	{
		if (sensorySystem.crashLeft)
		{
			//Potential Crash Left
			return 1; 
		}
		else if(sensorySystem.crashRight)
		{
			//Potential Crash Right
			return 2; 
		}
		else if (sensorySystem.freePathForward)
		{
			//Free Path Forward
			return 3; 
		}
		else if (sensorySystem.freePathLeft)
		{
			//Free Path Left
			return 4; 
		}
		else if (sensorySystem.freePathRight)
		{
			//Free Path Right
			return 5; 
		}
		else
		{
			// If no condition is met, we continue as if we have a Free Path Right
			return 3; 
		}
	}

	public bool CrashFront()
	{
		//If the car detects a possible frontal crash the gene 0 will be triggered responsable for car brake
		//otherwise acceleration will be constant in order to speed up evolution process
		return sensorySystem.crashFront;
	}
}
