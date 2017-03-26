using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;




public class Outlaw : MonoBehaviour
{

    private StateMachine<Outlaw> stateMachine;

    

    public Locations Location = Locations.outlawCamp;
    public Grid jesseGrid;


    public int GoldCarried = 0;
    public int MoneyInBank = 0;
    public int Thirst = 0;
    public int Fatigue = 0;
    public int missingSal;

    public int WAIT_TIME;
    public int JAIL_SENTANCE;
    public int waitedTime = 0;
    public int createdTime = 0;



    //create robbery event
    public delegate void bankRobbery();
    public static event bankRobbery OnBankRobbery;

    //create death event
    public delegate void jesseDead();
    public static event jesseDead OnJesseDead;
    public bool isDead;
    private CapsuleCollider jesseCol;

    // AStar details
    //private AStarPathfinder bobLocManager;
    private Transform destination; // The position of the destination target
    float speed = 10; // Speed of the agent's movement
    Vector3[] path; // A vector3 array containing the nodes in the path
    int targetIndex;


    public void Awake()
    {
        Debug.Log("Jesse the outlaw is waking up...");
        this.stateMachine = new StateMachine<Outlaw>();


        this.missingSal = Random.Range(0, 10);
        
        this.JAIL_SENTANCE = Random.Range(70, 120);
        this.WAIT_TIME = Random.Range(20, 50);

        this.jesseCol = GetComponent<CapsuleCollider>();


        jesseGrid = GameObject.Find("GameManager").GetComponent<Grid>();

        transform.position = jesseGrid.outlawCampPos;
    }

    public void Start()
    {
        Go(jesseGrid.outlawCampPos);
        this.stateMachine.Init(this, LurkAndPlot.Instance, OutlawGlobalState.Instance);
    }

    public void ChangeLocation(Locations l)
    {
        Location = l;
    }


    public void StealSomeGold()
    {
        int amount = Random.Range(1, 11);
        GoldCarried += amount;
    }

    public bool checkLuck()
    {
        //Debug.Log("Checking");
        int luck = Random.Range(0, 10);

        if (luck < 7) return false;

        else return true;
    }

   public void robBank()
    {
        //Note that if there is no Bob instanciated, Jesse will be stuck in the bank
        //OnBankRobbery();
        StealSomeGold();
    }

    public void getShotDead()
    {
        isDead = true;
        StopCoroutine("FollowPath");

        this.transform.rotation = Quaternion.AngleAxis(90, Vector3.left);
        jesseCol.radius = 4.0f;

        //Note that if there is no undertaker instanciated, Jesse will be stuck
        OnJesseDead();
        ChangeState(InNeedOfRepair.Instance);
    }

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Jesse Collide");
        if (c.gameObject.tag == "Undertaker")
        {
            //Debug.Log("Undertaker Collide");
            if(isDead)
            {
                GameObject.Find("Undertaker").SendMessage("UndertakerAtRobot");
            }    
        }
    }

    public void JesseIsFixed()
    {
        isDead = false;

        this.transform.rotation = Quaternion.AngleAxis(0, Vector3.left);
        jesseCol.radius = 0.5f;

        Go(jesseGrid.outlawCampPos);
        ChangeState(LurkAndPlot.Instance);
    }

    public void Drag()
    {
        if(isDead)
            this.transform.position = GameObject.Find("Undertaker").GetComponent<Transform>().position;
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
        bool full = GoldCarried == 2 ? true : false;
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

    public bool CompletedJailSentance()
    {
        if (this.waitedTime >= JAIL_SENTANCE)
            return true;
        else
            return false;
    }


    public void ChangeState(State<Outlaw> state)
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

        //For debugging the undertaker behaviour
        if (Input.GetKeyDown("space"))
            getShotDead();
    }

    #region MOVEMENT+PATHFINDING

    public void Go(Vector3 _destination)
    {
        //Debug.Log("Jesse is Going");
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
                Gizmos.color = Color.red;
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


    public void JesseAtOutlawCamp()
    {
        ChangeLocation(Locations.outlawCamp);
    }

    public void JesseAtBank()
    {
        ChangeLocation(Locations.bank);
    }

    public void JesseAtCemetery()
    {
        ChangeLocation(Locations.cemetery);
    }

    public void JesseAtJailHouse()
    {
        ChangeLocation(Locations.jailhouse);
    }

    #endregion
}