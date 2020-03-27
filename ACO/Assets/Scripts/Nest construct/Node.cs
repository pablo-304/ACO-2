using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Voxel locationVoxel;
    public List<Edge> nextEdgeList;
    public GameObject nodeObject;
    public int nodeValue;

    public Node(Voxel locationVoxel)
    {
        this.locationVoxel = locationVoxel;
        nodeValue = locationVoxel.voxelValue;
        nextEdgeList = new List<Edge>();
    }

   

    public static IComparer Comparison()
    {
        return (IComparer)new ComparisonHelper();
    }

    public class ComparisonHelper : IComparer
    {
        public int Compare(object x, object y)
        {
            Node a = (Node)x;
            Node b = (Node)y;
            if (a.nodeValue > b.nodeValue)
            {
                return 1;
            }
            else if (a.nodeValue < b.nodeValue)
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
