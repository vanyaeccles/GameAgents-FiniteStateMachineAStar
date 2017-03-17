using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailhouseCollision : MonoBehaviour {

    void OnCollisionEnter(Collision c)
    {
        //Debug.Log("Collision");

        if (c.gameObject.tag == "Jesse")
        {
            GameObject.Find("Jesse").SendMessage("JesseAtJailHouse");
        }

    }
}
