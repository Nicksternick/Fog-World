using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Variables
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject[] enemyList;
    //IMPORTANT NOTE: This must be the uniqe player object placed into the scene and not the player prefab
    //The Enemies wont chase the player otherwise.
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Ruby 9/21/2024
    /// Spawns enemies and adds them to enemy list
    /// </summary>
    void spawn()
    {
        //These are just going to have to be hard coded every time
        //Vector3[] enemySpawn = new[] { new Vector3(10f, 0.5f, 10f), new Vector3(-10f,0.5f,-10f) };
        Vector3[] enemySpawn = new[] { new Vector3(10f, 0.5f, 10f) };
        enemyList = new GameObject[5];

        //Spawns all of the enemies
        for (int i = 0; i<enemySpawn.Length; i++)
        {
            enemyList[i]=Instantiate(enemy, enemySpawn[i], Quaternion.identity);
            enemyList[i].GetComponent<EnemyController>().target = player.transform;
        }
    }
}
