using UnityEngine;
using UnityEngine.AI;

public class NPCNavFollow : MonoBehaviour
{
    public Transform target;

    [Header("Speed Settings")]
    public float normalSpeed = 3.5f;
    public float chaseSpeed = 8f;

    private NavMeshAgent agent;
    private bool playerDetected = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            agent.speed = chaseSpeed; // bibilis
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            agent.speed = normalSpeed; // balik sa normal
        }
    }
}
