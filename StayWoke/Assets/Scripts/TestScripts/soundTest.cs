using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundTest : MonoBehaviour {

    public AudioSource audio;

    public GameObject player;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        float distance = (this.transform.position - player.transform.position).magnitude;
        //Debug.Log(distance);

	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(audio.isPlaying) {
                audio.Stop();
                Debug.Log("PLAYER");
                audio.Play();
            }
            else
            {
                audio.Play();
            }
        }
    }
}
