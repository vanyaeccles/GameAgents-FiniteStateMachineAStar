using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Sheriff : MonoBehaviour
{

    private StateMachine<Sheriff> stateMachine;
    public Grid sheriffGrid;

    static Random rnd = new Random();

    public Locations Location = Locations.jailhouse;
    public int GoldCarried = 10;
    public int MoneyInBank = 0;
    public int Thirst = 0;
    public int Fatigue = 0;
    public int bullets = 0;
    public int restedness = 0;
    private int sleepThreshold;


    public static int WAIT_TIME = 5;
    public int waitedTime = 0;
    public int createdTime = 0;
    public bool isAtALocation;

    // AStar details
    private Transform destination; // The position of the destination target
    float speed = 10; // Speed of the agent's movement
    Vector3[] path; // A vector3 array containing the nodes in the path
    int targetIndex;



    public void Awake()
    {
        Debug.Log("Sheriff is waking up...");
        this.stateMachine = new StateMachine<Sheriff>();

        sleepThreshold = Random.Range(40, 150);

        sheriffGrid = GameObject.Find("GameManager").GetComponent<Grid>();

        transform.position = sheriffGrid.jailhousePos;
    }

    public void Start()
    {

        Go(sheriffGrid.saloonPos);
        this.stateMachine.Init(this, DrinkAtTheSaloon.Instance, SheriffGlobalState.Instance);
    }

    public bool Thirsty()
    {
        bool thirsty = Thirst == 40 ? true : false;
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

    public void DrinkWhisky()
    {
        GoldCarried--;
    }

    public bool OutofMoney()
    {
        if (GoldCarried == 0)
            return true;
        else
            return false;
    }

    public Vector3 ChooseRandomLocation()
    {
        //int r = rnd.Next(sheriffGrid.locationPositions.Count);

        //Debug.Log("Chosen");

        Vector3 randomLocPos = sheriffGrid.locationPositions[Random.Range(0, sheriffGrid.locationPositions.Count)];

        return randomLocPos;
    }

    public void ChangeState(State<Sheriff> state)
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
        isAtALocation = false;
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


    //Draws a gizmo cube for each waypoint on the path, with lines connecting them, white for the Sheriff
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.white;
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

    public bool InspectLocation()
    {
        //have a wide look around?

        // Nothing fishy here
        return false;
    }

    public void Snooze()
    {
        restedness++;
    }

    public bool SleptLongEnough()
    {
        if (restedness > sleepThreshold)
            return true;
        else return false;
    }

    public void SheriffAtJailHouse()
    {
        ChangeLocation(Locations.jailhouse);
        GoldCarried += Random.Range(40, 100);
        Debug.Log("Sheriff: Got me s'more cash from the safe");
    }

    public void SheriffAtSaloon()
    {
        ChangeLocation(Locations.saloon);
    }

    public void AtLocation()
    {
        isAtALocation = true;
    }

    #endregion
}