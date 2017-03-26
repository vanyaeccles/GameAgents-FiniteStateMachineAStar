using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlawCampCollision : MonoBehaviour {

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Collision");

        if (c.gameObject.tag == "Jesse")
        {
            //Debug.Log("Bob is Here");
            GameObject.Find("Jesse").SendMessage("JesseAtOutlawCamp");
        }

    }
}
