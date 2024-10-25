using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Nicholas 10/2/2024
/// Interface that the enemy uses 
/// to navigate around the scene
/// </summary>
public abstract class AbstractNavigation : MonoBehaviour
{
    // ===== | Properties | =====
    public EnemyController Controller { set; protected get; }
    public NavMeshAgent EnemyAgent { set; protected get; }

    // ===== | Methods | =====

    /// <summary>
    /// Nicholas 10/16/2024
    /// Calls the navigation logic for the enemy
    /// </summary>
    public abstract void Move();
}
