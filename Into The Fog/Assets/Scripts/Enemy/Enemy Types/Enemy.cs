using System.IO;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Nicholas 10/10/2024
/// Abstracted enemy class, that contains
/// all code that all enemies share
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    // ===== | Variables | =====
    [SerializeField] protected Transform target;
    [SerializeField] protected float health;
    [SerializeField] private float healthModifier;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected float takeDamageCooldown;
    [SerializeField] public bool isImportant;
    protected bool isDead = false;
    protected UnityEvent onDeath;
    protected float lastDamaged;

    // ===== | Properties | =====
    /// <summary>The current health of the enemy</summary>
    public float Health { get { return health; } }
    /// <summary>Checks whether the enemy is alive or death</summary>
    public bool IsDead { get { return isDead; } }
    /// <summary>Event that is called when the enemy dies</summary>
    public UnityEvent OnDeath { get { return onDeath; } }
    public Transform Target { get { return target; } set { target = value; } }

    // ===== | Methods | =====
    private void Awake()
    {
        
    }

    private void Start()
    {

    }

    protected virtual void EnemyStart()
    {
        onDeath = new UnityEvent();
        lastDamaged = 0;

        health += healthModifier * GameManager.Instance.CurrentLevel;

        if (healthBar != null)
            healthBar.SetMaxHealth(health);
        else
            Debug.LogWarning($"HealthBar for enemy {gameObject.GetInstanceID()} was not set");
    }

    /// <summary>
    /// Nicholas 10/10/2024
    /// Reduces the enemy health value
    /// </summary>
    public virtual void TakeDamage(float damage)
    {
        // Check damage cooldown
        if (Time.time >= lastDamaged + takeDamageCooldown)
        {
            // Reduce the enemy health, and update the health bar
            health -= damage;
            if (healthBar != null)
                healthBar.SetHealth(health);

            // If the enemies health is reduced to zero, call the onDeath functions
            if (health <= 0)
            {
                isDead = true;
                //onDeath.Invoke();

                Destroy(gameObject);
            }
        }
    }

    public bool PlayerInRange(float range)
    {
        bool inRange = Vector3.Distance(transform.position, 
            target.transform.position) < range;
        return inRange;
    }

    public RaycastHit PlayerInSight(float range)
    {
        if (PlayerInRange(range))
        {
            Vector3 direction = target.position - transform.position;
            direction = direction.normalized;

            float dist = Vector3.Distance(transform.position, target.position);

            int layerMask = (1 << 3) | (1 << 6);

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, dist, layerMask))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                    return hit;
            }
        }

        return new RaycastHit();
    }
}
