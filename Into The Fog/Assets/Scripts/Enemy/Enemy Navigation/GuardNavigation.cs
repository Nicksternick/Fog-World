using UnityEngine;
using UnityEngine.AI;

public class GuardNavigation : AbstractNavigation
{
    // ===== | Variables | =====
    private Vector3 guardOrigin;
    private float guardRadius;

    private const float timer = 2;
    private const float chaseDistance = 50;
    private float wanderTime = timer;

    // ===== | Methods | =====
    private void Start()
    {
        guardOrigin = transform.position;
        guardRadius = 10;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(guardOrigin, guardRadius);
    }

    public override void Move()
    {
        if (Controller.Target != null)
        {
            float distanceToPlayer = Vector3.Distance(Controller.Target.transform.position, guardOrigin);

            if (distanceToPlayer > guardRadius)
            {
                Wander();
            }
            else
            {
                EnemyAgent.ResetPath();
                wanderTime = 0;

                Controller.transform.LookAt(Controller.Target.transform, Vector3.up);
                Quaternion rotation = Controller.transform.rotation;
                rotation.x = 0;
                rotation.z = 0;
                Controller.transform.rotation = rotation;
            }
        }
    }

    private void Wander()
    {
        if (wanderTime >= timer)
        {
            EnemyAgent.SetDestination(RandomNavSphere(guardOrigin, guardRadius));
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
