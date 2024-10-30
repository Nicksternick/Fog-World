using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HiveEnemy : Enemy
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AbstractNavigation navigation;
    [SerializeField] private GameObject enemy;
    [SerializeField] public List<GameObject> enemyList;
    [SerializeField] private float spawnCooldown;

    // Start is called before the first frame update
    void Start()
    {
        spawnCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCooldown<=0 && enemyList.Count<4)
        {
            spawn();
            spawnCooldown = 1;
        }
        spawnCooldown -= Time.deltaTime;

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
            }
        }
    }

    void spawn()
    {
        GameObject newEnemy = Instantiate(enemy, new Vector3(this.transform.position.x+1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        newEnemy.GetComponent<EnemyController>().Target = this.Target;
        enemyList.Add(newEnemy);
    }
}
