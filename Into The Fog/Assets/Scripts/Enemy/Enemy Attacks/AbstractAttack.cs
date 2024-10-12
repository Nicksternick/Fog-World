using UnityEngine;

/// <summary>
/// Nicholas 10/11/2024
/// Abstract class defining if the player is in range for an attack
/// </summary>
public abstract class AbstractAttack : MonoBehaviour
{
    // ===== | Variables | =====
    [SerializeField] protected EnemyController enemy;
    protected Collider playerCollider;
    protected bool canAttack;

    // ===== | Methods | =====
    private void Awake()
    {
        canAttack = false;

        if (enemy == null)
        {
            enemy = GetComponent<EnemyController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canAttack = true;
            playerCollider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canAttack = false;
            playerCollider = null;
        }
    }
}
