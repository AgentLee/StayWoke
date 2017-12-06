using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHMDController : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
        // For whatever reason, the layer gets changed to ignore ray casting.
        // We need this for debugging.
        player.layer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
