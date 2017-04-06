using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlawCampCollision : MonoBehaviour {

    ParticleSystem smoke;

    void Start()
    {
        smoke = GameObject.Find("CampFire").GetComponent<ParticleSystem>();
        smoke.Play();
    }

    

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Collision");

        if (c.gameObject.tag == "Jesse")
        {
            //Debug.Log("Bob is Here");
            GameObject.Find("Jesse").SendMessage("JesseAtOutlawCamp");
            smoke.Play();
        }

    }


}
