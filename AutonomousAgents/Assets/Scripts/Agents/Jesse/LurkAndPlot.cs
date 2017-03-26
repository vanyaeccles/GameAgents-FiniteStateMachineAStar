using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public sealed class LurkAndPlot : State<Outlaw>
{

    static readonly LurkAndPlot instance = new LurkAndPlot();

    public static LurkAndPlot Instance
    {
        get
        {
            return instance;
        }
    }

    static LurkAndPlot() { }
    private LurkAndPlot() { }

    public override void Enter(Outlaw agent)
    {
        //agent.ChangeLocation(Locations.moving);
        Debug.Log("Jesse: Another day of sweet crime!");
        agent.waitedTime = 0;
        agent.missingSal = UnityEngine.Random.Range(0, 10);

    }

    public override void Execute(Outlaw agent)
    {
        if (agent.Location != Locations.outlawCamp)
        {
            Debug.Log("Jesse: On ma way back to camp...");
        }

        //Checks if the agent is at their destination
        if (agent.Location == Locations.outlawCamp)
        {
            agent.IncreaseWaitedTime(1);
            Debug.Log("Jesse has been lurking and plotting for " + agent.waitedTime + " minute" + (agent.waitedTime > 1 ? "s" : "") + " so far...");

            if (agent.WaitedLongEnough() && agent.missingSal < 5)
            {
                agent.Go(agent.jesseGrid.bankPos);
                agent.ChangeState(RobTheBank.Instance);
            }
            else if (agent.WaitedLongEnough() && agent.missingSal >= 5)
            {
                agent.Go(agent.jesseGrid.cemeteryPos);
                agent.ChangeState(VisitCemetery.Instance);
            }
        }
    }

    public override void Exit(Outlaw agent)
    {
        Debug.Log("Jesse: I'm getting out of here!");
    }
}

