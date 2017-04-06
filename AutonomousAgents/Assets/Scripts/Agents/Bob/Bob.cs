using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  This class defines the agent Bob
 */

public class Bob : MonoBehaviour {

	private StateMachine<Bob> stateMachine;
    private Soul bobSoul;

    // AStar details
    //private AStarPathfinder bobLocManager;
    private Transform destination; // The position of the destination target
    Vector3[] path; // A vector3 array containing the nodes in the path
    int targetIndex;

    public Locations Location = Locations.goldmine;
    public Grid bobGrid;



    //public Transform bobTransform;


    const float speed = 10; // Speed of the agent's movement
    public int GoldCarried = 0;
    public int MoneyInBank = 0;
    public int Thirst = 0;
    public int Fatigue = 0;
    public static int WAIT_TIME = 5;
    public int waitedTime = 0;
    public int createdTime = 0;
    public int hearingThreshold = 30;

    private TextMesh bobSpeech;


    #region STATE MACHINE + BASIC AGENT METHODS

    public void Awake () {
        //Debug.Log("Bob is waking up...");
		this.stateMachine = new StateMachine<Bob>();
		
      
        Outlaw.OnBankRobbery += handlerBankRobbery;

        if ((bobGrid == null) && (GameObject.Find("GameManager").GetComponent<Grid>() != null))
        {
            bobGrid = GameObject.Find("GameManager").GetComponent<Grid>();
        }
        else
        {
            Debug.LogWarning("Missing grid script component. Please add one");
        }

        bobSpeech = GameObject.Find("BobText").GetComponent<TextMesh>();

        transform.position = bobGrid.housePos;
    }

    public void Start()
    {
        Go(bobGrid.goldminePos);

        //ListenOut(bobGrid.goldminePos);

        this.stateMachine.Init(this, EnterMineAndDigForNuggets.Instance, BobGlobalState.Instance);
    }

    public void ChangeState(State<Bob> state)
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


        //updateSpriteLocation();

        //this.transform.position = bobPos;
        //bobTransform.position = bobPos;
    }

    public void ChangeLocation(Locations l)
    {
        Location = l;
    }


    public void Speak(string text)
    {
        bobSpeech.text = text;
    }

    #endregion



    #region STATE CHECKS

    public bool RichEnough()
    {
        return false;
    }

    public bool PocketsFull()
    {
        bool full = GoldCarried == 150 ? true : false;
        return full;
    }

    public bool Thirsty()
    {
        bool thirsty = Thirst == 10 ? true : false;
        return thirsty;
    }



    public bool WaitedLongEnough()
    {
        return this.waitedTime >= WAIT_TIME;
    }

    // Checks his hearing, if his pockets are full of gold all he can hear is how wealthy he is
    public int CheckHearing()
    {
        hearingThreshold = 75;

        hearingThreshold -= GoldCarried;

        return hearingThreshold; 
    }

    #endregion



    #region STATE UPDATE

    public void IncreaseFatigue()
    {
        Fatigue++;
    }

    public void CreateTime()
    {
        this.createdTime++;
        this.waitedTime = 0;
    }

    public void IncreaseWaitedTime(int amount)
    {
        this.waitedTime += amount;
    }

    public void AddToMoneyInBank(int amount)
    {
        MoneyInBank += amount;
        GoldCarried = 0;
    }

    public void AddToGoldCarried(int amount)
    {
        GoldCarried += amount;
    }

    #endregion


    #region EVENTS

    public void handlerBankRobbery()
    {
        MoneyInBank -= 20;
        //Debug.Log("Bob: My money's been stolen!");
        Speak("My money's been stolen!");
    }

    #endregion



    #region MOVEMENT+PATHFINDING

    // Requests a movement path from the AStar pathfinder
    public void Go(Vector3 _destination)
    {
        //Debug.Log("Bob is Going");
        PathRequestManager.RequestPath(transform.position, _destination, false, OnPathFound); //false because its not a hearing check
    }

    // Requests an auditory path from the AStar pathfinder
    public void ListenOut(Vector3 interestPoint)
    {
        //Debug.Log("Bob is Going");
        PathRequestManager.RequestPath(transform.position, interestPoint, true, OnSoundPathFound); //true because it is a hearing check!
    }

    //Supplied by the path request manager when a path is found
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful, bool isSoundPath)
    {
        //if its a movement path, get the agent to follow it
        if (pathSuccessful && !isSoundPath)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }


    public void OnSoundPathFound(Vector3[] newPath, bool pathSuccessful, bool isSoundPath)
    {
        //if its a sound path, see if the item of interest was within the auditory threshold
        if (pathSuccessful && isSoundPath)
        {
            //Debug.Log(newPath.Length);

            if (newPath.Length <= CheckHearing())
            {
                Speak("Sounds like that outlaw!");
                //Debug.Log("Bob heard something!");
            }
            else
            {
                Speak("Its pretty quiet, but I'm sure rich!");
                //Debug.Log("Bob heard nothing");
            }  
        }
    }



    IEnumerator FollowPath()
    {
        if (path != null)
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

    // These methods are called when the agent collides with certain location gameobjects

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