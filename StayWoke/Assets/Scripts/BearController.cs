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

	public GameObject npcMover;

	// Wander Points
	public GameObject[] wanderPoints;
	public int wanderPointID;

    public bool pokedBear;
    public bool isSleeping     = false;
    public bool seenPlayer     = false;
	public bool isWalking      = false;
    public bool isRunning    = false;
    public bool heardSomething = false;
	public bool heardPlayer 	= false;
    public bool facingPlayer = false;

	public float idleTimer;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        collider_radius = player_collider.radius;
        heardSomething = false;
		wanderPoints = GameObject.FindGameObjectsWithTag ("WanderPoints");
		wanderPointID = Random.Range (0, wanderPoints.Length - 1);
		npcMover = GetComponent<NPCMove> ().gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		/*
		if(anim.GetBool("isIdle")) {
			idleTimer += Time.deltaTime;
		}

		Debug.Log (idleTimer);
		if(idleTimer > 5.0f) {
			wander ();
			return;
		}
*/
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

		/*
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
        */


		if (heardPlayer && !player.GetComponent<PlayerHMDController> ().hasPooridge) {
			setIdle();
		}

        
		if (!player.GetComponent<PlayerHMDController> ().hasPooridge) {
			// If player is close to NPC and is either in FOV or has already seen the player, walk or attack
			if (bearRaySeesPlayer && (angle < maxSightAngle || seenPlayer) && (playerAboveGrass) && !npcMover.GetComponent<NPCMove>().goAfterObject)
			{
				seenPlayer = true;

				heardSomething = false;

				// Have NPC rotate towards player
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

				//Debug.Log("Direction Length: " + directionLength);

				if (directionLength < 3)
				{
					attackPlayer();
				}
				else
				{
					walkTowardsPlayer ();
				}

				// Only starts to follow within a certain distance
				if (direction.magnitude > 3)
				{
					walkTowardsPlayer ();
				}
				else
				{
					attackPlayer();
				}
			}
			else if(!bearRaySeesPlayer && angle < maxSightAngle)
			{
				seenPlayer = false;
				setIdle();
			}
			// Has pooridge
		} 
		else {
			if (bearRaySeesPlayer && (angle < maxSightAngle || seenPlayer) && (playerAboveGrass))
			{
				seenPlayer = true;

				heardSomething = false;

				// Have NPC rotate towards player
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

				//Debug.Log("Direction Length: " + directionLength);

				if (directionLength < 3)
				{
					attackPlayer();
				}
				else
				{
					runTowardsPlayer ();
				}

				// Only starts to follow within a certain distance
				if (direction.magnitude > 3)
				{
					runTowardsPlayer ();
				}
				else
				{
					attackPlayer();
				}
			}
			else if(!bearRaySeesPlayer && angle < maxSightAngle)
			{
				Debug.Log ("CANT SEE");
				player.GetComponent<PlayerHMDController> ().hasPooridge = false;
				seenPlayer = false;
				setIdle();
			}
		}


        
    }

	public void wander()
	{
		if (Vector3.Distance (this.transform.position, wanderPoints [wanderPointID].transform.position) >= 2.0f) {
			//npcMover.GetComponent<NPCMove>().targetDestination = wanderPoints[wanderPointID].transform.position;
			//npcMover.GetComponent<NPCMove> ().SetDestination ();


			//anim.SetBool("isWalking", true);
			//anim.SetBool("isIdle", false);
			//anim.SetBool("isSleeping", false);
			//anim.SetBool("isAttacking", false);

			walkTowardsObject ();

			npcMover.GetComponent<NPCMove> ()._navMeshAgent.SetDestination (wanderPoints [wanderPointID].transform.position);
			
		}
		else if(Vector3.Distance(this.transform.position, wanderPoints[wanderPointID].transform.position) < 2.0f) {
			setIdle ();

			wanderPointID = Random.Range (0, wanderPoints.Length - 1);
		}
	}

	public void runTowardsPlayer()
	{
		this.isRunning = true;
		this.transform.Translate(0, 0, 0.0075f);
		anim.SetBool("isRunning", true);
		anim.SetBool("isWalking", false);
		anim.SetBool("isIdle", false);
		anim.SetBool("isSleeping", false);
		anim.SetBool("isAttacking", false);
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
		idleTimer = 0.0f;
        isSleeping = false;
        anim.SetBool("isIdle", true);
        anim.SetBool("isSleeping", false);
        anim.SetBool("isWalking", false);
		anim.SetBool("isAttacking", false);
        anim.SetBool("isRunning", false);
    }

    IEnumerator goToSleep()
    {
        isSleeping = true;
        anim.SetBool("isSleeping", true);
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
		anim.SetBool("isRunning", false);

        //Debug.Log("SLEEPING");
        yield return new WaitUntil(() => heardSomething);

		//setIdle ();
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
		anim.SetBool("isRunning", false);
    }

    public void walkTowardsObject()
    {
		//Debug.Log ("FROM NPCMOVE");
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);
        anim.SetBool("isSleeping", false);
		anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
    }

    public void attackPlayer()
    {
        anim.SetBool("isAttacking", true);
		int attackType = (int)Random.Range (1, 3);
		if (attackType == 3) {
			attackType = 2;
		}
		//Debug.Log (attackType);
		anim.SetInteger ("attackType", attackType);//(int)Random.Range(0, 3));
        anim.SetBool("isIdle", false);
        anim.SetBool("isSleeping", false);
        anim.SetBool("isWalking", false);
		anim.SetBool("isRunning", false);
    }
}