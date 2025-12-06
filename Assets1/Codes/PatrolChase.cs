using UnityEngine;
using UnityEngine.AI;

public class PatrolChase : MonoBehaviour
{
    public Transform[] patrolPoints;

    public Transform player;

    public float detectionRange = 5f;

    private NavMeshAgent agent;

    private int currentPointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Start walking to the first patrol point
        GoToNextPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            agent.SetDestination(player.position);
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            return;
        }

        Transform targetPoint = patrolPoints[currentPointIndex];

        agent.SetDestination(targetPoint.position);

        currentPointIndex = currentPointIndex + 1;

        if (currentPointIndex >= patrolPoints.Length)
        {
            currentPointIndex = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
