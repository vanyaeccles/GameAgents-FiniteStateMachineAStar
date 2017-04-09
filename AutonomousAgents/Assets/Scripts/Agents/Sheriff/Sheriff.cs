using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

/*
 *  This class defines the agent Sheriff
 */

public class Sheriff : MonoBehaviour
{

    private StateMachine<Sheriff> stateMachine;
    private Soul sheriffSoul;

    // AStar details
    private Transform destination; // The position of the destination target
    const float speed = 10; // Speed of the agent's movement
    Vector3[] path; // A vector3 array containing the nodes in the path
    int targetIndex;

    public Locations Location = Locations.jailhouse;
    public Grid sheriffGrid;



    public int GoldCarried = 10;
    public int MoneyInBank = 0;
    public int Thirst = 0;
    public int Fatigue = 0;
    public int bullets = 5;
    public int restedness = 0;
    private int sleepThreshold;
    private float sightPerceptiveness;

    public static int WAIT_TIME = 5;
    public int waitedTime = 0;
    public int createdTime = 0;
    public bool isAtALocation;

    static Random rnd = new Random();

    private TextMesh sheriffSpeech;


    #region STATE MACHINE + BASIC AGENT METHODS

    public void Awake()
    {
        //Debug.Log("Sheriff is waking up...");
        this.stateMachine = new StateMachine<Sheriff>();

        Outlaw.OnBankRobbery += handlerBankRobbery;

        sleepThreshold = Random.Range(40, 150);
        sightPerceptiveness = 20.0f;

        sheriffGrid = GameObject.Find("GameManager").GetComponent<Grid>();
        sheriffSpeech = GameObject.Find("SheriffText").GetComponent<TextMesh>();

        transform.position = sheriffGrid.jailhousePos;
    }

    public void Start()
    {

        Go(sheriffGrid.saloonPos);
        this.stateMachine.Init(this, DrinkAtTheSaloon.Instance, SheriffGlobalState.Instance);
    }

    public void Update()
    {
        Thirst++;
        this.stateMachine.Update();
        LookAhead();

        if (sightPerceptiveness < 20.0f && sightPerceptiveness > 0.0f)
        {
            if (sheriffGrid.isDayTime)
                sightPerceptiveness += 0.5f;
            else if (!sheriffGrid.isDayTime)
                sightPerceptiveness -= 0.5f;
        } 
    }

    public void ChangeState(State<Sheriff> state)
    {
        this.stateMachine.ChangeState(state);
    }

    public void RevertToPreviousState()
    {
        this.stateMachine.RevertToPreviousState();
    }


    public void ChangeLocation(Locations l)
    {
        Location = l;
    }

    public void Speak(string text)
    {
        sheriffSpeech.text = text;
    }

    #endregion



    #region STATE CHECKS

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

    public bool OutofMoney()
    {
        if (GoldCarried == 0)
            return true;
        else
            return false;
    }

    #endregion



    #region STATE UPDATE

    public void ShootBullet()
    {
        bullets--;
    }

    public void GetMoreAmmo()
    {
        bullets = 6;
    }

    public void DrinkWhisky()
    {
        GoldCarried--;
        if (sightPerceptiveness > 5.0f)
            sightPerceptiveness -= 1.0f;
    }

    #endregion

    #region EVENTS

    public void handlerBankRobbery()
    {
        Speak("A Bank Robbery! Deputy go and arrest the thief!");
    }

    #endregion

    #region SENSES 

    public void LookAhead()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, sightPerceptiveness);

        foreach (RaycastHit hit in hits)
        {
            if (hits.Length > 0)
            {
                //calls a sight event to process the seen object
                SightEvent(hit);
            }
        }
    }

    //A sight event handler for raycasts
    public void SightEvent(RaycastHit hit)
    {
        if (hit.collider.tag == "Jesse")
        {
            //Debug.LogError("Sheriff: There's that dirty scoundrel Jesse!");
            Speak("There's that dirty scoundrel Jesse!");
            if (bullets > 0)
                TakeAimAndFire();
        }
        if (hit.collider.tag == "Elsa")
        {
            Speak("Howdy Elsa");
        }
        if (hit.collider.tag == "Bob")
        {
            Speak("Howdy Bob");
        }
        if (hit.collider.tag == "Undertaker")
        {
            Speak("Doesn't look like anything to me!");
        }
    }

    //A scent event handler for sphere colliders
    public void ScentEvent(Collider hit)
    {
        if (hit.tag == "House")
        {
            Speak("I smell some darn good smelling stew!");
        }

        if (hit.tag == "OutlawCamp")
        {
            Speak("Wheres that smokey smell coming from?");
        }

        if (hit.tag == "Jesse")
        {
            //Debug.LogError("Sheriff: There's that dirty scoundrel Jesse!");
            Speak("I smell that darn stinky scoundrel Jesse!");
            if (bullets > 0)
                TakeAimAndFire();
        }
    }

    //Does a 360 sphere check to inspect a location with the Sheriff's great sense of smell!
    public bool SniffOutLocation()
    {
        Vector3 windDir = sheriffGrid.windDirection;

        // performs an overlap sphere (sphere center adjusted for current wind) to check a location, if he smells Jesse he'll try shoot him
        Collider[] smellHits = Physics.OverlapSphere(transform.position + windDir, sightPerceptiveness);

        if (smellHits.Length > 0)
        {
            foreach (Collider i in smellHits)
            {
                ScentEvent(i);
            }
            return true;
        }
        return false;
    }

    #endregion



    public void TakeAimAndFire()
    {
        bullets--;

        int hitJesse = Random.Range(3, 20);

        if (hitJesse > 10)
        {
            GameObject.Find("Jesse").SendMessage("getShotDead");

            //Debug.LogError("Sheriff: Got that yella belly!");
            Speak("Got that yella belly!");
        }

        else Speak("Darnit I missed him!"); //Debug.Log("Sheriff: Darnit I missed him!");
    }



    public Vector3 ChooseRandomLocation()
    {
        //int r = rnd.Next(sheriffGrid.locationPositions.Count);

        //Debug.Log("Chosen");

        Vector3 randomLocPos = sheriffGrid.locationPositions[Random.Range(0, sheriffGrid.locationPositions.Count)];

        return randomLocPos;
    }

   


    #region MOVEMENT+PATHFINDING

    public void Go(Vector3 _destination)
    {
        isAtALocation = false;
        PathRequestManager.RequestPath(transform.position, _destination, false, OnPathFound);
    }


    public void OnPathFound(Vector3[] newPath, bool pathSuccessful, bool isSoundPath)
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

   

    public void Snooze()
    {
        restedness++;

        if(sightPerceptiveness < 20.0f)
            sightPerceptiveness += 1.0f;
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
        GetMoreAmmo();
        //Debug.Log("Sheriff: Got me s'more cash and ammo from the safe");
        Speak("Got me s'more cash and ammo from the safe");
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