using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

/*
 *  This class creates a grid for the game environment
 * 
 */


public class Grid : MonoBehaviour{


    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }


    public bool displayGridGizmos;
   

    public LayerMask unwalkableMask; 
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public TerrainType[] walkableRegions;

    LayerMask walkableMask;

    // Takes an int ID for the region and returns an int for the movement cost
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>(); 


    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;



    #region BoardLocations

    List<Vector3> worldLocationPositions = new List<Vector3>();

    //public Count mountainCount = new Count(2, 4);
    //public Count treeCount = new Count(4, 6);

    public GameObject lake;
    public GameObject house;
    public GameObject goldmine;
    public GameObject bank;
    public GameObject jailhouse;
    public GameObject saloon;
    public GameObject cemetery;
    public GameObject outlawCamp;
    public GameObject prairie;
    public GameObject workshop;

    public Vector3 lakePos;
    public Vector3 housePos;
    public Vector3 goldminePos;
    public Vector3 bankPos;
    public Vector3 jailhousePos;
    public Vector3 cemeteryPos;
    public Vector3 saloonPos;
    public Vector3 outlawCampPos;
    public Vector3 prairiePos;
    public Vector3 workshopPos;

    public List<Vector3> locationPositions;

    public GameObject[] mountains;
    public GameObject[] trees;

    #endregion




    void Awake()
    {
        locationPositions = new List<Vector3>();

        nodeDiameter = nodeRadius * 2;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);


        //Instantiate locations etc
        SpawnLocations();



        foreach (TerrainType region in walkableRegions)
        {
            //Bitwise or gets the next element in unity's layer system, 
            walkableMask.value |= region.terrainMask.value;
            //This computes the layer cost as from the ie layer 9 would have 2^12 @TODO
            // IE ask to what power should 2 be raised in order to equal the terrainMask value
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value,2), region.terrainPenalty);
        }

        CreateGrid();
    }


    void createWorldPositions()
    {
        worldLocationPositions.Clear();

        int xStart = -(int)(gridWorldSize[0] / 2) + 5;
        int zStart = -(int)(gridWorldSize[1] / 2) + 5;

        int xRange = (int)Mathf.Abs(gridWorldSize[0]/2 - 5);
        int zRange = (int)Mathf.Abs(gridWorldSize[1]/2 - 5);

        for (int x = xStart; x < xRange; x += 10)
            for (int z = zStart; z < zRange; z += 10)
            {
                worldLocationPositions.Add(new Vector3(x, 0.0f, z));
            }
    }


    //Gives an object a random position on the grid
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, worldLocationPositions.Count);
        Vector3 randomPosition = worldLocationPositions[randomIndex];
        worldLocationPositions.RemoveAt(randomIndex);
        return randomPosition;
    }


    public void SpawnLocations()
    {
        createWorldPositions();

        foreach (GameObject i in trees)
        {
            Vector3 randomPos = RandomPosition();
            Instantiate(i, randomPos, Quaternion.identity);
        }

        foreach (GameObject i in mountains)
        {
            Vector3 randomPos = RandomPosition();
            Instantiate(i, randomPos, Quaternion.Euler(270,0,0));
        }

        housePos = RandomPosition();
        goldminePos = RandomPosition();
        bankPos = RandomPosition();
        jailhousePos = RandomPosition();
        cemeteryPos = RandomPosition();
        saloonPos = RandomPosition();
        outlawCampPos = RandomPosition();
        prairiePos = RandomPosition();
        workshopPos = RandomPosition();

        lakePos = RandomPosition();



        //Add the locations to a List for the sheriff, doesn't need the outlawcamp, the jailhouse or the secret workshop
        locationPositions.Add(housePos);
        locationPositions.Add(goldminePos);
        //locationPositions.Add(jailhousePos);
        locationPositions.Add(cemeteryPos);
        locationPositions.Add(saloonPos);
        //locationPositions.Add(outlawCampPos); 
        locationPositions.Add(prairiePos);


        Instantiate(house, housePos, Quaternion.identity);
        Instantiate(goldmine, goldminePos, Quaternion.identity);
        Instantiate(bank, bankPos, Quaternion.identity);
        Instantiate(jailhouse, jailhousePos, Quaternion.identity);
        Instantiate(saloon, saloonPos, Quaternion.identity);
        Instantiate(cemetery, cemeteryPos, Quaternion.identity);
        Instantiate(outlawCamp, outlawCampPos, Quaternion.Euler(45, 0, 0));
        Instantiate(prairie, prairiePos, Quaternion.identity);
        Instantiate(lake, lakePos, Quaternion.identity);
        Instantiate(workshop, workshopPos, Quaternion.identity);

    }


    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                //Set the bool based on the layermask
                bool isWalkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                int movementCost = 0;

                //raycast code
                if(isWalkable)
                {
                    Ray ray = new Ray(worldPoint + Vector3.up * 45, Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, walkableMask))
                    {
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementCost);
                    }
                }

                grid[x, y] = new Node( worldPoint, x, y, isWalkable, movementCost);
            }
        }
    }


    //Gets the node from a given world position 
    public Node GetNodeFromWorldPos(Vector3 worldPosition)
    {
        //float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f - (nodeRadius / gridWorldSize.x);
        //float percentY = (worldPosition.z - transform.position.z) / gridWorldSize.y + 0.5f - (nodeRadius / gridWorldSize.y);

        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.x / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }




    // Returns a list of a node's neighbour nodes 
    public List<Node> GetNodeNeighbours(Node node)
    {
        List<Node> nodeNeighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    nodeNeighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return nodeNeighbours;
    }




    //Returns the maximum size of the grid
    public int MaxGridSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }



    // Visualises the nodes
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null && displayGridGizmos)
            {
                foreach (Node n in grid)
                {
                    //if walkable, draw white, if not draw red
                    Gizmos.color = (n.isWalkable) ? Color.white : Color.red;

                    Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
    }


    






}



//Want it to show up in the editor
[System.Serializable]
public class TerrainType
{
    public LayerMask terrainMask;
    public int terrainPenalty;
}