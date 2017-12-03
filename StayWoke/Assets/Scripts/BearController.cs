using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour {

    // Player
    public Transform player;
    // NPC
    public Transform head;          
    static Animator anim;

    bool isSleeping = false;
    bool seenPlayer = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = player.position - this.transform.position;
        // Prevent NPC from tipping over when you get too close 
        direction.y = 0;

        // Basically get FOV from NPC head
        float angle = Vector3.Angle(direction, head.up);

        // If player is close to NPC and is either in FOV or has already seen the player, walk or attack
        if (Vector3.Distance(player.position, this.transform.position) < 10 && (angle < 30 || seenPlayer)) {
            seenPlayer = true;

            // Have NPC rotate towards player
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            // Only starts to follow within a certain distance
            if(direction.magnitude > 5) {
                walkTowardsPlayer();
            }
            else {
                attackPlayer();
            }
        }
        else {
            seenPlayer = false;

            // This is a little buggy
            if ((int)Random.Range(0.0f, 1.0f) == 0)
            {
                print("Start sleep cycle");
                StartCoroutine(goToSleep());
                print("Finished sleep cycle");
            }
            else
            {
                setIdle();
            }
        }
    }

    void setIdle()
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
    void walkTowardsPlayer()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isSleeping", false);
        this.transform.Translate(0, 0, 0.05f);
        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);
    }

    void attackPlayer()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isSleeping", false);
        anim.SetBool("isAttacking", true);
        anim.SetBool("isWalking", false);
    }
}
