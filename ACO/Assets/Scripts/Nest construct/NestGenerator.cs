using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestGenerator
{
    Voxel pellet;
    public bool carryYesNo;
    public Voxel[,,] grid;
    public Colony colony;
    public Edge tunel;
    public Node camera;
    public int NodeNumber;
    public int nestDensity;

    public NestGenerator(Voxel [,,] grid, Colony colony, int nestDensity)
    {
        this.grid = grid;
        this.colony = colony;
        this.nestDensity = nestDensity;
        NodeNumber = colony.colony.Count / nestDensity;
    }

    public void addPellet()
    {        
        for (int i = 0; i < colony.colony.Count; i++)
        {
            if (colony.colony[i].currentVoxel.currentValue > colony.pheromoneValue * 5)
            {
                pellet = findLowestVoxel(colony.colony[i].currentVoxel.nextNeighbours());
                break;
            }
        }    
    }

    public void removePellet()
    {
        for (int i = 0; i < colony.colony.Count; i++)
        {
            if (colony.colony[i].currentVoxel.currentValue == colony.colony[i].currentVoxel.voxelValue && colony.colony[i].currentVoxel.position.y != 0)
            {
                
                break;
            }
        }
    }

    public Voxel findLowestVoxel(List<Voxel> listOfValues)//detects the lowest value
    {
        int index = 0;
        for (int i = 1; i < listOfValues.Count; i++)
        {
            if (listOfValues[i].position.y < listOfValues[index].position.y)
            {
                index = i;
            }
        }
        return listOfValues[index];
    }
}

