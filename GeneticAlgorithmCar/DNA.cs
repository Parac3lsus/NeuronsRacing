using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class DNA 
{
    List<float> genes = new List<float>();
    int dnaLenght = 0;
    float minValue = 0;
	float maxValue = 0;

    public DNA(int l, int minV, int maxV)
	{
        dnaLenght = l;
		minValue = minV;
        maxValue = maxV;
        SetRandom();
	}

    public void SetRandom()
	{
        genes.Clear();
        for(int i = 0; i < dnaLenght; i++)
		{
            genes.Add(Random.Range(minValue, maxValue));
		}
	}

    public void SetFloat(int pos, float value)
	{
        genes[pos] = value;
	}

	public void Combine(DNA d1, DNA d2)
	{
		for (int i = 0; i < dnaLenght; i++)
		{
			//selecting genes from both parents randomly
			int lifeDice = Random.Range(0, 2);
			if (lifeDice == 0)
			{
				genes[i] = d1.genes[i];
			}
			else
			{
				genes[i] = d2.genes[i];
			}
		}
	}

	public void Mutate(DNA d1, DNA d2)
	{
		Combine(d1, d2);
		genes[Random.Range(0, dnaLenght)] = Random.Range(minValue, maxValue);
	}

	public float GetGene(int pos)
	{
		return genes[pos];
	}

	public string PrintGenes()
	{
		string genesStr = "";
		foreach (float g in genes)
		{
			genesStr += g + "/";
		}
		return genesStr;
	}

	public void LoadGenes(string genesString)
	{
		if (genesString == "") return;

		var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
		culture.NumberFormat.NumberDecimalSeparator = ",";
		string[] geneValues = genesString.Split('/');

		for(int i = 0; i< geneValues.Length;i++ )
		{
			if (i < dnaLenght)
			{
				genes[i] = float.Parse(geneValues[i], culture);
			}
		}
	}
}
