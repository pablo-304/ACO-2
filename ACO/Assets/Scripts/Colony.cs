using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colony 
{
    public Voxel colonyPosition;    
    public int populationNumber;
    public int eliteNumber;
    public int pheromoneValue;
    public int memoryLenght;
    public List<NewAnt> colony;
    public GameObject sphere;
    public int goal;
    public bool activateTheColony;
    public int counter;    
    public bool reset;   

    public Colony(int populationNumber,  Voxel colonyPosition, int pheromoneValue, int memoryLenght,  GameObject sphere, int goal, bool avoidOverlapping)
    {        
        this.populationNumber = populationNumber;
        eliteNumber = populationNumber / (UnityEngine.Random.Range(5, 10));
        this.colonyPosition = colonyPosition;
        this.pheromoneValue = pheromoneValue;
        this.memoryLenght = memoryLenght;        
        this.sphere = sphere;
        colony = new List<NewAnt>();
        this.goal = goal;               
        counter = 0;
    }
     
    public NewAnt explorer()//starts the ants
    {
        sphere.transform.localScale = new Vector3(1, 1, 1);
        NewAnt newAnt = new NewAnt(colonyPosition, UnityEngine.Object.Instantiate(sphere), pheromoneValue, goal, memoryLenght);
        newAnt.currentVoxel.antHere = true;
        colony.Add(newAnt);
        counter++;
        return newAnt; 
    }

    public NewAnt eliteAnt()//starts the ants
    {
        sphere.transform.localScale = new Vector3(2, 2, 2);        
        NewAnt newEliteAnt = new NewAnt(colonyPosition, UnityEngine.Object.Instantiate(sphere), pheromoneValue*(int)1.5, goal, memoryLenght*2);
        newEliteAnt.currentVoxel.antHere = true;
        colony.Add(newEliteAnt);
        counter++;
        return newEliteAnt;
    }

    public void MoveColony()
    {
        foreach (NewAnt ant in colony)
        {
            ant.direction();
            ant.Move();
        }
    }

    public void resetThePopulation()
    {        
        foreach (NewAnt a in colony)
        {
            activateTheColony = false;
            UnityEngine.Object.Destroy(a.antObject);            
        }
        
        colony.Clear();
        counter = 0;
    }

    void targetSearch(Voxel food)
    {
        for (int i = 0; i < colony.Count; i++)
        {
            if (colony[i].currentVoxel == food)
            {
                colony[i].food = true;
                colony[i].lastSteps.Clear();
            }
            if (colony[i].food == true)
            {
                colony[i].antPheromone = pheromoneValue;
                colony[i].goal = goal;
                Debug.Log(colony[i].goal + " is magenta colony's goal");
            }
            if (colony[i].currentVoxel == colonyPosition)
            {
                colony[i].food = false;
                colony[i].lastSteps.Clear();
            }
            else if (colony[i].food == false)
            {
                colony[i].antPheromone = pheromoneValue;
                colony[i].goal = goal;
                Debug.Log(colony[i].goal + " is yellow colony's goal");
            }
        }
    }

    public static IComparer Comparison()
    {
        return (IComparer)new ComparisonHelper();
    }

    public class ComparisonHelper : IComparer
    {
        public int Compare(object x, object y)
        {
            Colony a = (Colony)x;
            Colony b = (Colony)y;
            if (a.populationNumber > b.populationNumber)
            {
                return 1;
            }
            else if (a.populationNumber < b.populationNumber)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
