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

    public bool canGetToObject;
    private Vector3 targetDestination;

	// Use this for initialization
	void Start () {
        //player = GetComponent<PlayerHMDController>().gameObject;

        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = 1.5f;
		_navMeshAgent.acceleration = 0.5f;
		_navMeshAgent.speed = 1.0f;

        item0 = Instantiate(Resources.Load("FixedJointGrab_Cube")) as GameObject;
        item0.transform.position = new Vector3(98.04f, 10.075f, 90.78f);
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
    }
	
    void SetDestination()
    {
        if(_destination != null)
        {
            //this.GetComponent<BearController>().anim.SetBool("isIdle", false);
            //this.GetComponent<BearController>().anim.SetBool("isWalking", true);

            _navMeshAgent.SetDestination(targetDestination);

            for(int i = 0; i < items.Count; i++)
            {
                item0.GetComponent<CollisionSound>().thrown = false;
                item1.GetComponent<CollisionSound>().thrown = false;
                //item2.GetComponent<CollisionSound>().thrown = false;
            }
        }
    }

	// Update is called once per frame
	void Update () {
        bool goToObject = false;

        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].GetComponent<CollisionSound>().thrown)
            {
                targetDestination = items[i].transform.position;
                goToObject = true;
            }
        }

        if(goToObject)
        {
            // Calculate new distance to target
            float pathLength = CalculatePathLength(targetDestination);

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
                SetDestination();
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
