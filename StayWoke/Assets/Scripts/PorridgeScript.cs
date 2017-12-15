using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorridgeScript : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		player = GetComponent<PlayerHMDController> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
				
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" || other.tag == "Left" || other.tag == "Right") {
			Debug.Log (other.tag);
			player.GetComponent<PlayerHMDController> ().hasPooridge = true;
		}
	}
}
