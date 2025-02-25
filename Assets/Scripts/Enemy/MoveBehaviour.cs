using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.FilePathAttribute;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveBehaviour : MonoBehaviour, IMovable
{
    public event Action<MoveBehaviour> OnDestinationReached;

    [SerializeField]
    public NavMeshAgent agent;

    private List<Transform> travelPoints = new List<Transform>();

    private int currentTravelPointIndex = 0;

    private Vector3 targetPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Move()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (currentTravelPointIndex == travelPoints.Count - 1)
            {
                if (Vector3.Distance(transform.position, travelPoints[currentTravelPointIndex].position) >= agent.stoppingDistance)
                {
                    OnDestinationReached.Invoke(this);
                }
            }
            else
            {
                currentTravelPointIndex++;
                agent.SetDestination(travelPoints[currentTravelPointIndex].position);
            }
        }
    }

    public void SetTravelPoints(List<Transform> travelPoints)
    {
        this.travelPoints = travelPoints;

        if (travelPoints.Count > 0)
        {
            agent.SetDestination(travelPoints[0].position);
        }
        else
        {
            Debug.LogError("No travel points found!");
        }
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    public void IsAgentStopped(bool state)
    {
        agent.isStopped = state;
    }
}
