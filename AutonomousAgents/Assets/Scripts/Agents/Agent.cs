using UnityEngine;
using System.Collections;

abstract public class Agent : MonoBehaviour {

    //invokes Execute() for the agent’s current state and a ChangeState() method that allows that state object to change the agent’s state
    abstract public void Update ();


}