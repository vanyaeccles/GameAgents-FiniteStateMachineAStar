using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FixRobot : State<Undertaker>
{


    static readonly FixRobot instance = new FixRobot();

    public static FixRobot Instance
    {
        get
        {
            return instance;
        }
    }

    static FixRobot() { }
    private FixRobot() { }


    public override void Enter(Undertaker agent)
    {
        agent.ChangeLocation(Locations.moving);

        //Debug.Log("Undertaker: I hope the dust doesn't get into the model's microphone again...");
        agent.Speak("I hope the dust doesn't get into the model's microphone again...");
    }



    public override void Execute(Undertaker agent)
    {

        if (!agent.isAtBrokenRobot())
        {
            //Debug.Log("Undertaker: I hope the dust doesn't get into the t1000's microphone again...");
            agent.Speak("I hope the dust doesn't get into the t1000's microphone again...");
        }

        ////Checks if the agent is at their destination
        if (agent.isAtBrokenRobot())
        {
            //Debug.Log("Undertaker: Ah, come with me please");
            agent.Speak("Ah, come with me please");
            agent.DragRobot();

            //Expensive
            agent.Go(agent.undertakerGrid.workshopPos);

            if (agent.Location == Locations.robotWorkshop)
            {
                //Debug.Log("Undertaker: Just needs a new robot heart");
                agent.Speak("Just needs a new robot heart");
                GameObject.Find("Jesse").SendMessage("JesseIsFixed");
                agent.isWithRobot = false;
                agent.ChangeState(TinkerWithRobots.Instance);
            }
        }
    }



    public override void Exit(Undertaker agent)
    {
        //Debug.Log("Undertaker: the model is fixed");
        agent.Speak("Wonderful, the model is fixed");
    }
}