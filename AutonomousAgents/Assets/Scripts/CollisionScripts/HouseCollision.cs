using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCollision : MonoBehaviour {

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Collision");

        if (c.gameObject.tag == "Bob")
        {
            //Debug.Log("Bob is Here");
            GameObject.Find("Bob").SendMessage("BobAtHouse");
        }

        if (c.gameObject.tag == "Elsa")
        {
            GameObject.Find("Elsa").SendMessage("ElsaAtHouse");
        }

        if (c.gameObject.tag == "Sheriff")
        {
            GameObject.Find("Sheriff").SendMessage("AtLocation");
        }
    }
}
