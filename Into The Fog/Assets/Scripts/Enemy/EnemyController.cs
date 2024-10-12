using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Enemy
{
    //Variables
    [SerializeField] private NavMeshAgent agent;

    private const float timer = 2;
    private const float chaseDistance = 50;
    private float wanderTime = timer;

    // ===== | Methods | =====

    /// <summary>
    /// Ruby 9/20/2024
    /// Makes enemy follow the player
    /// </summary>
    void Update()
    {
        if (target != null)
        {
            if (!PlayerInRange(chaseDistance))
            {
                Wander();
            }
            else if (PlayerInRange(chaseDistance))
            {
                if (!PlayerInSight(chaseDistance).Equals(default(RaycastHit)))
                {
                    wanderTime = timer;
                    agent.SetDestination(target.position);
                }
                else
                {
                    Wander();
                }
            }
        }
    }
    
    private void Wander()
    {
        if (wanderTime >= timer)
        {
            agent.SetDestination(RandomNavSphere(transform.position, Random.Range(30, 40)));
            wanderTime = 0;
        }

        if (wanderTime < timer)
        {
            wanderTime += Time.deltaTime;
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask = -1)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;
        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
    public new RaycastHit PlayerInSight(float range)
    {
        return base.PlayerInSight(range);
    }

    public new bool PlayerInRange(float range)
    {
        return base.PlayerInRange(range);
    }
}
