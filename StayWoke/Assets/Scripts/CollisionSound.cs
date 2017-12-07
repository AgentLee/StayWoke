using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour {

    public AudioClip audio;

    public bool onStart;
    public bool thrown;

    public int x;

	// Use this for initialization
	void Start () {
        thrown = false;
        onStart = true;
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = audio;
	}

    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        // Eventually want to add tags for walls too
        if (collision.collider.tag == "Terrain")
        {
            if(onStart)
            {
                onStart = false;
                return;
            }

            Debug.Log(collision.collider.tag);
            this.GetComponent<AudioSource>().Play();
            thrown = true;
        }
    }

    public void printSomething(int n)
    {
        x = n;
        Debug.Log("YASS");
    }
}
