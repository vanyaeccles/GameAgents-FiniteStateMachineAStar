using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldmineCollision : MonoBehaviour {

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Collision");

           if (c.gameObject.tag == "Bob")
           {
                //Debug.Log("Bob is Here");
                //Bob is at the goldmine
                GameObject.Find("Bob").SendMessage("BobAtGoldmine");
            }

        if (c.gameObject.tag == "Sheriff")
        {
            GameObject.Find("Sheriff").SendMessage("AtLocation");
        }
    }
}
