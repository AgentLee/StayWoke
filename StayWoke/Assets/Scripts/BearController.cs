﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    // Player
    public GameObject player;
    public CapsuleCollider player_collider;
    public float collider_radius;
    // NPC
    public Transform head;
    public Animator anim;
    public float maxSightDistance;
    public float maxSightAngle;
    public Transform eyeTransform;

    public AudioSource audio;
    public AudioSource aux;

    bool isSleeping = false;
    bool seenPlayer = false;
    bool isWalking = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        collider_radius = player_collider.radius;

        audio = GetComponent<AudioSource>();

        ////aux = GetComponent<AudioSource>();
        //aux.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player.GetComponent<PlayerHMDController>().canHearBear && !audio.isPlaying)
        {
            Debug.Log("DETECTS PLAYER");
            aux.Play();
        }
        else
        {
            aux.Stop();
        }

        Vector3 direction = player.transform.position - this.transform.position;
        // Prevent NPC from tipping over when you get too close 
        direction.y = 0;

        //if(anim.GetBool("isSleeping"))
        //{
        //    StartCoroutine(goToSleep());
        //    return;
        //}

        float distanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position);
        //Debug.Log("Distance to player: " + distanceToPlayer);

        // Basically get FOV from NPC head
        float angle = Vector3.Angle(direction, head.up);
        //Debug.Log("FOV to player: " + angle);

        float directionLength = direction.magnitude;

        //Do ray casts to check if the player can actually be seen
        float maxSightDistance = 10.0f;

        RaycastHit hit;

        Vector3 playerPositionCenter    = player.transform.position;
        Vector3 playerPositionLeft      = player.transform.position + eyeTransform.right * collider_radius;
        Vector3 playerPositionRight     = player.transform.position - eyeTransform.right * collider_radius;

        Vector3 rayDirectionCenter = playerPositionCenter - eyeTransform.position;
        Vector3 rayDirectionLeft = playerPositionLeft - eyeTransform.position;
        Vector3 rayDirectionRight = playerPositionRight - eyeTransform.position;

        bool player_seen = Physics.Raycast(eyeTransform.position, rayDirectionCenter, out hit, maxSightDistance) ||
                           Physics.Raycast(eyeTransform.position, rayDirectionLeft, out hit, maxSightDistance) ||
                           Physics.Raycast(eyeTransform.position, rayDirectionRight, out hit, maxSightDistance);

        player_seen = hit.collider == player_collider;

        // If player is close to NPC and is either in FOV or has already seen the player, walk or attack
        if (player_seen && (angle < maxSightAngle || seenPlayer))
        {
            seenPlayer = true;

            // Have NPC rotate towards player
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            //Debug.Log("Direction Length: " + directionLength);

            if (directionLength < 5)
            {
                attackPlayer();
            }
            else
            {
                walkTowardsPlayer();
            }



            // Only starts to follow within a certain distance
            if (direction.magnitude > 3)
            {
                walkTowardsPlayer();
            }
            else
            {
                attackPlayer();
            }
        }
        else
        {
            seenPlayer = false;

            // This is a little buggy
            //if ((int)Random.Range(0.0f, 1.0f) == 0)
            //{
            //    print("Start sleep cycle");
            //    StartCoroutine(goToSleep());
            //    print("Finished sleep cycle");
            //}
            //else
            //{
            //    setIdle();
            //}

            setIdle();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (audio.isPlaying)
            {
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
    public void setIdle()
    {
        anim.SetBool("isIdle", true);
        anim.SetBool("prepSleep", false);
        anim.SetBool("isSleeping", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
    }

    IEnumerator goToSleep()
    {
        isSleeping = true;
        anim.SetBool("prepSleep", true);
        anim.SetBool("isSleeping", true);
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);

        yield return new WaitForSeconds(10);

        setIdle();
    }

    // TODO:
    // Add follow steering behavior *maybe*
    public void walkTowardsPlayer()
    {
        this.isWalking = true;
        this.transform.Translate(0, 0, 0.005f);
        anim.SetBool("isWalking", true);

        anim.SetBool("isIdle", false);
        anim.SetBool("isSleeping", false);
        anim.SetBool("isAttacking", false);
    }

    public void attackPlayer()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isSleeping", false);
        anim.SetBool("isAttacking", true);
        anim.SetBool("isWalking", false);
    }
}