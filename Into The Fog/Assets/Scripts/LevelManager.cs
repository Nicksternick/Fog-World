using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerSpawnLocation;
    [SerializeField] private GameObject enemyManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawnPlayer()
    {
        player = (Instantiate(playerPrefab, playerSpawnLocation, playerPrefab.transform.rotation));
        enemyManager.GetComponent<EnemyManager>().player = player;
    }
}
