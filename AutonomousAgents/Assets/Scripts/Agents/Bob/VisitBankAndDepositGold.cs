using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public sealed class VisitBankAndDepositGold : State<Bob> {

    static readonly VisitBankAndDepositGold instance = new VisitBankAndDepositGold();

    public static VisitBankAndDepositGold Instance
    {
        get
        {
            return instance;
        }
    }

    static VisitBankAndDepositGold() { }
    private VisitBankAndDepositGold() { }

    public override void Enter(Bob agent)
    {
        agent.ChangeLocation(Locations.moving);
        Debug.Log("Bob: On the way to the bank...");
    }

    public override void Execute(Bob agent)
    {
        if (agent.Location != Locations.bank)
        {
            Debug.Log("Bob: On the way to the bank...");
        }


        //Checks if the agent is at their destination
        if (agent.Location == Locations.bank)
        {
            Debug.Log("Bob: Feeding The System with MY gold... " + agent.MoneyInBank);
            agent.AddToMoneyInBank(agent.GoldCarried);

            //Go back to mining
            agent.ChangeState(EnterMineAndDigForNuggets.Instance);
            agent.Go(agent.bobGrid.goldminePos);
        }
    }


    public override void Exit(Bob agent)
    {
        Debug.Log("Bob: Leaving the bank...");
    }

}
