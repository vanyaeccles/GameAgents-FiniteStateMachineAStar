using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheriffGlobalState : State<Sheriff>
{

    static readonly SheriffGlobalState instance = new SheriffGlobalState();

    public static SheriffGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static SheriffGlobalState() { }
    private SheriffGlobalState() { }

    public override void Enter(Sheriff agent)
    {
        Debug.Log("Sheriff: I'm entering a blip");
    }

    public override void Execute(Sheriff agent)
    {
        bool thirsty = agent.Thirsty();
        if (thirsty)
        {
            Debug.Log("Sheriff: Jus' suppin' on ma whisky flask");
            agent.Thirst = 0;
        }


    }

    public override void Exit(Sheriff agent)
    {
        agent.RevertToPreviousState();
        Debug.Log("Sheriff: I'm leaving a blip");
    }
}
