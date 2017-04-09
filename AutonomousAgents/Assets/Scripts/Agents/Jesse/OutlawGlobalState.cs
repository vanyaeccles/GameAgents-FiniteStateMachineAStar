using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;



public sealed class OutlawGlobalState : State<Outlaw>
{

    static readonly OutlawGlobalState instance = new OutlawGlobalState();

    public static OutlawGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static OutlawGlobalState() { }
    private OutlawGlobalState() { }

    public override void Enter(Outlaw agent)
    {
        //Debug.Log("Jesse: I wonder...");
        agent.Speak("I wonder...");
    }


    public override void Execute(Outlaw agent)
    {
        bool thirsty = agent.isThirsty();
        if (thirsty)
        {
            //Debug.Log("Jesse: What if none of this is really real?");
            agent.Speak("What if none of this is really real?");
            agent.Thirst = 0;
        }
    }

    public override void Exit(Outlaw agent)
    {
        //Debug.Log("Jesse: I'm done daydreamin'");
        agent.Speak("I'm done daydreamin'");
        agent.RevertToPreviousState();
    }
}