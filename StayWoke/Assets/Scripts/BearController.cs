using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    // Player
    public GameObject player;
    public CapsuleCollider player_collider;
    public float collider_radius;
    public Transform playerHead;
    // NPC
    public Transform head;
    public Animator anim;
    public float maxSightAngle;
    public Transform eyeTransform;

    public bool pokedBear;
    public bool isSleeping     = false;
    public bool seenPlayer     = false;
    public bool isWalking      = false;
    public bool heardSomething = false;
    public bool facingPlayer = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        collider_radius = player_collider.radius;
        heardSomething = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction       = player.transform.position - this.transform.position;
        // Prevent NPC from tipping over when you get too close 
        direction.y             = 0;
        float directionLength   = direction.magnitude;
        float distanceToPlayer  = Vector3.Distance(player.transform.position, this.transform.position);
        // Basically get FOV from NPC head
        float angle             = Vector3.Angle(direction, head.up);

        float maxSightDistance = 10.0f;

        // Do ray casts to check if the player can actually be seen
        RaycastHit hit;
        Vector3 playerPositionCenter    = player.transform.position;
        Vector3 playerPositionLeft      = player.transform.position + eyeTransform.right * collider_radius;
        Vector3 playerPositionRight     = player.transform.position - eyeTransform.right * collider_radius;
        Vector3 rayDirectionCenter      = playerPositionCenter - eyeTransform.position;
        Vector3 rayDirectionLeft        = playerPositionLeft - eyeTransform.position;
        Vector3 rayDirectionRight       = playerPositionRight - eyeTransform.position;

        bool bearRaySeesPlayer = Physics.Raycast(eyeTransform.position, rayDirectionCenter, out hit, maxSightDistance) ||
                           Physics.Raycast(eyeTransform.position, rayDirectionLeft, out hit, maxSightDistance) ||
                           Physics.Raycast(eyeTransform.position, rayDirectionRight, out hit, maxSightDistance);
        
        // Check to see if there's an object in between the bear and the player from the previous raycasts
        bearRaySeesPlayer = hit.collider == player_collider;

        // Checks to see if the player is above the grass
        // Might have to make this a user defined parameter
        bool playerAboveGrass = playerHead.transform.position.y > 11f;

        // This should be the start state
        Coroutine sleepRoutine;
        if (anim.GetBool("isSleeping"))
        {
            sleepRoutine = StartCoroutine(goToSleep());
            return;
        }

        if(pokedBear)
        {
			//Debug.Log ("POKE");
			
            // Orient bear towards player
            Vector3 targetDir = player.transform.position - this.transform.position;
            float step =  0.45f * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            this.transform.rotation = Quaternion.LookRotation(newDir);

			// Attack player once the bear is facing the bear
            attackPlayer();
            return;
        }
        
        // If player is close to NPC and is either in FOV or has already seen the player, walk or attack
        if (bearRaySeesPlayer && (angle < maxSightAngle || seenPlayer) && (playerAboveGrass))
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

            setIdle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // For whatever reason, I can't call attackPlayer() here.
        if(other.tag == "Left" || other.tag == "Right")
        {
            //Debug.Log(other.tag);
            pokedBear = true;
        }
    }
    public void setIdle()
    {
        isSleeping = false;
        anim.SetBool("isIdle", true);
        anim.SetBool("isSleeping", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
    }

    IEnumerator goToSleep()
    {
        isSleeping = true;
        anim.SetBool("isSleeping", true);
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);

        //Debug.Log("SLEEPING");
        yield return new WaitUntil(() => heardSomething);

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

    public void walkTowardsObject()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);
        anim.SetBool("isSleeping", false);
        anim.SetBool("isAttacking", false);
    }

    public void attackPlayer()
    {
        anim.SetBool("isAttacking", true);
		int attackType = (int)Random.Range (1, 5);
		if (attackType == 5) {
			attackType = 4;
		}
		Debug.Log (attackType);
		anim.SetInteger ("attackType", attackType);//(int)Random.Range(0, 3));
        anim.SetBool("isIdle", false);
        anim.SetBool("isSleeping", false);
        anim.SetBool("isWalking", false);
    }
}