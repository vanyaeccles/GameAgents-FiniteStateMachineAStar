using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OnPatrol : State<Sheriff>
{


    static readonly OnPatrol instance = new OnPatrol();

    public static OnPatrol Instance
    {
        get
        {
            return instance;
        }
    }

    static OnPatrol() { }
    private OnPatrol() { }

    Vector3 chosenLocPos;


    public override void Enter(Sheriff agent)
    {
        agent.ChangeLocation(Locations.moving);

        Debug.Log("Sheriff: Going on patrol!");

        //Pick a random location
        chosenLocPos = agent.ChooseRandomLocation();

        //Debug.Log(chosenLocPos);

        //Go to that location
        agent.Go(chosenLocPos);
    }



    public override void Execute(Sheriff agent)
    {

        if (!agent.isAtALocation)
        {
            Debug.Log("Sheriff: On patrol!");
        }

        ////Checks if the agent is at their destination
        if (agent.isAtALocation)
        {
            Debug.Log("Sheriff: What have we here?");

            if(!agent.InspectLocation())
            {
                Debug.Log("Sheriff: Nothing seems outta the order here");

                agent.Go(agent.sheriffGrid.jailhousePos);
                agent.ChangeState(SleepOnTheJob.Instance);
            }
            
        }





    }

    public override void Exit(Sheriff agent)
    {
        Debug.Log("Sheriff: Goin' for a drink!");
    }
}
