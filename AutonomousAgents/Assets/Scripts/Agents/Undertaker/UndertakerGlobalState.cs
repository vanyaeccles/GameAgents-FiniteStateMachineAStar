using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndertakerGlobalState : State<Undertaker>
{

    static readonly UndertakerGlobalState instance = new UndertakerGlobalState();

    public static UndertakerGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static UndertakerGlobalState() { }
    private UndertakerGlobalState() { }

    public override void Enter(Undertaker agent)
    {
        Debug.Log("Undertaker: I'm entering a blip");
    }

    public override void Execute(Undertaker agent)
    {
        bool thirsty = agent.Thirsty();
        if (thirsty)
        {
            Debug.Log("Undertaker: The difference is: I'm ACTUALLY thirsty");
            agent.Thirst = 0;
        }


    }

    public override void Exit(Undertaker agent)
    {
        agent.RevertToPreviousState();
        Debug.Log("Undertaker: I'm leaving a blip");
    }
}