using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Enemy
{
    //Variables
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AbstractNavigation navigation;
    [SerializeField] private AbstractAttack attack;
    [SerializeField] private float speedModifier = 0.2f;

    // ===== | Methods | =====
    private void Awake()
    {
        navigation.Controller = this;
        navigation.EnemyAgent = agent;
        agent.speed += 0.15f;
    }
    private void Start()
    {
        EnemyStart();
        agent.speed += (speedModifier * GameManager.Instance.CurrentLevel);
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
