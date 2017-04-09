using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

/*
 *  This class defines the AStar pathfinder
 */

public class AStarPathfinder : MonoBehaviour {

    
    PathRequestManager requestManager; // This handles getting and receiving paths from the AStar algorithm
    public Grid asgrid; // A copy of the grid


    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        asgrid = GetComponent<Grid>();
    }

    public void Start()
    {
        //UnityEngine.Debug.Log(grid);
    }


    public void StartFindPath(Vector3 startPos, Vector3 targetPos, bool soundPath)
    {
        // initiates AStar to begin the path search
        StartCoroutine(FindAstarPath(startPos, targetPos, soundPath));
    }



    // AStar pathfinding Code
    // Receives start and target in world coordinates
    // Returns the path as a Vector3 array
    IEnumerator FindAstarPath(Vector3 startPos, Vector3 targetPos, bool isSoundPath)
    {
        Vector3[] pathWaypoints = new Vector3[0]; // The returned path
        bool isPathFound = false;
        

        Node startNode = asgrid.GetNodeFromWorldPos(startPos);   // The node we start with (converted from world coords)
        Node targetNode = asgrid.GetNodeFromWorldPos(targetPos); // Node we want to get to (converted from world coords)      


        //UnityEngine.Debug.Log(startNode.gridX + " " + startNode.gridY);
        //UnityEngine.Debug.Log(targetNode.gridX + " " + targetNode.gridY);



        //Check that the start and target are both walkable
        if (startNode.isWalkable && targetNode.isWalkable)
        {

            //List to hold the OPEN node set
            List<Node> openSet = new List<Node>();

            //HashSet to hold the CLOSED node set
            HashSet<Node> closedSet = new HashSet<Node>();

            //Begin with the startNode
            openSet.Add(startNode);


            //Begin the loop to find the path
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


                // Go through the neighbours and 
                foreach (Node neighbour in asgrid.GetNodeNeighbours(currentN))
                {
                    //If neighbour is not traversable or in the CLOSED set, skip to next neighbour
                    if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }


                    int newCostToNeighbour;

                    // Are we creating an auditory path?
                    if(isSoundPath)
                    {
                        newCostToNeighbour = currentN.gCost + GetNode2NodeDistance(currentN, neighbour) + neighbour.soundPropogationCost; 
                    }

                    else
                        newCostToNeighbour = currentN.gCost + GetNode2NodeDistance(currentN, neighbour) + neighbour.movementCost;



                    // if the neighbour has a lesser cost and isn't already in the open set, set the cost and parent and add to open set
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        // Set the g+h cost and parent of the neighbour node
                        neighbour.gCost = newCostToNeighbour; 
                        neighbour.hCost = GetNode2NodeDistance(neighbour, targetNode);
                        neighbour.parent = currentN;

                        //add it to open list (if not already there)
                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);

                    }
                }


            }//End of loop, path is found
        }

        //wait for a frame before returning
        yield return null;

        if (isPathFound)
        {
            //Retrace the path and set it as a Vector3 of waypoints
            pathWaypoints = RetracePath(startNode, targetNode, isSoundPath);
        }


        // Gives the path back to the request manager, will return an empty path if no path was found
        requestManager.CurrentPathFound(pathWaypoints, isPathFound, isSoundPath);
    }




    //Retraces the path from the endNode to the startNode, path is stored in grid
    Vector3[] RetracePath(Node startNode, Node endNode, bool isSoundPath)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        //Work way back to start node based on parent nodes
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints;

        if (!isSoundPath)
            waypoints = SimplifyPath(path);
        else
            waypoints = ReturnPathOfNodes(path);

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

    Vector3[] ReturnPathOfNodes(List<Node> path)
    {
        List<Vector3> pathpoints = new List<Vector3>();

        for (int i = 1; i < path.Count; i++)
        {
             pathpoints.Add(path[i].worldPos);
        }

        return pathpoints.ToArray();
    }



    //Gets the 'fancy diagnonal' distance between Nodes, returned distance is scaled up by 10 (sqrt(2) is 1.4, hence diagonal is 14 per unit distance)
    int GetNode2NodeDistance(Node nodeA, Node nodeB)
    {
        int xDistance = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int yDistance = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        int distance;

        if (xDistance > yDistance)
        {
            distance = 14 * yDistance + 10 * (xDistance - yDistance); 
        }
        else
        {
            distance = 14 * xDistance + 10 * (yDistance - xDistance);
        }

        return distance; 
    }

}
