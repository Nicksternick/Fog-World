using UnityEngine;

/// <summary>
/// Nicholas 9/30/2024
/// Deals damage to the player when 
/// they enter the attacks collider
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    // ===== | Variables | =====
    [SerializeField] private float damage;
    [SerializeField] private float damageCooldown;
    [SerializeField] private bool canAttack;

    private const float MaxTimer = 5;

    // ===== | Methods | =====
    // Start is called before the first frame update
    void Start()
    {
        damageCooldown = MaxTimer;
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canAttack)
        {
            damageCooldown -= Time.deltaTime;
            Debug.Log($"{gameObject.name}: {damageCooldown}");

            if (damageCooldown < 0)
            {
                damageCooldown = MaxTimer;
                canAttack = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = new Color(1, 0, 0, 0.5f);
        //Gizmos.DrawSphere(transform.position, 1.2f);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawRay(transform.position, transform.forward);
    }

    private void OnTriggerStay(Collider other)
    {
        // Check to see if the enemy collided with the enemy.
        if (other.gameObject.CompareTag("Player") && canAttack)
        {
            Debug.Log($"{gameObject.name}: Attack Hit!");
            canAttack = false;
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            Rigidbody rb = player.GetComponent<Rigidbody>();

            rb.velocity = Vector3.zero;
            rb.AddForce(CalculatePushForce(15, player.transform.position - transform.position), ForceMode.Impulse);

            // WIP ADD DAMAGING METHOD FOR PLAYER HERE WHEN IMPLEMENTED
        }
    }

    /// <summary>
    /// Nicholas 9/30/2024
    /// Calculates the force to apply to the player
    /// </summary>
    /// <param name="pushForce"></param>
    /// <returns></returns>
    private Vector3 CalculatePushForce(float pushForce, Vector3 direction)
    {
        direction = direction.normalized;
        return new Vector3(direction.x, direction.y) * pushForce;
    }
}
