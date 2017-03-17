using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;



public sealed class ElsaGlobalState : State<Elsa>
{

    static readonly ElsaGlobalState instance = new ElsaGlobalState();

    public static ElsaGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static ElsaGlobalState() { }
    private ElsaGlobalState() { }

    public override void Enter(Elsa agent)
    {
        Debug.Log("Elsa: I'm entering a blip");
    }

    public override void Execute(Elsa agent)
    {
        bool thirsty = agent.Thirsty();
        if (thirsty)
        {
            Debug.Log("Elsa: I'm having a drink of water!");
            agent.Thirst = 0;
        }


    }

    public override void Exit(Elsa agent)
    {
        agent.RevertToPreviousState();
        Debug.Log("Elsa: I'm leaving a blip");
    }
}