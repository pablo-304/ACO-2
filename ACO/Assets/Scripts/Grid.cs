using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(LineRenderer))]
public class Grid : MonoBehaviour
{
    public Voxel[,] _grid;
    public int counter;

    public static int gridX;
    public static int gridZ;
    public static float stepBetweenVoxels;
    public static bool releaseTheKraken;
    public static int colonyGoal;

    //pheromones and its creation, together with the ants     
    public static int pheromoneIntensity;

    public static bool Reset;

    //ants creation and movement    
    public static int populationNumber;
    public GameObject sphere;
    public static int antMemory;
    public bool avoidOverlapping;
    Voxel colonyOrigin;

    public int numberOfColonies;

    //mesh generator
    public GenerateMesh maping;
    Mesh movementMap;
    public Gradient gradient;
    LineRenderer edgeDrawer;

    public Voxel food;
    public Colony newColony;   

    //set the grid size and scale
    public void quitApp()
    {
        Debug.Log("BYE BYE");
        Application.Quit();
    }
    public void sizeTheGridX(float a)
    { gridX = (int)a; }
    public void sizeTheGridZ(float a)
    { gridZ = (int)a; }
    public void sizeTheGridScale(float a)
    { stepBetweenVoxels = a; }
    //set the grid
    public void setGrid()
    {
        _grid = ArrayOfMovement();
        ThisIsMyNeighborhood();
        movementMap = new Mesh();
        GetComponent<MeshFilter>().mesh = movementMap;
        maping = new GenerateMesh(movementMap, _grid, gradient);
        maping.createShape();
    }

    //set the colony fields
    public void setThePopulationNumber(float census)
    { populationNumber = (int)census; ; }
    public void setPheromoneIntensity(float intensity)
    { pheromoneIntensity = (int)intensity; }
    public void setVoxel()
    { colonyOrigin = colonyOrigin = _grid[Random.Range(5, gridX - 5), Random.Range(5, gridZ - 5)];
        colonyOrigin.voxelValue = pheromoneIntensity * 10; }
    public void setMemory(float memory)
    { antMemory = (int)memory; }
    public void setGoal(float goal)
    { colonyGoal = (int)goal; }

    public bool pheromoneViewer = false;
    public bool weightViewer = false;
    public bool pheromoneNumViewer = false;

    public void pheromoneNumberViewer()
    {
        maping.weightViewer = false;
        maping.pheromoneIntensityViewer = false;
        maping.pheromoneNumberViewer = true;
    }
    public void pheromoneIntViewer()
    {
        maping.pheromoneNumberViewer = false;
        maping.weightViewer = false;
        maping.pheromoneIntensityViewer = true;
    }
    public void weightIntViewer()
    {
        maping.pheromoneIntensityViewer = false;
        maping.pheromoneNumberViewer = false;
        maping.weightViewer = true;
    }
    public void terrainViewer()
    {
        maping.pheromoneIntensityViewer = false;
        maping.pheromoneNumberViewer = false;
        maping.weightViewer = false;
    }

    //set the colony
    public void setColony()
    {    newColony = new Colony(populationNumber, colonyOrigin, pheromoneIntensity, antMemory, sphere, colonyGoal, avoidOverlapping);    }

    public Colony[] createColony(int numberOfColonies)
    {
        Colony[] allTheColonies = new Colony[numberOfColonies];
        return allTheColonies;
    }

    public bool move = false;
    public void startTheAnts()
    { move = true;
        StartCoroutine(startTheMovement()); }
    public void stopTheAnts()
    { move = false; }

    //start the colony 
    IEnumerator startTheMovement()
    {
        while (move)
        {            
            if (newColony.counter < newColony.populationNumber)
            { newColony.explorer(); }

            if (newColony.counter < newColony.eliteNumber)
            { newColony.eliteAnt(); }
            newColony.MoveColony();
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    //_grid[i, j].UpdateVoxelValue();
                    _grid[i, j].updateWeight();
                }
            }
            maping.updateMesh();
            yield return null;
        }
    }   

    //reset
    public void reset()
    {
        releaseTheKraken = false;
        newColony.resetThePopulation();
    }   

    public Voxel[,] ArrayOfMovement()
    {
        var grid = new Voxel[gridX, gridZ];
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridZ; j++)
            {
                grid[i, j] = new Voxel(i, j, stepBetweenVoxels);
            }
        }
        return grid;
    }

    public void ThisIsMyNeighborhood()
    {
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                List<Voxel> neighbours = new List<Voxel>();
                for (int x = i - 2; x <= i + 2; x++)
                {
                    for (int y = j - 2; y <= j + 2; y++)
                    {
                        if (!(x == i && y == j))
                        {
                            if (x >= 0 && x < _grid.GetLength(0) && y >= 0 && y < _grid.GetLength(1))
                            {
                                neighbours.Add(_grid[x, y]);
                            }                           
                        }
                    }
                }
                if (i == _grid.GetLength(0) - 1)
                { neighbours.Add(_grid[0, j]); }
                if (i == 0)
                { neighbours.Add(_grid[_grid.GetLength(0) - 1, j]); }
                if (j == _grid.GetLength(1) - 1)
                { neighbours.Add(_grid[i, 0]); }
                if (j == 0)
                { neighbours.Add(_grid[i, _grid.GetLength(1) - 1]); }
                _grid[i, j].neighbours = neighbours.ToArray();                
            }
        }
    }   
}
    

