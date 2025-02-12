using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.FilePathAttribute;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveBehaviour : MonoBehaviour, IMovable
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private List<Transform> travelPoints = new List<Transform>();

    private int currentTravelPointIndex = 0;

    private Vector3 targetPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (travelPoints.Count > 0)
        {
            agent.SetDestination(travelPoints[0].position);
        }
        else
        {
            Debug.LogError("No travel points found!");
        }
    }

    public void Move()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (currentTravelPointIndex == travelPoints.Count - 1)
            {
                if (Vector3.Distance(transform.position, travelPoints[currentTravelPointIndex].position) >= agent.stoppingDistance)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                currentTravelPointIndex++;
                agent.SetDestination(travelPoints[currentTravelPointIndex].position);
            }
        }
    }
}
