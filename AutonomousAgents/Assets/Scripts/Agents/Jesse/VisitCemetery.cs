using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


public sealed class VisitCemetery : State<Outlaw>
{

    static readonly VisitCemetery instance = new VisitCemetery();

    public static VisitCemetery Instance
    {
        get
        {
            return instance;
        }
    }

    static VisitCemetery() { }
    private VisitCemetery() { }

    public override void Enter(Outlaw agent)
    {
        //if (agent.Location != Locations.cemetery)
        //{
        agent.ChangeLocation(Locations.moving);
        Debug.Log("Jesse: Darn I miss Sal...");
        agent.waitedTime = 0;
        //}
    }

    public override void Execute(Outlaw agent)
    {
        if (agent.Location != Locations.cemetery)
        {
            Debug.Log("Jesse: On my way to the cemetery to visit Sal's grave..");
        }

        //Checks if the agent is at their destination
        if (agent.Location == Locations.cemetery)
        {
            agent.IncreaseWaitedTime(1);
            Debug.Log("Jesse has been mourning for " + agent.waitedTime + " cycle" + (agent.waitedTime > 1 ? "s" : "") + " so far...");

            agent.missingSal--;

            if (agent.WaitedLongEnough() && agent.missingSal < 2)
            {
                agent.Go(agent.jesseGrid.outlawCampPos);
                agent.ChangeState(LurkAndPlot.Instance);
                Debug.Log("Jesse: I'll bet your raisin' hell in heaven");
            }
        }
    }





    public override void Exit(Outlaw agent)
    {
        Debug.Log("Jesse: I will have revenge!");
    }
}
