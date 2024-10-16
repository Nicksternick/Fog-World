using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Enemy
{
    //Variables
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AbstractNavigation navigation;
    [SerializeField] private AbstractAttack attack;

    // ===== | Methods | =====
    private void Awake()
    {
        navigation.Controller = this;
        navigation.EnemyAgent = agent;
    }

    /// <summary>
    /// Ruby 9/20/2024
    /// Makes enemy follow the player
    /// </summary>
    void Update()
    {
        navigation.Move();
        attack.Attack();
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
