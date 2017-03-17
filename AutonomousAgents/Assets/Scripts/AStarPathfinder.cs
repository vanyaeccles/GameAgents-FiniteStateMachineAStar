using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

/*
 *  This class defines the AStar pathfinder
 * 
 */

public class AStarPathfinder : MonoBehaviour {

    
    PathRequestManager requestManager;
    Grid grid;


    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        // initiates AStar to begin the path search
        StartCoroutine(FindAstarPath(startPos, targetPos));
    }



    // AStar pathfinding Code
    // Receives start and target in world coordinates
    // Returns the path as a Vector3 array
    IEnumerator FindAstarPath(Vector3 startPos, Vector3 targetPos)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();

        Vector3[] pathWaypoints = new Vector3[0];

        bool isPathFound = false;


        Node startNode = grid.GetNodeFromWorldPos(startPos);
        Node targetNode = grid.GetNodeFromWorldPos(targetPos);

        
        //Check that the path is walkable
        if (startNode.isWalkable && targetNode.isWalkable)
        {

            //List to hold the OPEN node set
            List<Node> openSet = new List<Node>();
            //HashSet to hold the CLOSED node set
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);


            //Begin the loop
            while(openSet.Count > 0)
            {


                //Get node in the OPEN  set with lowest f-cost, set it to current node
                Node currentN = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentN.fCost || openSet[i].fCost == currentN.fCost)
                    {
                        if (openSet[i].hCost < currentN.hCost)
                            currentN = openSet[i];
                    }
                }

                openSet.Remove(currentN);
                closedSet.Add(currentN);


                // Check if a complete path has been found
                if (currentN == targetNode)
                {   //Path has been found! Retrace this path 
                    //sw.Stop();
                    //print("Path Found: " + sw.ElapsedMilliseconds + " ms");

                    isPathFound = true;
                    break;
                }




                foreach (Node neighbour in grid.GetNodeNeighbours(currentN))
                {
                    //If neighbour is not traversable or in the CLOSED set, skip to next neighbour
                    if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                
                    int newCostToNeighbour = currentN.gCost + GetNode2NodeDistance(currentN, neighbour) + neighbour.movementCost;
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        // Set the g+h cost and parent of the neighbour node
                        neighbour.gCost = newCostToNeighbour; 
                        neighbour.hCost = GetNode2NodeDistance(neighbour, targetNode);
                        neighbour.parent = currentN;

                        //add it to open list (if not already there)
                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        //else //if its already in the openset its value has changed
                            //openSet.UpdateItem(neighbour);
                    }
            }
        }
        }
        //wait for a frame before returning
        yield return null;

        if (isPathFound)
        {
            //Retrace the path and set it as a Vector3 of waypoints
            pathWaypoints = RetracePath(startNode, targetNode);
        }

        requestManager.CurrentPathFound(pathWaypoints, isPathFound);
    }




    //Retraces the path from the endNode to the startNode, path is stored in grid
    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        //Work way back to start node based on parent nodes
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        //Reverse it to get the path
        Array.Reverse(waypoints);

        //grid.path = path;
        return waypoints;
    }


    
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
                directionOld = directionNew;
            }
        }
        return waypoints.ToArray();
    }



    //Gets the distance between Nodes
    int GetNode2NodeDistance(Node nodeA, Node nodeB)
    {
        int xDistance = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int yDistance = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        // @TODO
        if (xDistance > yDistance)
        {
            return 14 * yDistance + 10 * (xDistance - yDistance);
        }
            
        return 14 * xDistance + 10 * (yDistance - xDistance);
    }

}
