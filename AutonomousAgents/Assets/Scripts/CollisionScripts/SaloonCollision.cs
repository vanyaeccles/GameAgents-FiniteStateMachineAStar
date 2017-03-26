using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaloonCollision : MonoBehaviour {

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Collision");

        if (c.gameObject.tag == "Sheriff")
        {
            //Debug.Log("Bob is Here");
            GameObject.Find("Sheriff").SendMessage("SheriffAtSaloon");
        }

        if (c.gameObject.tag == "Sheriff")
        {
            GameObject.Find("Sheriff").SendMessage("AtLocation");
        }
    }
}
