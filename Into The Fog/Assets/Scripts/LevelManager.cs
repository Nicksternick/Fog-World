using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerSpawnLocation;
    [SerializeField] private GameObject enemyManager;
    /// <summary>A reference to the players healthbar</summary>
    [SerializeField] HealthBar healthBar;

    public static LevelManager Instance;

    // ===== | Methods | =====
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPlayer();
        //Gives the health bar the player health
        healthBar.SetMaxHealth(player.GetComponent<PlayerController>().health);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyManager.GetComponent<EnemyManager>().enemyList.Count <= 0)
        {
            SceneManager.LoadScene("YouWin");
        }
    }

    private void OnEnable()
    {
        PlayerController.playerDamageEvent += CheckLose;
    }

    private void OnDisable()
    {
        PlayerController.playerDamageEvent -= CheckLose;
    }

    /// <summary>
    /// Ruby 10/9/2024
    /// Spawns the player
    /// </summary>
    private void spawnPlayer()
    {
        player = (Instantiate(playerPrefab, playerSpawnLocation, playerPrefab.transform.rotation));
        enemyManager.GetComponent<EnemyManager>().player = player;
    }

    /// <summary>
    /// Ruby 10/9/2024
    /// Checks if hte player has lost and updates the healthbar
    /// </summary>
    /// <param name="playerController">Event stuff</param>
    private void CheckLose(PlayerController playerController)
    {
        healthBar.SetHealth(player.GetComponent<PlayerController>().health);
        if (player.GetComponent<PlayerController>().health <= 0)
        {
            player.SetActive(false);
            SceneManager.LoadScene("GameOver");
        }
    }
}
