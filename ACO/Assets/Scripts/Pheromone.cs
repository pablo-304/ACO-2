using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pheromone
{
    public int pheromoneValue;    

    public Pheromone(int pheromoneValue)
    {
        this.pheromoneValue = pheromoneValue;
    }
    public int GetValue()
    {
        int result = pheromoneValue;
        if (pheromoneValue > 0)
        {
            pheromoneValue--;
        }
        else if(pheromoneValue<0)
        {
            pheromoneValue++;
        }
        return result;
    }
}

