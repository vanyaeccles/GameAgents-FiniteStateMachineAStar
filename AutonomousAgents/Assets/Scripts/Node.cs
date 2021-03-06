﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This class defines a Node on the AStar grid
 */

public class Node 
{
    //General Node variables
    public bool isWalkable; // Whether its walkable or not
    public Vector3 worldPos; //Pos in world coordinates
    public int gridX; //Pos on X in grid coordinates
    public int gridY; //Pos on Y in grid coordinates
    public int movementCost; // Cost of movement on this Node
    public int soundPropogationCost; // Cost of sound propogation


    // AStar-related Node variables
    public int gCost; // Distance from start node
    public int hCost; // Distance from end node
    public Node parent; //The nodes parent in the current AStar path


    public Node(Vector3 _world, int _gridX, int _gridY, bool _walk, int _cost, int _soundCost)
    {
        // Set position variables
        worldPos = _world;
        gridX = _gridX;
        gridY = _gridY;

        // Set AStar movement variables
        isWalkable = _walk;
        movementCost = _cost;
        soundPropogationCost = _cost;
    }

    //Compute fCost as its needed, this will change dynamically depending on place in the path
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

}
