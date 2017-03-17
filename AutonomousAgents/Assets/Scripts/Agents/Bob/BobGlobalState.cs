using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;



public sealed class BobGlobalState : State<Bob>
{

    static readonly BobGlobalState instance = new BobGlobalState();

    public static BobGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static BobGlobalState() { }
    private BobGlobalState() { }

    public override void Enter(Bob agent)
    {
        Debug.Log("Bob: I'm entering a blip");
    }

    public override void Execute(Bob agent)
    {
        bool thirsty = agent.Thirsty();
        if (thirsty)
        {
            Debug.Log("Bob: I'm having a drink of water!");
            agent.Thirst = 0;
        }

        
        
    }

    public override void Exit(Bob agent)
    {
        agent.RevertToPreviousState();
        Debug.Log("Bob: I'm leaving a blip");
    }
}