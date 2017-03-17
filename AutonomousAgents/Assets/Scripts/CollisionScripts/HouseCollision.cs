using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCollision : MonoBehaviour {

    void OnCollisionEnter(Collision c)
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

    }
}
