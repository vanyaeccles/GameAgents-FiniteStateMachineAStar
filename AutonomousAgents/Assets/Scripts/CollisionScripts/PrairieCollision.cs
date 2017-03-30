using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrairieCollision : MonoBehaviour {

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Prairie Collision");

        if (c.gameObject.tag == "Elsa")
        {
            GameObject.Find("Elsa").SendMessage("ElsaAtPrairie");
        }

        if (c.gameObject.tag == "Sheriff")
        {
            GameObject.Find("Sheriff").SendMessage("AtLocation");
        }
    }
}
