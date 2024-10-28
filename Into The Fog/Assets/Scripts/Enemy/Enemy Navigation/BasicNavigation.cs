using UnityEngine;
using UnityEngine.AI;

public class BasicNavigation : AbstractNavigation
{
    // ===== | Variables | =====
    private const float timer = 2;
    private const float chaseDistance = 50;
    private float wanderTime = timer;

    private EnemyState enemyState = EnemyState.Wander;

    /// <summary>
    /// The different states that the enemy can be in
    /// </summary>
    private enum EnemyState
    { 
        /// <summary>
        /// Enemy wanders around to 
        /// a random nearby point
        /// </summary>
        Wander,
        /// <summary>
        /// Enemy chases after the player
        /// </summary>
        Chase
    }


    // ===== | Methods | =====
    public override void Move()
    {
        if (Controller.Target != null)
        {
            // Check which state the enemy is current in
            switch (enemyState)
            {
                case EnemyState.Wander:
                    // Wander around
                    Wander();

                    // If the enemy is able to see the enemy
                    if (!Controller.PlayerInSight(chaseDistance).Equals(default(RaycastHit)))
                    {
                        // Reset the timer
                        wanderTime = timer;

                        // Change the enemies state to chase
                        enemyState = EnemyState.Chase;
                    }
                    break;
                case EnemyState.Chase:
                    // Always have the enemy update it's current destination to the player
                    EnemyAgent.SetDestination(Controller.Target.position);

                    // If the player is unable to be seen by the enemy
                    if (Controller.PlayerInSight(chaseDistance).Equals(default(RaycastHit)))
                    {
                        // Set the enemies state to wander
                        enemyState = EnemyState.Wander;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Nicholas 10/16/2024
    /// Basic AI for the enemy moving around randomly
    /// </summary>
    private void Wander()
    {
        // If the time the enemy is set to wander is over 
        if (wanderTime >= timer)
        {
            // Find a new random place to wander too
            EnemyAgent.SetDestination(RandomNavSphere(transform.position, Random.Range(30, 40)));
            // Reset the timer
            wanderTime = 0;
        }

        // While the enemy still has time to wander
        if (wanderTime < timer)
        {
            // Increment the timer
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
}
