﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Bob : MonoBehaviour {

	private StateMachine<Bob> stateMachine;
    

    public Locations Location = Locations.goldmine;
    public Grid bobGrid;
    

    //public Vector3 bobPos;
    public Transform bobTransform;



    public int GoldCarried = 0;
    public int MoneyInBank = 0;
    public int Thirst = 0;
    public int Fatigue = 0;

    public static int WAIT_TIME = 5;
    public int waitedTime = 0;
    public int createdTime = 0;


    // AStar details
    //private AStarPathfinder bobLocManager;
    private Transform destination; // The position of the destination target
    float speed = 10; // Speed of the agent's movement
    Vector3[] path; // A vector3 array containing the nodes in the path
    int targetIndex;


    // public Vector3 bobPos;

    public void Awake () {
        Debug.Log("Bob is waking up...");
		this.stateMachine = new StateMachine<Bob>();
		
      
        Outlaw.OnBankRobbery += handlerBankRobbery;

        bobGrid = GameObject.Find("GameManager").GetComponent<Grid>();

        transform.position = bobGrid.housePos;
    }

    public void Start()
    {
        Go(bobGrid.goldminePos);
        this.stateMachine.Init(this, EnterMineAndDigForNuggets.Instance, BobGlobalState.Instance);
    }



    public void ChangeLocation(Locations l)
    {
        Location = l;
    }

    public void AddToGoldCarried(int amount)
    {
        GoldCarried += amount;
    }

    public bool RichEnough()
    {
        return false;
    }

    public void AddToMoneyInBank(int amount)
    {
        MoneyInBank += amount;
        GoldCarried = 0;
    }

    public bool PocketsFull()
    {
        bool full = GoldCarried == 20 ? true : false;
        return full;
    }

    public bool Thirsty()
    {
        bool thirsty = Thirst == 10 ? true : false;
        return thirsty;
    }

    public void IncreaseFatigue()
    {
        Fatigue++;
    }

    public void IncreaseWaitedTime(int amount)
    {
        this.waitedTime += amount;
    }

    public bool WaitedLongEnough()
    {
        return this.waitedTime >= WAIT_TIME;
    }

    public void CreateTime()
    {
        this.createdTime++;
        this.waitedTime = 0;
    }

    public void ChangeState (State<Bob> state) {
		this.stateMachine.ChangeState(state);
	}

    public void RevertToPreviousState()
    {
        this.stateMachine.RevertToPreviousState();
    }

    public void handlerBankRobbery()
    {
        MoneyInBank -= 20;
        Debug.Log("Bob: My money's been stolen!");
    }

	public void Update () {
        Thirst++;
		this.stateMachine.Update();
        

        //updateSpriteLocation();

        //this.transform.position = bobPos;
        //bobTransform.position = bobPos;
    }




    #region MOVEMENT+PATHFINDING

    public void Go(Vector3 _destination)
    {
        //Debug.Log("Bob is Going");
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


    //Obselete
    //public void updateSpriteLocation()
    //{
    //    if(Location == Locations.home)
    //    {
    //        bobPos = bobGrid.housePos;
    //    }

    //    else if (Location == Locations.goldmine)
    //    {
    //        bobPos = bobGrid.goldminePos;
    //    }

    //    else if (Location == Locations.bank)
    //    {
    //        bobPos = bobGrid.bankPos;
    //    }

    //    else if (Location == Locations.saloon)
    //    {
    //        bobPos = bobGrid.saloonPos;
    //    }

    //}


    public void BobAtGoldmine()
    {
        //Debug.Log("Colliding");
        //if (transform.position != bobGrid.goldminePos)
        //{
            ChangeLocation(Locations.goldmine);
        //}
            
    }

    public void BobAtHouse()
    {
        //if (transform.position != bobGrid.housePos)
        //{
            ChangeLocation(Locations.house);
        //}
    }

    public void BobAtBank()
    {
        //if (transform.position != bobGrid.bankPos)
        //{
            ChangeLocation(Locations.bank);
        //}
    }


    #endregion

}