using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour {

	public AudioSource aux;

    public bool onStart;
    public bool thrown;

    public int x;

	// Use this for initialization
	void Start () {
        thrown = false;
        onStart = true;

		aux = GetComponent<AudioSource>();
	}

    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        // Eventually want to add tags for walls too
		if (collision.collider.tag == "Terrain") {
			//if(onStart)
			//{
			//    onStart = false;
			//    return;
			//}

			if (!aux.isPlaying) {
				aux.Play ();
			} else if (aux.isPlaying) {
				aux.Stop ();
			}

			thrown = true;
		} 
    }

    public void printSomething(int n)
    {
        x = n;
        Debug.Log("YASS");
    }
}
