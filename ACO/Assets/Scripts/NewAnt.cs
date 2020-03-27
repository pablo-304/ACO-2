using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAnt
{
    public List<Voxel> lastSteps;
    public Voxel currentVoxel;
    public int weight=0;
    public GameObject antObject;
    public int antPheromone;
    public List<Pheromone> pheromoneList; //list of deposited pheromones
    public int goal;
    int antMemory;
    public bool food;
    public Vector3 currentDirection;
    //bool avoidOverlapping;


    public NewAnt(Voxel currentVoxel, GameObject antObject, int antPheromone, int goal, int antMemory)
    {
        lastSteps = new List<Voxel>();
        this.currentVoxel = currentVoxel;
        this.antObject = antObject;
        this.antPheromone = antPheromone;
        pheromoneList = new List<Pheromone>();
        this.goal = goal;
        this.antMemory = antMemory;
        food = false;
        currentDirection = direction();
       // avoidOverlapping = false;
    }

    public Pheromone PheromonePooper()
    {
        Pheromone pheromone = new Pheromone(antPheromone);
        return pheromone;
    }

    public Vector3 direction()
    {
        currentDirection = new Vector3();
        if (lastSteps.Count > 1)
        { currentDirection = (currentVoxel.position - lastSteps[lastSteps.Count-2].position).normalized; }
        else
        { currentDirection = new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1)); }
        return currentDirection;
    }    
   
    public double evaluateVoxelVector(Voxel a) //THIS IS WHAT I NEED TO SORT
    {
        double voxelScore = 0;
        Vector3 neighbourDirection = (a.position - currentVoxel.position).normalized;
        float angle = Vector3.Angle(currentDirection, neighbourDirection);
        
        voxelScore = a.weight / ( 1+50*angle);
        
        return voxelScore;
    }

    public Voxel nextVoxel()
    {
        int index = 0;        
        for (int i = 1; i < currentVoxel.neighbours.Length; i++)
        {
            if (evaluateVoxelVector(currentVoxel.neighbours[i]) > evaluateVoxelVector(currentVoxel.neighbours[index]))
            {
                index = i;
            }
        }
        //if (currentVoxel.neighbours[index].antHere == true)
        //{
        //    index = Random.Range(0, currentVoxel.neighbours.Length - 1);
        //}
        return currentVoxel.neighbours[index];
    }
    
    void sortNeighboursWeights()
    {
        foreach (Voxel v in currentVoxel.neighbours)
        { v.SortNeighbours(); }
    }

    public void Move()
    {
        //sortNeighboursWeights();
        currentVoxel.pheromones.Add(PheromonePooper());
        currentVoxel.antHere = false;
        currentVoxel = nextVoxel( /*goal*/);
        lastSteps.Add(currentVoxel);
        antObject.transform.position = currentVoxel.position;
        currentVoxel.antHere = true;
    }

    //Voxel nextVoxel(bool avoidOverlapping, int antMemory)
    //{
    //    Voxel voxel = currentVoxel;
    //    voxel.SortNeighbours();
    //    if (lastSteps.Count >= antMemory)
    //    { lastSteps.RemoveAt(0); }

    //    if (goal >= currentVoxel.neighbours.Length)// && currentVoxel.neighbours[currentVoxel.neighbours.Length - 1] == lastSteps[i])
    //    {
    //        voxel = currentVoxel.neighbours[Random.Range((int)((currentVoxel.neighbours.Length - 1) * 0.25), currentVoxel.neighbours.Length - 1)];
    //        if (lastSteps.Contains(voxel))
    //        { voxel = currentVoxel.neighbours[Random.Range(0, currentVoxel.neighbours.Length - 1)]; }
    //        break;
    //    }

    //    else if (goal < currentVoxel.neighbours.Length)//&& lastSteps[i] != currentVoxel.neighbours[goal])
    //    {
    //        voxel = currentVoxel.neighbours[goal];
    //        if (lastSteps.Contains(voxel))
    //        { voxel = currentVoxel.neighbours[Random.Range(0, currentVoxel.neighbours.Length - 1)]; }
    //        break;
    //    }
    //    if (voxel.antHere == true && avoidOverlapping)
    //    { voxel = currentVoxel.neighbours[Random.Range(0, currentVoxel.neighbours.Length - 1)]; }

    //    return voxel;
    //}

    public static IComparer Comparison()
    {
        return (IComparer)new ComparisonHelper();
    }
    public class ComparisonHelper: IComparer
    {
        public int Compare(object x, object y)
        {
            NewAnt a = (NewAnt)x;
            NewAnt b = (NewAnt)y;
            if (a.weight > b.weight)
            {
                return -1;
            }
            else if (a.weight < b.weight)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
