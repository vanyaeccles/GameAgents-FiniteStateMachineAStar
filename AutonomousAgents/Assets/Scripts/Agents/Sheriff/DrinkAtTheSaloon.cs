using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DrinkAtTheSaloon : State<Sheriff>
{


    static readonly DrinkAtTheSaloon instance = new DrinkAtTheSaloon();

    public static DrinkAtTheSaloon Instance
    {
        get
        {
            return instance;
        }
    }

    static DrinkAtTheSaloon() { }
    private DrinkAtTheSaloon() { }

    Vector3 chosenLocPos;


    public override void Enter(Sheriff agent)
    {
        agent.ChangeLocation(Locations.moving);

        Debug.Log("Sheriff: Going for a drink!");

    }



    public override void Execute(Sheriff agent)
    {

        Debug.Log("Sheriff: Darn ahm thirsty");

        if (agent.Location != Locations.saloon)
        {
            Debug.Log("Sheriff: On official business to the saloon!");
        }

        //Checks if the agent is at their destination
        if (agent.Location == Locations.saloon)
        {
            if (!agent.OutofMoney())
            {
                Debug.Log("Sheriff: Mmmm thirsty work!");
                agent.DrinkWhisky();
            }
            
            else
            {
                Debug.Log("Sheriff: No cash! Gah better go do some policin'");
                agent.ChangeState(OnPatrol.Instance);
            }
        }
    }

    public override void Exit(Sheriff agent)
    {
        Debug.Log("Sheriff: Goin' for a drink!");
    }
}
