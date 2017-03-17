using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


public sealed class GunPractise : State<Elsa>
{

    static readonly GunPractise instance = new GunPractise();

    public static GunPractise Instance
    {
        get
        {
            return instance;
        }
    }

    static GunPractise() { }
    private GunPractise() { }


    public override void Enter(Elsa agent)
    {
        agent.ChangeLocation(Locations.moving);

        Debug.Log("Elsa: Goin' Shootin!");
        
    }



    public override void Execute(Elsa agent)
    {
        if (agent.Location != Locations.prairie)
        {
            Debug.Log("Elsa: Goin' Shootin!");
        }

        //Checks if the agent is at their destination
        if (agent.Location == Locations.prairie)
        {
            Debug.Log("Elsa: Bang bang! " + agent.bullets + " bullets left!");
            agent.ShootBullet();

            if (agent.OutOfAmmo())
            {
                agent.Go(agent.elsaGrid.housePos);
                agent.ChangeState(MakingSomeDinner.Instance);
            } 
        }

    }





    public override void Exit(Elsa agent)
    {
        Debug.Log("Elsa: Outta Bullets! I'm gitting pretty good");

    }


}


