using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    List<int> genes = new List<int>();
    int dnaLenght = 0;
    int maxValues = 0;

    //l = lenght dna
    //v = max value
    public DNA(int l, int v)
    {
        dnaLenght = l;
        maxValues = v;
        SetRandom();
    }

    public void SetRandom()
    {
        genes.Clear();
        for (int i = 0; i < dnaLenght; i++)
        {
            genes.Add(Random.Range(0, maxValues));
        }
    }

    //force values in pos
    public void SetInt(int pos, int value)
    {
        genes[pos] = value;
    }

    public void Combine(DNA d1, DNA d2)
    {
        //divided in 2
        for (int i = 0; i < dnaLenght; i++)
        {
            //firts half of the parent 1 to the offspring
            if (i < dnaLenght / 2.0)
            {
                int c = d1.genes[i];
                genes[i] = c;
            }
            else
            {
                //seconf half of the parent 2 to the offspring
                int c = d2.genes[i];
                genes[i] = c;
            }
        }
    }

    //make a mutation, so  this func create a mutation in a random position and insert a random mutation
    public void Mutate()
    {
        genes[Random.Range(0, dnaLenght)] = Random.Range(0, maxValues);
    }

    public int GetGene(int pos)
    {
        return genes[pos];
    }

}
