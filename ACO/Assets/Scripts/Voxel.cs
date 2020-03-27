using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel
{
    public Vector3 position;
    public Voxel[] neighbours;
    public int voxelValue;
    public List<Pheromone> pheromones;    
    public bool antHere = false;
    float distanceBetweenVoxels;
    public int currentValue;
    public double weight;
    public bool walkable;
    
    
    public Voxel(int x, int y, float distanceBetweenVoxels)
    {
        this.distanceBetweenVoxels = distanceBetweenVoxels;
        voxelValue = Random.Range(1, 10);
        position = new Vector3(x * distanceBetweenVoxels, 0, y * distanceBetweenVoxels);
        pheromones = new List<Pheromone>();
        currentValue = voxelValue;
        weight = voxelValue;
        walkable = true;
    }

    public List<Voxel> nextNeighbours()
    {
        List<Voxel> freeNeighbours = new List<Voxel>();
        walkable = true;
        for (int i = 0; i < neighbours.Length; i++)
        {
            if(neighbours[i]== null)
            { walkable = true;
                freeNeighbours.Add(neighbours[i]);
                break;
            }
            else
            { walkable = false; }
        }
        return freeNeighbours;
    }

    public void updateWeight()
    {
        UpdateVoxelValue();
        weight =   Mathf.Pow(1 +( currentValue / (1 + Mathf.Abs(currentValue * pheromoneCounter()))), pheromoneGradientCoef());        
    }

    public void SortNeighbours()
    {
        System.Array.Sort(neighbours, Comparison());
    }

    public float pheromoneGradientCoef()
    {
        float osmotropotaxis = 0;
        for (int i = 0; i < neighbours.Length - 1; i++)
        {           
           osmotropotaxis += voxelValue + neighbours[i].currentValue;                       
        }
        osmotropotaxis /= (neighbours.Length*3);
        return osmotropotaxis;
    }

    public int pheromoneCounter()
    {
        int numberOfPheromones = 0;        
        numberOfPheromones += pheromones.Count;
        numberOfPheromones *=2;
        return numberOfPheromones;
    }

    public void UpdateVoxelValue()
    {
        int i = 0;
        currentValue = voxelValue;
        while (i<pheromones.Count)
        {
            int value = pheromones[i].GetValue();
            if (value == 0)
            {
                pheromones.RemoveAt(i);
            }
            if(value>0)
            {                
                 currentValue += value;
                i++;                
            }
            else if (value < 0)
            {
                currentValue += value;
                i++;
            }
        }
        //Vector3 newPos = new Vector3(position.x, currentValue * 0.1f/*(float)weight * 0.1f*/, position.z);
        Vector3 newPos = new Vector3(position.x, position.y, position.z);
        position = newPos;
    }

    public static IComparer Comparison()
    {
        return (IComparer)new ComparisonHelper();
    }

    public class ComparisonHelper : IComparer
    {
        public int Compare(object x, object y)
        {
            Voxel a = (Voxel)x;
            Voxel b = (Voxel)y;
            if (a.weight > b.weight)
            {
                return 1;
            }
            else if (a.weight < b.weight)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public double sensoryCapacity()
    {
        double averagePheromoneIntensity = 0;
        double totalPheromoneIntensity = 0;

        for (int i = 0; i < neighbours.Length; i++)
        {
            totalPheromoneIntensity += neighbours[i].currentValue;
        }
        averagePheromoneIntensity = totalPheromoneIntensity / neighbours.Length;

        return totalPheromoneIntensity;
    }

    public List<int> getIndex(Voxel[,] grid, Voxel v)
    {
        int a;
        int b;
        List<int> indexPair = new List<int>();
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] == v)
                {
                    a = i;
                    b = j;
                    indexPair.Add(a);
                    indexPair.Add(b);
                    break;
                }
            }
        }
        return indexPair;
    }
}
