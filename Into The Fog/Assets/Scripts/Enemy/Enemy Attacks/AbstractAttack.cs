using UnityEngine;

/// <summary>
/// Nicholas 10/11/2024
/// Abstract class defining if the player is in range for an attack
/// </summary>
public abstract class AbstractAttack : MonoBehaviour
{
    // ===== | Variables | =====
    protected Collider playerCollider;
    protected bool playerInAttackRange;

    // ===== | Methods | =====
    private void Awake()
    {
        playerInAttackRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInAttackRange = true;
            playerCollider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInAttackRange = false;
            playerCollider = null;
        }
    }

    public abstract void Attack();
}
