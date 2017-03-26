using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabCollision : MonoBehaviour
{

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Collision");

        if (c.gameObject.tag == "Undertaker")
        {
            GameObject.Find("Undertaker").SendMessage("UndertakerAtLab");
        }

    }
}