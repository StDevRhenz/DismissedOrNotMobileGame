using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform target;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float stopDistance = 1.5f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > stopDistance)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0;
            controller.Move(direction * moveSpeed * Time.deltaTime);

            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
