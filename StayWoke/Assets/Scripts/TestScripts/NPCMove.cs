using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    [SerializeField]
    Transform _destination;

    NavMeshAgent _navMeshAgent;

    public List<GameObject> items;
    public GameObject item0;
    public GameObject item1;
    public GameObject item2;

    public GameObject player;
    public GameObject bearController;

    public bool canGetToObject;
    private Vector3 targetDestination;

	// Use this for initialization
	void Start () {
        //player = GetComponent<PlayerHMDController>().gameObject;

        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = 0.15f;
		_navMeshAgent.acceleration = 0.25f;
		_navMeshAgent.speed = 1.0f;

        item0 = Instantiate(Resources.Load("FixedJointGrab_Cube")) as GameObject;
        //item0.transform.position = new Vector3(98.04f, 10.075f, 90.78f);
        item0.transform.position = new Vector3(70, 10, 104);
        items.Add(item0);

        item1 = Instantiate(Resources.Load("FixedJointGrab_Cube")) as GameObject;
        item1.transform.position = new Vector3(80.0f, 10.075f, 90.78f);
        items.Add(item1);

        //item2 = Instantiate(Resources.Load("FixedJointGrab_Cube")) as GameObject;
        //item2.tag = "porridge";
        //item2.transform.position = new Vector3(70.0f, 10.0f, 103.0f);
        //items.Add(item2);

        _destination = item0.transform;

        canGetToObject = false;

        bearController = GetComponent<BearController>().gameObject;
    }
	
    void SetDestination()
    {
        if(_destination != null)
        {
            //this.GetComponent<BearController>().anim.SetBool("isIdle", false);
            //this.GetComponent<BearController>().anim.SetBool("isWalking", true);


            //bearController.GetComponent<BearController>().anim.SetBool("isWalking", false);
            //_navMeshAgent.stoppingDistance = 10.0f;
            _navMeshAgent.SetDestination(targetDestination);


            //Debug.Log(_navMeshAgent.nextPosition);
            //Debug.Log(_destination);
            //Debug.Log("-------------------------");

            Debug.Log(_navMeshAgent.pathStatus);

            //bearController.GetComponent<BearController>().setIdle();

            for (int i = 0; i < items.Count; i++)
            {
                item0.GetComponent<CollisionSound>().thrown = false;
                item1.GetComponent<CollisionSound>().thrown = false;
                //item2.GetComponent<CollisionSound>().thrown = false;
            }
        }
    }

	// Update is called once per frame
	void Update () {
        float dist = (targetDestination - bearController.GetComponent<BearController>().transform.position).magnitude;
        if(dist < 2.0f)
        {
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
                //Debug.Log("THROWN");
                targetDestination = items[i].transform.position;
                _destination.position = targetDestination;
                goToObject = true;
            }
        }

        if(goToObject)
        {
            // Calculate new distance to target
            float pathLength = CalculatePathLength(targetDestination);

            //Debug.Log(pathLength);

            // Check to see if the path length is within some radius
            if (pathLength < 9.0f && goToObject)
            {
                canGetToObject = true;
            }
            else
            {
                canGetToObject = false;
                item0.GetComponent<CollisionSound>().thrown = false;
                item1.GetComponent<CollisionSound>().thrown = false;
                //item2.GetComponent<CollisionSound>().thrown = false;
            }

            // Basically need to listen for the objects that fall and make sounds.
            if (canGetToObject)
            {
                //Debug.Log("CANCANCANCAN");

                bearController.GetComponent<BearController>().heardSomething = true;
                bearController.GetComponent<BearController>().walkTowardsObject();
                SetDestination();
                _navMeshAgent.isStopped = false;

                //if (bearController.GetComponent<BearController>().isSleeping)
                //{
                //}
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
