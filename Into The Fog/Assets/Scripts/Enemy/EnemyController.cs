using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //Variables
    [SerializeField] public Transform target;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float health;
    [SerializeField] public HealthBar healthBar;

    private const float Timer = 2;
    private const float ChaseDistance = 50;
    private float wanderTime = Timer;

    // ===== | Properties | =====
    public float Health
    {
        get { return health; }
    }
   
    /// <summary>
    /// AJ Wagner - 10/2/2024
    /// Added basic support for the health bar
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth(health);
        agent.speed += 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color (1, 1, 1, 0.1f);
        Gizmos.DrawSphere(transform.position, ChaseDistance);
    }

    /// <summary>
    /// Ruby 9/20/2024
    /// Makes enemy follow the player
    /// </summary>
    void Update()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > ChaseDistance)
            {
                Wander();
            }

            if (Vector3.Distance(transform.position, target.position) < ChaseDistance)
            {
                Vector3 direction = target.position - transform.position;
                direction = direction.normalized;

                float dist = Vector3.Distance(transform.position, target.position);

                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, dist))
                {
                    Debug.DrawLine(transform.position, hit.point);

                    if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Ball"))
                    {
                        wanderTime = Timer;
                        agent.SetDestination(target.position);
                    }
                    else
                    {
                        Wander();
                    }
                }
            }
            
            //Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
            //rb.MovePosition(pos);
            //transform.LookAt(target);
        }
    }
    
    public void Wander()
    {
        if (wanderTime >= Timer)
        {
            agent.SetDestination(RandomNavSphere(transform.position, Random.Range(30, 40)));
            wanderTime = 0;
        }

        if (wanderTime < Timer)
        {
            wanderTime += Time.deltaTime;
        }
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask = -1)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;
        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    /// <summary>
    /// Nicholas 10/3/2024
    /// Reduces the enemy health value
    /// </summary>
    /// <param name="amount"></param>
    public void takeDamage(float amount)
    {
        health -= amount;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
