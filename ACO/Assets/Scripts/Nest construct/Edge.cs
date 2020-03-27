using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge 
{
    public double edgeLength;
    double currentLength;
    public Node startNode;
    public Node endNode;

    public Edge(Vector3 tunel)
    {
        edgeLength = Random.Range(0, 5);
        currentLength = edgeLength;
    }

   
}
