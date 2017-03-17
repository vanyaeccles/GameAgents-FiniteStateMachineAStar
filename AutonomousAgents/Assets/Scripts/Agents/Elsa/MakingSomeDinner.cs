using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public sealed class MakingSomeDinner: State<Elsa>
{

    static readonly MakingSomeDinner instance = new MakingSomeDinner();

    public static MakingSomeDinner Instance
    {
        get
        {
            return instance;
        }
    }

    static MakingSomeDinner() { }
    private MakingSomeDinner() { }

    public override void Enter(Elsa agent)
    {
        agent.ChangeLocation(Locations.moving);

        //if (agent.Location != Locations.house)
        //{
            Debug.Log("Elsa: On my way home..");
        //}
    }

    public override void Execute(Elsa agent)
    {
        if (agent.Location != Locations.house)
        {
            Debug.Log("Elsa: On my way home..");
        }

        //Checks if the agent is at their destination
        if (agent.Location == Locations.house)
        {
            Debug.Log("Elsa: This is great stew");

            agent.Go(agent.elsaGrid.prairiePos);
            agent.ChangeState(GunPractise.Instance);
        }
            
    }

    public override void Exit(Elsa agent)
    {
        agent.GetMoreAmmo();
        Debug.Log("Leaving the home, armed...");
    }

}