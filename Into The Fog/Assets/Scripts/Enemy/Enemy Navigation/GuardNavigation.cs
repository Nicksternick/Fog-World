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

    // ===== | Temp Variables | =====
    private Vector3 jumpTarget;
    private float jumpTime;
    private float jumpHeight;

    [SerializeField] private bool isJumping;

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
                if (EnemyAgent.hasPath && !isJumping)
                {
                    EnemyAgent.ResetPath();
                    wanderTime = 0;
                }

                Controller.transform.LookAt(Controller.Target.transform, Vector3.up);
                Quaternion rotation = Controller.transform.rotation;
                rotation.x = 0;
                rotation.z = 0;
                Controller.transform.rotation = rotation;

                Jump();
            }
        }
    }

    private void Jump()
    {
        if (!isJumping)
        {
            Vector3 destination = RandomNavSphere(guardOrigin, guardRadius);
            //Vector3 destination = Controller.Target.position;

            EnemyAgent.SetDestination(destination);

            // Start the jump
            jumpTarget = EnemyAgent.destination; // Target the current destination
            jumpTime = Vector3.Distance(EnemyAgent.transform.position, destination);
            jumpTime -= EnemyAgent.stoppingDistance;
            jumpHeight = 2f; // Set desired height for the jump

            EnemyAgent.speed = 4 + jumpTime;

            // Disable obstacle avoidance while jumping
            //EnemyAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            isJumping = true;
        }

        if (Mathf.Clamp01(EnemyAgent.remainingDistance / jumpTime) >= 0.1 && isJumping)
        {
            // Reduce jumpTime based on deltaTime
            //jumpTime -= Time.deltaTime;

            // Calculate progress based on time: from 1 (start) to 0 (end)
            //float progress = Mathf.Clamp01(1 - (jumpTime / 1.0f));
            float progress = Mathf.Clamp01(EnemyAgent.remainingDistance / jumpTime);

            Debug.Log($"{EnemyAgent.isPathStale}");

            // Parabolic movement on Y-axis: y = -4h * (progress)(progress - 1)
            float yOffset = -4 * jumpHeight * progress * (progress - 1);
            

            // Update the agent's position to follow the curve
            Controller.transform.position = new Vector3(
                Controller.transform.position.x,
                Controller.transform.position.y + yOffset,
                Controller.transform.position.z
            );
        }
        else
        {
            // End the jump and re-enable obstacle avoidance
            //EnemyAgent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
            isJumping = false;
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
