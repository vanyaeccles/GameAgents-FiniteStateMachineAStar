using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlawCampCollision : MonoBehaviour {

    void OnCollisionEnter(Collision c)
    {
        //Debug.Log("Collision");

        if (c.gameObject.tag == "Jesse")
        {
            //Debug.Log("Bob is Here");
            GameObject.Find("Jesse").SendMessage("JesseAtOutlawCamp");
        }

    }
}
