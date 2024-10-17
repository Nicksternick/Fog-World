using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Variables
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject hive;
    [SerializeField] public List<GameObject> enemyList;
    [SerializeField] public List<GameObject> hiveList;
    [SerializeField] public GameObject player;
    [SerializeField] private Vector3[] enemySpawn;
    [SerializeField] private Vector3[] hiveSpawn;
    [SerializeField] private GameObject enemySpawners;
    [SerializeField] private GameObject hiveSpawners;
    [SerializeField] TextMeshProUGUI enemiesLeftText;

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> list = new List<Vector3>();

        foreach (Transform transform in enemySpawners.GetComponentsInChildren<Transform>())
        {
            list.Add(transform.position);
        }

        list.RemoveAt(0);

        enemySpawn = list.ToArray();

        List<Vector3> hiveList = new List<Vector3>();

        foreach (Transform transform in hiveSpawners.GetComponentsInChildren<Transform>())
        {
            hiveList.Add(transform.position);
        }

        hiveList.RemoveAt(0);

        hiveSpawn = hiveList.ToArray();

        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hiveList.Count; i++)
        {
            if (hiveList[i] == null)
            {
                hiveList.RemoveAt(i);
            }
        }

        enemiesLeftText.text = "Hives Left: " + hiveList.Count;
    }

    /// <summary>
    /// Ruby 9/21/2024
    /// Spawns enemies and adds them to enemy list
    /// </summary>
    void spawn()
    {
        //Spawns all of the enemies
        for (int i = 0; i< enemySpawn.Length; i++)
        {
            enemyList.Add(Instantiate(enemy, enemySpawn[i], Quaternion.identity));
            enemyList[i].GetComponent<EnemyController>().Target = player.transform;
        }
        for (int i = 0; i < hiveSpawn.Length; i++)
        {
            hiveList.Add(Instantiate(hive, hiveSpawn[i], Quaternion.identity));
            //hiveList[i].GetComponent<EnemyController>().Target = player.transform;
        }
    }
}
