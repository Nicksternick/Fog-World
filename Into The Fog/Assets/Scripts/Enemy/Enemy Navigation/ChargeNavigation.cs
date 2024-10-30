using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargeNavigation : AbstractNavigation
{
    // ===== | Variables | =====
    private const float timer = 2;
    private const float chaseDistance = 50;
    private float wanderTime = timer;


    // ===== | Charge Variables | =====
    private float sightConeRadius = 60;
    private float baseSpeed = 4;
    private float chargeWaitTime;

    private EnemyState enemyState;

    private enum EnemyState
    {
        Wandering,
        Charge,
        GraciePeriod,
        Cooldown,
    }

    // ===== | Methods | =====
    public override void Move()
    {
        if (Controller.Target != null)
        {
            switch(enemyState)
            {
                case EnemyState.Wandering:
                    Wander();
                    CheckForPlayer();
                    break;
                case EnemyState.Charge:
                    if (EnemyAgent.remainingDistance < 0.1)
                    {
                        chargeWaitTime = 0;
                        enemyState = EnemyState.Cooldown;
                    }
                    break;
                case EnemyState.GraciePeriod:
                    LookTowardsTarget();

                    if (Controller.PlayerInSight(float.PositiveInfinity).Equals(default(RaycastHit)))
                    {
                        chargeWaitTime = 0;
                        enemyState = EnemyState.Wandering;
                    }

                    if (chargeWaitTime < 1f)
                    {
                        chargeWaitTime += Time.deltaTime;
                    }
                    else
                    {
                        int layerMask = (1 << 6);

                        if (Physics.Raycast(transform.position, Controller.transform.forward,
                        out RaycastHit hit, float.PositiveInfinity, layerMask))
                        {
                            if (hit.collider.gameObject.layer == 6)
                                EnemyAgent.SetDestination(hit.point);
                        }

                        EnemyAgent.speed = 30;
                        enemyState = EnemyState.Charge;
                    }
                    break;
                case EnemyState.Cooldown:
                    if (chargeWaitTime < 0.5f)
                    {
                        chargeWaitTime += Time.deltaTime;
                    }
                    else
                    {
                        EnemyAgent.speed = baseSpeed;
                        chargeWaitTime = 0;
                        enemyState = EnemyState.Wandering;
                    }
                    break;
            }
        }
    }

    private void LookTowardsTarget()
    {
        Controller.transform.LookAt(Controller.Target.transform, Vector3.up);
        Quaternion rotation = Controller.transform.rotation;
        rotation.x = 0;
        rotation.z = 0;
        Controller.transform.rotation = rotation;
    }

    private void CheckForPlayer()
    {
        RaycastHit hit = Controller.PlayerInSight(float.PositiveInfinity);
        if (!hit.Equals(default(RaycastHit)))
        {
            Vector3 playerDirection = (hit.point - EnemyAgent.transform.position).normalized;
            if (Vector3.Angle(EnemyAgent.transform.forward, playerDirection) <= sightConeRadius)
            {
                enemyState = EnemyState.GraciePeriod;
                EnemyAgent.ResetPath();
                wanderTime = timer;
            }
        }
    }

    private void Wander()
    {
        if (wanderTime >= timer)
        {
            EnemyAgent.SetDestination(RandomNavSphere(transform.position, 30));
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
