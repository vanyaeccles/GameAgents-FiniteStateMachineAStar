using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;



public sealed class JailTime : State<Outlaw>
{

    static readonly JailTime instance = new JailTime();

    public static JailTime Instance
    {
        get
        {
            return instance;
        }
    }

    static JailTime() { }
    private JailTime() { }

    public override void Enter(Outlaw agent)
    {
        //if (agent.Location != Locations.jailhouse)
        //{
        //Debug.Log("Jesse: I'll come quietly");
        agent.Speak("I'll come quietly");
            agent.waitedTime = 0;
        //}
    }

    public override void Execute(Outlaw agent)
    {
        if (agent.Location != Locations.jailhouse)
        {
            //Debug.Log("Jesse: I'll come quietly");
            agent.Speak("I'll come quietly");
        }

        if (agent.Location == Locations.jailhouse)
        {
            agent.IncreaseWaitedTime(1);
            //Debug.Log("Jesse: In jail for " + agent.waitedTime + " turns");
            agent.Speak("In jail for " + agent.waitedTime + " turns");

            if (agent.CompletedJailSentance())
            {
                //Debug.Log("Jesse: No jail house can hold me in for long!");
                agent.Speak("No jail house can hold me in for long!");
                agent.Go(agent.jesseGrid.outlawCampPos);
                agent.ChangeState(LurkAndPlot.Instance);
            }

        }

    }

    public override void Exit(Outlaw agent)
    {
        //Debug.Log("Jesse: Free at last! Glad I'm out of that cage...");
        agent.Speak("Free at last! Glad I'm out of that cage...");
    }
}

