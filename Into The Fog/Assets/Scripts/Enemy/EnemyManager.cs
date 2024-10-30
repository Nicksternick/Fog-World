using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Variables
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject hive;
    [SerializeField] public List<GameObject> enemyList;
    [SerializeField] public List<GameObject> importantList;
    [SerializeField] public GameObject player;
    [SerializeField] private Vector3[] enemySpawn;
    [SerializeField] private Vector3[] hiveSpawn;
    [SerializeField] private GameObject[] enemySpawners;
    [SerializeField] private GameObject hiveSpawners;
    [SerializeField] TextMeshProUGUI enemiesLeftText;

    // Start is called before the first frame update
    void Start()
    {
        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < importantList.Count; i++)
        {
            if (importantList[i] == null)
            {
                importantList.RemoveAt(i);
            }
        }

        enemiesLeftText.text = "Hives Left: " + importantList.Count;
    }

    /// <summary>
    /// Ruby 9/21/2024
    /// Spawns enemies and adds them to enemy list
    /// </summary>
    void spawn()
    {
        //Spawns all of the enemies
        for (int i = 0; i < enemySpawners.Length; i++)
        {
            if (enemySpawners[i].GetComponent<EnemySpawner>().spawnEnemy.GetComponent<Enemy>().isImportant)
            {
                for (int j = 0; j < enemySpawners[i].GetComponent<EnemySpawner>().spawnPositions.Length; j++)
                {
                    importantList.Add(Instantiate(enemySpawners[i].GetComponent<EnemySpawner>().spawnEnemy,
                        enemySpawners[i].GetComponent<EnemySpawner>().spawnPositions[j].position,
                        Quaternion.identity));
                    importantList[j].GetComponent<Enemy>().Target = player.transform;
                }
            }
            else
            {
                for (int j = 0; j < enemySpawners[i].GetComponent<EnemySpawner>().spawnPositions.Length; j++)
                {
                    enemyList.Add(Instantiate(enemySpawners[i].GetComponent<EnemySpawner>().spawnEnemy,
                        enemySpawners[i].GetComponent<EnemySpawner>().spawnPositions[j].position,
                        Quaternion.identity));

                    enemyList[j].GetComponent<EnemyController>().Target = player.transform;
                }
            }
            
        }
        
    }
}
