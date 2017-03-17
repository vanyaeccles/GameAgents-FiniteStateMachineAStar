using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldmineCollision : MonoBehaviour {

    void OnCollisionEnter(Collision c)
    {
        //Debug.Log("Collision");

            if (c.gameObject.tag == "Bob")
            {
                //Debug.Log("Bob is Here");
                //Bob is at the goldmine
                GameObject.Find("Bob").SendMessage("BobAtGoldmine");
            }

    }
}
