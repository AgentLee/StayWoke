using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHMDController : MonoBehaviour {

    public GameObject player;
    public Transform bear;
    public Transform thisPlayer;
    public AudioSource aux;

    public bool canHearBear;

	// Use this for initialization
	void Start () {
        // For whatever reason, the layer gets changed to ignore ray casting.
        // We need this for debugging.
        aux = GetComponent<AudioSource>();
        player.layer = 0;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void FixedUpdate()
    {
        
        RaycastHit hit;

        float distToBear = (bear.position - player.transform.position).magnitude;
        if(distToBear > 8.0)
        {
            canHearBear = false;
        }
        else
        {
            canHearBear = true;
        }
        //Debug.Log(distToBear);

        //canHearBear = Physics.Raycast(player.transform.position, bear.position - player.transform.position, out hit, 8.0f);

        if(!aux.isPlaying && distToBear < 5.0f)
        {
            aux.Play();
        } else if(aux.isPlaying && distToBear >= 5.0f)
        {
            aux.Stop();
        }


        //if (canHearBear && !aux.isPlaying)
        //{
        //    Debug.Log("PLAY BEAR SOUND");
        //    aux.Play();
        //    Debug.DrawRay(player.transform.position, bear.position - player.transform.position, Color.red, 2.0f);
        //}
        //else
        //{
        //    Debug.Log("DON'T PLAY BEAR SOUND");
        //    aux.Stop();
        //    Debug.DrawRay(player.transform.position, bear.position - player.transform.position, Color.green, 2.0f);
        //}

    }
}
