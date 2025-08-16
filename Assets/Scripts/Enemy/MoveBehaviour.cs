using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls enemy movement using a NavMeshAgent and manages travel points.
/// </summary>
/// <remarks>
/// - Implements `IMovable` to provide movement functionality.
/// - Moves the enemy along predefined travel points.
/// - Invokes `OnDestinationReached` when the final destination is reached.
/// - Supports speed adjustment and the ability to stop the agent.
/// - Ensures `NavMeshAgent` is properly assigned and utilized.
/// - Manages the upgrade of a tower, including cost, range, and attack cooldown.
/// - Instantiates an upgraded version of the tower and applies the upgrade.
/// - Triggers an event when the tower is successfully upgraded.
/// </remarks>

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

    /// <summary>
    /// Moves the enemy along travel points and invokes OnDestinationReached when the final point is reached.
    /// </summary>
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

    /// <summary>
    /// Assigns a list of travel points and sets the first destination for the NavMeshAgent.
    /// </summary>
    /// <param name="travelPoints"></param>
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

    /// <summary>
    /// Updates the NavMeshAgent's movement speed.
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    /// <summary>
    /// Starts or stops the NavMeshAgent's movement.
    /// </summary>
    /// <param name="state"></param>
    public void IsAgentStopped(bool state)
    {
        agent.isStopped = state;
    }
}
