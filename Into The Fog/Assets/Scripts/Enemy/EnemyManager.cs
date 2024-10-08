using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Variables
    [SerializeField] private GameObject enemy;
    [SerializeField] private List<GameObject> enemyList;
    //IMPORTANT NOTE: This must be the unique player object placed into the scene and not the player prefab
    //The Enemies wont chase the player otherwise.
    [SerializeField] public GameObject player;
    [SerializeField] private Vector3[] enemySpawn;

    [SerializeField] private GameObject enemySpawners;
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

        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
            }
        }

        enemiesLeftText.text = "Enemies Left: " + enemyList.Count;
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
            enemyList[i].GetComponent<EnemyController>().target = player.transform;
        }
    }
}
