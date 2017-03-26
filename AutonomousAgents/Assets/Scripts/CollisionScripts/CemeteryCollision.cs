using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CemeteryCollision : MonoBehaviour {

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Collision");

        if (c.gameObject.tag == "Jesse")
        {
            //Jesse is at the cemetery
            GameObject.Find("Jesse").SendMessage("JesseAtCemetery");
        }

        if (c.gameObject.tag == "Sheriff")
        {
            GameObject.Find("Sheriff").SendMessage("AtLocation");
        }

    }
}
