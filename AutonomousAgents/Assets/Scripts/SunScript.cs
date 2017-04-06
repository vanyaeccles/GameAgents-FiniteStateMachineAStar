using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunScript : MonoBehaviour {

    public Vector3 windDirection;


	// Use this for initialization
	void Start ()
    {
        windDirection = new Vector3(0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.RotateAround(Vector3.zero, Vector3.forward, 5 * Time.deltaTime);

        windDirection[0] = Mathf.Sin(1 * Time.deltaTime);
    }
}
