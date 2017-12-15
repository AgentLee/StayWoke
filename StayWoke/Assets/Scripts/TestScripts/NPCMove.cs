using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    [SerializeField]
    Transform _destination;

    public NavMeshAgent _navMeshAgent;

    public List<GameObject> items;
    public GameObject item0;
    public GameObject item1;
    public GameObject item2;

    public GameObject player;
    public GameObject bearController;

    public bool canGetToObject;
    public Vector3 targetDestination;
	public bool goAfterObject;

	// Use this for initialization
	void Start () {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = 3.0f;
		_navMeshAgent.acceleration = 0.25f;
		_navMeshAgent.speed = 1.0f;

        item0 = Instantiate(Resources.Load("FixedJointGrab_Cube")) as GameObject;
        item0.transform.position = new Vector3(70, 10, 104);
        items.Add(item0);

        item1 = Instantiate(Resources.Load("FixedJointGrab_Cube")) as GameObject;
        item1.transform.position = new Vector3(80.0f, 10.075f, 90.78f);
        items.Add(item1);

        //item2 = Instantiate(Resources.Load("FixedJointGrab_Cube")) as GameObject;
        //item2.tag = "porridge";
        //item2.transform.position = new Vector3(70.0f, 10.0f, 103.0f);
        //items.Add(item2);

		targetDestination = item0.transform.position;

        canGetToObject = false;

        bearController = GetComponent<BearController>().gameObject;

		goAfterObject = false;
    }
	
    public void SetDestination()
    {
		if(targetDestination != null)
        {
			// Can't call walkTowardsObject() at the end of the coroutine because
			// navMeshAgent.SetDestination() acts like its own coroutine and won't
			// do anything until after the NPC reaches the destination.
			bearController.GetComponent<BearController> ().walkTowardsObject ();

            _navMeshAgent.SetDestination(targetDestination);

			// Reset flags
            for (int i = 0; i < items.Count; i++)
            {
				items [i].GetComponent<CollisionSound> ().thrown = false;
            }
        }
    }

	// Update is called once per frame
	void Update () {
        float dist = (targetDestination - bearController.GetComponent<BearController>().transform.position).magnitude;
        if(dist < 2.0f)
        {
			if (bearController.GetComponent<BearController> ().heardSomething) {
				bearController.GetComponent<BearController> ().heardSomething = false;
			}
            //Debug.Log(dist);
            bearController.GetComponent<BearController>().setIdle();
            _navMeshAgent.isStopped = true;
            //return;
        }

        bool goToObject = false;

        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].GetComponent<CollisionSound>().thrown)
            {
                targetDestination = items[i].transform.position;
                //_destination.position = targetDestination;
                goToObject = true;
            }
        }

        if(goToObject)
        {
            // Calculate new distance to target
            float pathLength = CalculatePathLength(targetDestination);

            // Check to see if the path length is within some radius
			// Basically need to listen for the objects that fall and make sounds.
            if (pathLength < 9.0f && goToObject)
            {
				goAfterObject = true;
				bearController.GetComponent<BearController> ().heardSomething = true;
				bearController.GetComponent<BearController> ().anim.SetBool ("isWalking", true);
				bearController.GetComponent<BearController> ().anim.SetBool ("isSleeping", false);
				bearController.GetComponent<BearController> ().anim.SetBool ("isIdle", false);
				bearController.GetComponent<BearController> ().anim.SetBool ("isAttacking", false);

				// Move NPC 
				SetDestination ();
				// Not sure why I still have this here D:
				_navMeshAgent.isStopped = false;

				//bearController.GetComponent<BearController> ().setIdle ();
            }
            else
            {
				goAfterObject = false;
				bearController.GetComponent<BearController> ().heardSomething = false;
				for (int i = 0; i < items.Count; i++)
				{
					items [i].GetComponent<CollisionSound> ().thrown = false;
				}
            }
        }
    }

    float CalculatePathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.CalculatePath(targetPosition, path);
        }

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
}
