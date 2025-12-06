using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGhost : MonoBehaviour
{
    [Header("AI Settings")]
    public Transform[] waypoints;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    public float detectionRange = 10f;
    public float attackRange = 2f;

    [Header("References")]
    public Transform player;

    private UnityEngine.AI.NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isChasing = false;
    private bool hasDetectedPlayer = false;
    private float attackCooldown = 0f;
    public float attackCooldownTime = 1f;
    private SafeRoom[] safeRooms;
    private bool hasShownDialogue = false;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        safeRooms = FindObjectsOfType<SafeRoom>();

        if (waypoints.Length > 0 && agent != null)
        {
            agent.speed = patrolSpeed;
            if (agent.isOnNavMesh)
            {
                GoToNextWaypoint();
            }
        }
    }

    void Update()
    {
        if (player == null || agent == null) return;

        if (!agent.isOnNavMesh || !agent.enabled)
        {
            Debug.LogWarning("Agent is not on NavMesh!");
            return;
        }

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        bool playerIsSafe = IsPlayerInSafeRoom();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !hasDetectedPlayer)
        {
            // Flag na once nakita ka, magiging hunter mode na siya
            hasDetectedPlayer = true;

            if (!hasShownDialogue)
            {
                hasShownDialogue = true;
                if (DialogueManager.Instance != null)
                {
                    string[] dialogue = new string[]
                    {
                        "Why are Ghosts Chasing Me now ?!",
                        "It's just one thing after another in this place"
                    };
                    DialogueManager.Instance.ShowMultipleLines(dialogue, 1f);
                }
            }
        }

        if (hasDetectedPlayer && !playerIsSafe)
        {
            // Aggro state: habol hanggang makapasok si player sa safe room
            ChasePlayer();
        }
        else if (!hasDetectedPlayer || playerIsSafe)
        {
            // Patrol fallback kapag hindi ka pa na-spot o nasa safe zone ka
            Patrol();

            if (playerIsSafe)
            {
                hasDetectedPlayer = false;
            }
        }

        if (distanceToPlayer <= attackRange && attackCooldown <= 0 && !playerIsSafe)
        {
            // Swing lang kapag nasa range at ready na ang cooldown
            AttackPlayer();
        }
    }

    bool IsPlayerInSafeRoom()
    {
        foreach (SafeRoom room in safeRooms)
        {
            if (room != null && room.IsPositionInside(player.position))
            {
                return true;
            }
        }
        return false;
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        if (!agent.isOnNavMesh || !agent.enabled)
            return;

        if (isChasing)
        {
            isChasing = false;
            agent.speed = patrolSpeed;
            GoToNextWaypoint();
        }

        if (!agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            GoToNextWaypoint();
        }
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0 || agent == null) return;

        if (!agent.isOnNavMesh || !agent.enabled)
            return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    void ChasePlayer()
    {
        if (agent == null || player == null) return;

        if (!agent.isOnNavMesh || !agent.enabled)
            return;

        if (!isChasing)
        {
            isChasing = true;
            agent.speed = chaseSpeed;
            Debug.Log("GHOST started chasing!");
        }

        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        if (IsPlayerInSafeRoom())
        {
            return;
        }

        attackCooldown = attackCooldownTime;

        Debug.Log("GHOST attacked the player!");

        HealthSystem playerHealth = player.GetComponent<HealthSystem>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}