using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailhouseCollision : MonoBehaviour {

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Collision");

        if (c.gameObject.tag == "Jesse")
        {
            GameObject.Find("Jesse").SendMessage("JesseAtJailHouse");
        }

        if (c.gameObject.tag == "Sheriff")
        {
            GameObject.Find("Sheriff").SendMessage("SheriffAtJailHouse");
        }
    }
}
