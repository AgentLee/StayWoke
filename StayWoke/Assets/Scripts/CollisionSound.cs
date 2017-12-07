using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour {

    public AudioClip audio;

	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = audio;
	}
	
    void OnCollisionEnter()
    {
        GetComponent<AudioSource>().Play();
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (!audio.isPlaying && collision.relativeVelocity.magnitude >= 2) {
    //        audio.volume = collision.relativeVelocity.magnitude / 20;
    //        Debug.Log("Relative Velocity: " + collision.relativeVelocity.magnitude);

    //        audio.Play();
    //    }
    //}
}
