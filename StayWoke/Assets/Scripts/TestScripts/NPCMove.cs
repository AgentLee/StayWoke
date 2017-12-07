using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour {

    [SerializeField]
    Transform _destination;

    NavMeshAgent _navMeshAgent;

	// Use this for initialization
	void Start () {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(_navMeshAgent == null)
        {
            Debug.Log("ERROR");
        }
        else
        {
            SetDestination();
        }
	}
	
    void SetDestination()
    {
        if(_destination != null)
        {
            Vector3 targetDestination = _destination.transform.position;
            _navMeshAgent.SetDestination(targetDestination);
        }
    }

	// Update is called once per frame
	void Update () {
        if (_navMeshAgent == null)
        {
            Debug.Log("ERROR");
        }
        else
        {
            SetDestination();
        }
    }
}
