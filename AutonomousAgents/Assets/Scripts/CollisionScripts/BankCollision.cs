﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankCollision : MonoBehaviour {

    void OnCollisionEnter(Collision c)
    {
        //Debug.Log("Collision");

        if (c.gameObject.tag == "Bob")
        {
            //Debug.Log("Bob is Here");
            GameObject.Find("Bob").SendMessage("BobAtBank");
        }

        if (c.gameObject.tag == "Jesse")
        {
            GameObject.Find("Jesse").SendMessage("JesseAtBank");
        }

    }
}
