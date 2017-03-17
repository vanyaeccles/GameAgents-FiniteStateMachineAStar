using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Elsa : MonoBehaviour
{

    private StateMachine<Elsa> stateMachine;
    public Grid elsaGrid;



    public Locations Location = Locations.house;
    public int GoldCarried = 0;
    public int MoneyInBank = 0;
    public int Thirst = 0;
    public int Fatigue = 0;
    public int bullets = 0;

    public static int WAIT_TIME = 5;
    public int waitedTime = 0;
    public int createdTime = 0;

    // AStar details
    //private AStarPathfinder bobLocManager;
    private Transform destination; // The position of the destination target
    float speed = 10; // Speed of the agent's movement
    Vector3[] path; // A vector3 array containing the nodes in the path
    int targetIndex;



    public void Awake()
    {
        Debug.Log("Elsa is waking up...");
        this.stateMachine = new StateMachine<Elsa>();


        elsaGrid = GameObject.Find("GameManager").GetComponent<Grid>();

        transform.position = elsaGrid.housePos;
    }

    public void Start()
    {
        Go(elsaGrid.housePos);
        this.stateMachine.Init(this, MakingSomeDinner.Instance, ElsaGlobalState.Instance);
    }

    public bool Thirsty()
    {
        bool thirsty = Thirst == 10 ? true : false;
        return thirsty;
    }

    public bool OutOfAmmo()
    {
        bool noAmmo = bullets == 0 ? true : false;
        return noAmmo;
    }

    public void ShootBullet()
    {
        bullets--;
    }

    public void GetMoreAmmo()
    {
        bullets = 6;
    }

    public void ChangeLocation(Locations l)
    {
        Location = l;
    }

    public void ChangeState(State<Elsa> state)
    {
        this.stateMachine.ChangeState(state);
    }

    public void RevertToPreviousState()
    {
        this.stateMachine.RevertToPreviousState();
    }

    public void Update()
    {
        Thirst++;
        this.stateMachine.Update();
    }


    #region MOVEMENT+PATHFINDING

    public void Go(Vector3 _destination)
    {
        PathRequestManager.RequestPath(transform.position, _destination, OnPathFound);
    }


    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

            //Debug.Log("Following path");
        }
    }


    //Draws a gizmo cube for each waypoint on the path, with lines connecting them, blue for bob
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    //Line from waypoint to waypoint
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    
    public void ElsaAtHouse()
    {
        ChangeLocation(Locations.house);
    }
    public void ElsaAtPrairie()
    {
        ChangeLocation(Locations.prairie);
    }

    #endregion
}