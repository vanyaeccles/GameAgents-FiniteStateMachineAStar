using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrairieCollision : MonoBehaviour {

    void OnCollisionEnter(Collision c)
    {
        Debug.Log("Prairie Collision");

        if (c.gameObject.tag == "Elsa")
        {
            GameObject.Find("Elsa").SendMessage("ElsaAtPrairie");
        }
    }
}
