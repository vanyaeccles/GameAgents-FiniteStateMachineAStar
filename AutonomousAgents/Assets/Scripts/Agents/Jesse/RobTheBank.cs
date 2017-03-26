using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


public sealed class RobTheBank : State<Outlaw>
{

    static readonly RobTheBank instance = new RobTheBank();

    public static RobTheBank Instance
    {
        get
        {
            return instance;
        }
    }

    static RobTheBank() { }
    private RobTheBank() { }

    public override void Enter(Outlaw agent)
    {
        //if (agent.Location != Locations.bank)
        //{
            Debug.Log("Jesse: Just a law abiding citizen on his way to the bank");
        //}
    }

    

    public override void Execute(Outlaw agent)
    {
        if (agent.Location != Locations.bank)
        {
            Debug.Log("Jesse: Just a law abiding citizen on his way to the bank");
        }

        if (agent.Location == Locations.bank)
        {
            Debug.Log("Jesse: This is a stick-up! Everyone on the floor!");


            //start the event
            agent.robBank();


            bool lucky = agent.checkLuck();

            if (!lucky)
            {

                Debug.Log("Jesse: The Sheriff! ... I'll come quietly...");
                agent.GoldCarried = 0;
                agent.ChangeState(JailTime.Instance);
                agent.Go(agent.jesseGrid.jailhousePos);
            }

            else
            {
                agent.Go(agent.jesseGrid.outlawCampPos);
                Debug.Log("Jesse: YeeHaw! Got away with it!");
                agent.ChangeState(LurkAndPlot.Instance);
            }
        }
    }





    public override void Exit(Outlaw agent)
    {
        Debug.Log("Jesse is leaving the bank");
    }

    
   

}

