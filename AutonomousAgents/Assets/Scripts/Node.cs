using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This class defines a Node
 * 
 */

public class Node 
{
    //General Node variables
    public bool isWalkable;
    public Vector3 worldPos; //Pos in world coordinates
    public int gridX; //Pos on X in grid coordinates
    public int gridY; //Pos on Y in grid coordinates
    public int movementCost; // Cost of movement on this Node


    // AStar-related Node variables
    public int gCost; // Distance from start node
    public int hCost; // Distance from end node
    public Node parent; //The nodes parent in the current determined path


    public Node(Vector3 _world, int _gridX, int _gridY, bool _walk, int _cost)
    {
        // Set position variables
        worldPos = _world;
        gridX = _gridX;
        gridY = _gridY;

        // Set AStar movement variables
        isWalkable = _walk;
        movementCost = _cost;
    }

    //Compute fCost as its needed, this will change dynamically depending on place in the path
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }


    //public int CompareTo(Node nodeToCompare)
    //{
    //    //Will return 1 if the integer is higher, so reverse the return value
    //    int compare = fCost.CompareTo(nodeToCompare.fCost);
    //    if (compare == 0)
    //    {
    //        compare = hCost.CompareTo(nodeToCompare.hCost);
    //    }
    //    return -compare;
    //}
}
