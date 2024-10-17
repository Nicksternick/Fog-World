using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private FogPlayer player;
    [SerializeField] private Vector3 playerSpawnLocation;
    [SerializeField] private EnemyManager enemyManager;
    /// <summary>A reference to the players healthbar</summary>
    [SerializeField] HealthBar healthBar;

    public static LevelManager Instance;

    // ===== | Properties | =====
    /*public PlayerController Player {  get { return player; } }

    // ===== | Methods | =====
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

    }
    */
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
        if (enemyManager.hiveList.Count <= 0)
        {
            SceneManager.LoadScene("YouWin");
        }
    }

    private void OnEnable()
    {
        FogPlayer.playerDamageEvent += CheckLose;
    }

    private void OnDisable()
    {
        FogPlayer.playerDamageEvent -= CheckLose;
    }

    /// <summary>
    /// Ruby 10/9/2024
    /// Spawns the player
    /// </summary>
    private void spawnPlayer()
    {
        player = (Instantiate(playerPrefab, playerSpawnLocation, playerPrefab.transform.rotation)).GetComponent<FogPlayer>();
        enemyManager.player = player.gameObject;
    }

    /// <summary>
    /// Ruby 10/9/2024
    /// Checks if hte player has lost and updates the healthbar
    /// </summary>
    /// <param name="playerController">Event stuff</param>
    private void CheckLose(FogPlayer fogPlayer)
    {
        healthBar.SetHealth(player.health);
        if (player.health <= 0)
        {
            player.gameObject.SetActive(false);
            SceneManager.LoadScene("GameOver");
        }
    }

}
