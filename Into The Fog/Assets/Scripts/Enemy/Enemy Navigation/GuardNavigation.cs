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

    private float jumpCooldownTime;
    private float jumpCooldown;

    private EnemySate enemyState;

    /// <summary>
    /// The different states that the enemy can be in
    /// </summary>
    private enum EnemySate
    {
        /// <summary>
        /// Enemy wanders around a nearby point
        /// </summary>
        Wandering,
        /// <summary>
        /// Selection the position to jump
        /// </summary>
        PickingJumpTarget,
        /// <summary>
        /// Jumping to the targed position
        /// </summary>
        Jumping,
        /// <summary>
        /// Cooldown after jump
        /// </summary>
        Cooldown,
    }

    // ===== | Methods | =====
    private void Start()
    {
        guardOrigin = transform.position;
        guardRadius = 10;

        enemyState = EnemySate.Wandering;
    }

    public override void Move()
    {
        if (Controller.Target != null)
        {
            float distanceToPlayer = Vector3.Distance(Controller.Target.transform.position, guardOrigin);

            switch (enemyState)
            {
                case EnemySate.Wandering:
                    if (distanceToPlayer > guardRadius)
                    {
                        Wander();
                    }
                    else
                    {
                        EnemyAgent.ResetPath();
                        wanderTime = timer;
                        enemyState = EnemySate.PickingJumpTarget;
                    }
                    break;
                case EnemySate.PickingJumpTarget:
                    LookTowardsTarget();

                    Vector3 destination = Controller.Target.position;

                    EnemyAgent.SetDestination(destination);

                    // Start the jump
                    jumpTarget = EnemyAgent.destination; // Target the current destination

                    jumpTime = Vector3.Distance(EnemyAgent.transform.position, destination);
                    jumpTime -= EnemyAgent.stoppingDistance;
                    jumpHeight = 3f; // Set desired height for the jump

                    enemyState = EnemySate.Jumping;
                    break;
                case EnemySate.Jumping:
                    Jump();
                    break;
                case EnemySate.Cooldown:
                    if (jumpCooldown < jumpCooldownTime)
                    {
                        jumpCooldown += Time.deltaTime;
                        return;
                    }

                    jumpCooldownTime = Random.Range(0.15f, 0.3f);

                    jumpCooldown = 0;
                    Cooldown();
                    break;
            }
        }
    }

    /// <summary>
    /// Enemy cooldown logic, pick a new position to jump 
    /// too, unless it cannot see the player anymore
    /// </summary>
    private void Cooldown()
    {
        // If the player is not within sight of the enemy
        if (Controller.PlayerInSight(chaseDistance).Equals(default(RaycastHit)))
        {
            // Set the enemies state to wandering
            enemyState = EnemySate.Wandering;
            return;
        }

        // If the enemy is very close to the player
        if (Vector3.Distance(Controller.Target.transform.position, EnemyAgent.destination) < 4)
        {
            // Pick a position away from the player to move
            Vector3 destination = RandomNavDirection(Controller.Target.transform.position, Random.Range(5, 8));

            // Set the enemies destination to there
            EnemyAgent.SetDestination(destination);

            jumpTarget = EnemyAgent.destination;

            jumpTime = Vector3.Distance(EnemyAgent.transform.position, destination);
            jumpTime -= EnemyAgent.stoppingDistance;
            jumpHeight = 3f;

            // Set the enemies state to jumping
            enemyState = EnemySate.Jumping;
        }
        else
        {
            // Set the enemies state to pick a new target
            enemyState = EnemySate.PickingJumpTarget;
        }
    }

    /// <summary>
    /// Makes the enemy look towards it target
    /// </summary>
    private void LookTowardsTarget()
    {
        Controller.transform.LookAt(Controller.Target.transform, Vector3.up);
        Quaternion rotation = Controller.transform.rotation;
        rotation.x = 0;
        rotation.z = 0;
        Controller.transform.rotation = rotation;
    }

    /// <summary>
    /// Controlls the jumping logic
    /// </summary>
    private void Jump()
    {
        // If the enemy is still jumping
        if (Mathf.Clamp01(EnemyAgent.remainingDistance / jumpTime) >= 0.1)
        {
            // Get it's current progress in the jump
            float progress = Mathf.Clamp01(EnemyAgent.remainingDistance / jumpTime);

            // Get it's jump height based on this parabola function
            float yOffset = -4 * jumpHeight * progress * (progress - 1);
            
            // Change it's speed based on it's height
            EnemyAgent.speed = baseSpeed + yOffset;

            // Update the agent's position to follow the curve
            Controller.transform.position = new Vector3(
                Controller.transform.position.x,
                Controller.transform.position.y + yOffset,
                Controller.transform.position.z
            );
        }
        else
        {
            // Set it's state to cooldown
            enemyState = EnemySate.Cooldown;
        }
    }

    /// <summary>
    /// Nicholas 10/26/2024
    /// Basic AI for the enemy moving around in it's guard range
    /// </summary>
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

    /// <summary>
    /// Gets a random point inside of 
    /// a circle that is on the nav mesh
    /// </summary>
    /// <param name="origin">The point of origin of the circle</param>
    /// <param name="dist">The max radius of the circle</param>
    /// <param name="layermask">The layer that it checks on</param>
    /// <returns>A point on the nav mesh</returns>
    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask = -1)
    {
        //Get a random point inside of a circle
        Vector3 randDirection = Random.insideUnitSphere * dist;

        // add that point onto the origin to move it to the correct spot
        randDirection += origin;

        // Create a navhit
        NavMeshHit navHit;

        // Sample the Navmesh to see if it's a valid position
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        // Return the position
        return navHit.position;
    }

    private Vector3 RandomNavDirection(Vector3 origin, float dist, int layermask = -1)
    {
        Vector3 randDirection = Random.insideUnitSphere.normalized * dist;

        randDirection.y = 0;

        randDirection += origin;
        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
