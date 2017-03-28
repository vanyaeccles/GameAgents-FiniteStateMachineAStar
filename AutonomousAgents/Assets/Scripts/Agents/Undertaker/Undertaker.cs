using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  This class defines the agent Undertaker
 */

public class Undertaker : MonoBehaviour
{
    private StateMachine<Undertaker> stateMachine;

    // AStar details
    private Transform destination; // The position of the destination target
    const float speed = 10; // Speed of the agent's movement
    Vector3[] path; // A vector3 array containing the nodes in the path
    int targetIndex;

    public Locations Location = Locations.robotWorkshop;
    public Grid undertakerGrid;


    public int Thirst = 0;
    public Transform brokenRobotPos;
    public bool isWithRobot = false;

    


    #region STATE MACHINE + BASIC AGENT METHODS

    public void Awake()
    {
        Debug.Log("The scientist is waking up...");
        this.stateMachine = new StateMachine<Undertaker>();

        Outlaw.OnJesseDead += handlerJesseBroken;

        undertakerGrid = GameObject.Find("GameManager").GetComponent<Grid>();

        transform.position = undertakerGrid.housePos;
    }

    public void Start()
    {
        Go(undertakerGrid.workshopPos);
        this.stateMachine.Init(this, TinkerWithRobots.Instance, UndertakerGlobalState.Instance);
    }

    public void ChangeLocation(Locations l)
    {
        Location = l;
    }

    public void ChangeState(State<Undertaker> state)
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

    #endregion


    #region STATE CHECKS

    public bool Thirsty()
    {
        bool thirsty = Thirst == 40 ? true : false;
        return thirsty;
    }

    #endregion




    public void handlerJesseBroken()
    {
        Debug.Log("Undertaker: Ah the t1000 is in need of repair");

        brokenRobotPos = GameObject.Find("Jesse").GetComponent<Transform>();
        Go(brokenRobotPos.position);
        ChangeState(FixRobot.Instance);
    }

    public void DragRobot()
    {
        GameObject.Find("Jesse").SendMessage("Drag");
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


    //Draws a gizmo cube for each waypoint on the path, with lines connecting them, black for the undertaker
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
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


    public void UndertakerAtLab()
    {
        ChangeLocation(Locations.robotWorkshop);
    }

    public bool isAtBrokenRobot()
    {
        return isWithRobot;
    }

    public void UndertakerAtRobot()
    {
        Debug.Log("Arrived at robot");
        isWithRobot = true;
    }

    public void FinishedWithRobot()
    {
        isWithRobot = false;
    }


    #endregion
}
