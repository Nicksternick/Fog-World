using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoxPusher : MonoBehaviour
{
    // The BoxCollider we are checking for
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private float pushForce = 10f;
    private Rigidbody playerRb;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Move the player back onto the NavMesh
            Vector3 closestPoint = NavMesh.SamplePosition(collision.gameObject.transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas) ? hit.position : collision.gameObject.transform.position;
            collision.gameObject.transform.position = closestPoint; // Teleport the player back to the nearest NavMesh point
        }
    }
}
