using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SleepOnTheJob : State<Sheriff>
{


    static readonly SleepOnTheJob instance = new SleepOnTheJob();

    public static SleepOnTheJob Instance
    {
        get
        {
            return instance;
        }
    }

    static SleepOnTheJob() { }
    private SleepOnTheJob() { }

    Vector3 chosenLocPos;


    public override void Enter(Sheriff agent)
    {
        agent.ChangeLocation(Locations.moving);

        Debug.Log("Sheriff: Back to the office for a nap!");

    }



    public override void Execute(Sheriff agent)
    {

        if (agent.Location != Locations.jailhouse)
        {
            Debug.Log("Sheriff: Back to the office to take a look at that paperwork!");
        }

        ////Checks if the agent is at their destination
        if (agent.Location == Locations.jailhouse)
        {
            agent.Snooze();

            Debug.Log("Sheriff: ZZzzzzzzz");

            if(agent.SleptLongEnough())
            {
                Debug.Log("Sheriff: ... !!! Oh! I.. Just resting my eyes for a second");
                agent.ChangeState(OnPatrol.Instance);
            }
        }
    }

    public override void Exit(Sheriff agent)
    {
        Debug.Log("Sheriff: Goin' for a drink!");
    }
}
