using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nicholas 10/2/2024
/// Interface that the enemy uses 
/// to navigate around the scene
/// </summary>
public interface IEnemyNavigation
{
    // ===== | Variables | =====
    EnemyController Controller { set; }

    // ===== | Methods | =====
    public void Move();
}
