using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;



public sealed class MovingState : State<Bob>
{

    static readonly MovingState instance = new MovingState();

    public static MovingState Instance
    {
        get
        {
            return instance;
        }
    }

    static MovingState() { }
    private MovingState() { }

    public override void Enter(Bob agent)
    {
        if (agent.Location != Locations.goldmine)
        {
            Debug.Log("Bob: On the move");
            agent.ChangeLocation(Locations.goldmine);
        }
    }

    public override void Execute(Bob agent)
    {
        Debug.Log("Bob: Picking up a nugget and that's..." + agent.GoldCarried);
        agent.IncreaseFatigue();
        if (agent.PocketsFull())
        {
            agent.ChangeState(VisitBankAndDepositGold.Instance);
            //agent.Go();
        }

    }

    public override void Exit(Bob agent)
    {
        Debug.Log("Bob: Leaving the mine with my pockets full...");
    }
}
