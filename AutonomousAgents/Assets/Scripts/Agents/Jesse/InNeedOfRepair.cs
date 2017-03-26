using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InNeedOfRepair : State<Outlaw>
{

    static readonly InNeedOfRepair instance = new InNeedOfRepair();



    public static InNeedOfRepair Instance
    {
        get
        {
            return instance;
        }
    }

    static InNeedOfRepair() { }
    private InNeedOfRepair() { }


    public override void Enter(Outlaw agent)
    {
        agent.ChangeLocation(Locations.broken);

        Debug.Log("Jesse: BBZZzzzzzzzzasdhliuewnkjdsndodsif");
    }

    public override void Execute(Outlaw agent)
    {
        if (agent.Location != Locations.robotWorkshop)
        {
            Debug.Log("Jesse: 01010111002");
        }
    }


    public override void Exit(Outlaw agent)
    {
        Debug.Log("Jesse: Feeling fine!");
    }
}
