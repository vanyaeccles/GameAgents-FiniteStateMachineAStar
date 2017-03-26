using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TinkerWithRobots : State<Undertaker>
{


    static readonly TinkerWithRobots instance = new TinkerWithRobots();

    public static TinkerWithRobots Instance
    {
        get
        {
            return instance;
        }
    }

    static TinkerWithRobots() { }
    private TinkerWithRobots() { }


    public override void Enter(Undertaker agent)
    {
        agent.ChangeLocation(Locations.moving);

        Debug.Log("Undertaker: More work to be done back at the lab!");
    }

    public override void Execute(Undertaker agent)
    {

        if (agent.Location != Locations.robotWorkshop)
        {
            Debug.Log("Undertaker: More work to be done back at the lab!");
        }

        ////Checks if the agent is at their destination
        if (agent.Location == Locations.robotWorkshop)
        {
            Debug.Log("Undertaker: These robots keep breaking!");
        }
    }


    public override void Exit(Undertaker agent)
    {
        Debug.Log("Undertaker: Duty calls!");
    }
}
