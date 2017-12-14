using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinScript : MonoBehaviour {
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			Global.instance.Win ();
		}
	}
}
