using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour {

    public GameObject obj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("DRAWING");
        //Debug.DrawLine(this.transform.position, this.transform.up * 100, Color.red, 100.0f);

        obj.layer = 0;
    }
}
