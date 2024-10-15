using UnityEngine;
using UnityEngine.AI;

public class BasicNavigation : AbstractNavigation
{
    // ===== | Variables | =====
    private const float timer = 2;
    private const float chaseDistance = 50;
    private float wanderTime = timer;

    // ===== | Methods | =====
    public override void Move()
    {
        if (Controller.Target != null)
        {
            if (!Controller.PlayerInRange(chaseDistance))
            {
                Wander();
            }
            else if (Controller.PlayerInRange(chaseDistance))
            {
                if (!Controller.PlayerInSight(chaseDistance).Equals(default(RaycastHit)))
                {
                    wanderTime = timer;
                    EnemyAgent.SetDestination(Controller.Target.position);
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
            EnemyAgent.SetDestination(RandomNavSphere(transform.position, Random.Range(30, 40)));
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
}
