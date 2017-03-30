using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;



 public sealed class EnterMineAndDigForNuggets : State<Bob> {

    static readonly EnterMineAndDigForNuggets instance = new EnterMineAndDigForNuggets();

    public static EnterMineAndDigForNuggets Instance
    {
        get
        {
            return instance;
        }
    }

    static EnterMineAndDigForNuggets() { }
    private EnterMineAndDigForNuggets() { }

    public override void Enter(Bob agent)
    {
        //Debug.Log("Bob: On my way to the mine...");
        agent.Speak("On my way to the mine...");
        agent.ChangeLocation(Locations.moving);
    }

    public override void Execute(Bob agent)
    {
        if (agent.Location != Locations.goldmine)
        {
            agent.Speak("On my way to the gold mine...");
            //Debug.Log("Bob: On my way to the gold mine...");
        }


        //Checks if the agent is at their destination
        if (agent.Location == Locations.goldmine)
        {
            agent.AddToGoldCarried(1);
            //Debug.Log("Bob: Picking up a nugget and that's..." +
            //  agent.GoldCarried);

            agent.Speak("Picking up a nugget and that's..." + agent.GoldCarried);

            agent.IncreaseFatigue();
            if (agent.PocketsFull())
            {
                agent.Go(agent.bobGrid.bankPos);
                agent.ChangeState(VisitBankAndDepositGold.Instance);
            }
        }    
    }

    public override void Exit(Bob agent)
    {
        //Debug.Log("Bob: Leaving the mine with my pockets full...");
        agent.Speak("Leaving the mine with my pockets full...");
    }
}
