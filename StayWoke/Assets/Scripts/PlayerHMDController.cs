using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State { notMoving, walking, running };

public class PlayerHMDController : MonoBehaviour
{
    public GameObject player;
    public GameObject bear;
    public Transform thisPlayer;
    public AudioSource aux;
    public Vector3 lastPos;
    public Vector3 lastLeftPos;
    public Vector3 lastRightPos;
    public GameObject VRTK;

    public GameObject left;
    public GameObject right;

    State state;
    public int movingState;

    public float speed;
    public bool canHearBear;

	// Use this for initialization
	void Start () {
        // For whatever reason, the layer gets changed to ignore ray casting.
        // We need this for debugging.
        aux = GetComponent<AudioSource>();
        player.layer = 0;
        movingState = 0;
        speed = 0;
    }

    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator delaySpeed()
    {
        //Debug.Log(Time.time);
        yield return new WaitForSecondsRealtime(6);
        //Debug.Log(Time.time);
        startSpeed = false;
        speed = (player.transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = player.transform.position;
    }

    public bool startSpeed = true;
    void FixedUpdate()
    {
        if(startSpeed)
        {
            StartCoroutine(delaySpeed());
        }
        else
        {
            //Debug.Log("SPEED: " + (player.transform.position - lastPos).magnitude);
            speed = (player.transform.position - lastPos).magnitude / Time.deltaTime;
            lastPos = player.transform.position;
        }

        //Debug.Log("Speed: " + speed);

        float distToBear = (bear.transform.position - player.transform.position).magnitude;
        //Debug.Log("distance to bear"+distToBear);

        // Power walk - 1.5
        // Running - 2
        // Walk - 1.3

        //float leftSpeed = (left.transform.position - lastLeftPos).magnitude;
        //float rightSpeed = (right.transform.position - lastRightPos).magnitude;
        //speed = (leftSpeed + rightSpeed) / 2;

        if (speed > 1.6f)
        {
            Debug.Log("LOUD " + speed);
			bear.GetComponent<BearController>().heardPlayer = true;
			bear.GetComponent<BearController> ().setIdle ();
        }
        else
        {
            Debug.Log("QUIET " + speed);
			bear.GetComponent<BearController>().heardPlayer = false;
        }

        //if (speed > 2.0)
        //{
        //    movingState = 3;
        //} else
        //{
        //    movingState = 0;
        //}

        //if (speed >= 1.75)
        //{
        //    // Running
        //    movingState = 3;
        //    Debug.Log("RUNNING: " + speed);
        //}
        //else if (speed >= 1.3 && speed <= 1.7499999999)
        //{
        //    movingState = 4;
        //    Debug.Log("POWER WALK: " + speed);
        //}
        //else if (speed >= 1.0)
        //{
        //    // Walking
        //    movingState = 2;
        //    Debug.Log("WALKING: " + speed);
        //}
        //else if (speed >= 0.05)
        //{
        //    // Tiptoe
        //    movingState = 1;
        //    Debug.Log("TIPTOE: " + speed);
        //}
        //else
        //{
        //    // In place
        //    movingState = 0;
        //    Debug.Log("IN PLACE: " + speed);
        //}

        //if (speed > 0.01)
        //{
        //    state = State.running;
        //    movingState = 2;

        //    if(distToBear < 6.0f)
        //    {
        //        bear.GetComponent<BearController>().heardSomething = true;
        //    }
        //}
        //else if(speed <= 0.0000)
        //{
        //    state = State.notMoving;
        //    movingState = 0;
        //}
        //else
        //{
        //    state = State.walking;
        //    movingState = 1;
        //}

        //Debug.Log(state);

        RaycastHit hit;

        //if(distToBear > 8.0)
        //{
        //    canHearBear = false;
        //}
        //else
        //{
        //    canHearBear = true;
        //}
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
