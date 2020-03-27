using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMesh
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Voxel[,] grid;
    Color[] colors;

    float minMeshHeight;
    float maxMeshHeight;
    int counter = 0;
    public bool pheromoneIntensityViewer;
    public bool weightViewer;
    public bool pheromoneNumberViewer;    

    Gradient gradient;

    public GenerateMesh(Mesh mesh, Voxel[,] grid, Gradient gradient)
    {
        this.mesh = mesh;
        this.grid = grid;
        this.gradient = gradient;
    }

    public void createShape()
    {
        int gridX = grid.GetLength(0);
        int gridZ = grid.GetLength(1);
        updateVertices(grid);
        
        triangles = new int[gridX * gridZ * 6];
        int vert = 0;
        int tris = 0;        

        for (int i = 0; i < gridX - 1; i++)
        {
            for (int j = 0; j < gridZ - 1; j++)
            {
                triangles[tris + 0] = i * gridZ + j;
                triangles[tris + 1] = i * gridZ + j + 1;
                triangles[tris + 2] = (i + 1) * gridZ + j;
                triangles[tris + 3] = i * gridZ + j + 1;
                triangles[tris + 4] = (i + 1) * gridZ + j + 1;
                triangles[tris + 5] = (i + 1) * gridZ + j;

                vert++;
                tris += 6;
            }
            vert++;
        }        
        updateMesh();
        updateColors();
    }

    void updateColors()
    {
        updateVertices(grid);
        colors = new Color[vertices.Length];
        for (int index = 0, i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                float height = Mathf.InverseLerp(minMeshHeight, maxMeshHeight, vertices[index].y);
                colors[index] = gradient.Evaluate(height);
                //vertices[index] = new Vector3(grid[i, j].position.x, grid[i, j].currentValue * 0.1f, grid[i, j].position.z);
                index++;
            }
        }
    }

    public void updateMesh()
    {
        mesh.Clear();
        updateVertices(grid);
        mesh.vertices = vertices;
        
        mesh.triangles = triangles;
        updateColors();
        mesh.colors = colors;        
        mesh.RecalculateNormals();
    }

    public void updateVertices(Voxel[,] grid)
    {
        counter = 0;
        int gridX = grid.GetLength(0);
        int gridZ = grid.GetLength(1);
        vertices = new Vector3[(gridX) * (gridZ)];
        for (int index = 0, i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                //if (pheromoneIntensityViewer)
                 vertices[index] = new Vector3(grid[i, j].position.x, grid[i, j].currentValue * 0.1f, grid[i, j].position.z); 
                //if (weightViewer)
                // vertices[index] = new Vector3(grid[i, j].position.x, (float)grid[i, j].weight * 0.001f, grid[i, j].position.z);
                //if (pheromoneNumberViewer)
                //{ vertices[index] = new Vector3(grid[i, j].position.x, (float)grid[i, j].pheromones.Count * 0.1f, grid[i, j].position.z); }
                //else
                //{ vertices[index] = grid[i, j].position; }
                index++;

                float y = grid[i, j].position.y;

                if (counter ==0)
                {
                    if (y >= maxMeshHeight)
                    { maxMeshHeight = y; }
                    if (y <= minMeshHeight)
                    { minMeshHeight = y; }                     
                }
               
                counter++;
            }
        }
        
    }    



       /////Auxiliar methods I didn't use in the end
    public double findMaxValue(List<double> listOfValues)//detects the highest value
    {
        int index = 0;
        for (int i = 1; i < listOfValues.Count; i++)
        {
            if (listOfValues[i] > listOfValues[index])
            {
                index = i;
            }
        }
        return listOfValues[index];
    }
    public double findMinValue(List<double> listOfValues)//detects the lowest value
    {
        int index = 0;
        for (int i = 1; i < listOfValues.Count; i++)
        {
            if (listOfValues[i] < listOfValues[index])
            {
                index = i;
            }
        }
        return listOfValues[index];
    }    
}
