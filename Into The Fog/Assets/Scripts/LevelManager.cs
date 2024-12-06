using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // ===== | Variables | =====
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private FogPlayer player;
    [SerializeField] private Vector3 playerSpawnLocation;
    [SerializeField] private EnemyManager enemyManager;

    [SerializeField] private Material[] rockMaterials;
    [SerializeField] private Material[] floorMaterials;
    [SerializeField] private string winScene;

    public static LevelManager Instance;

    public List<GameObject> ImportantEnemies { get { return enemyManager.importantList; } }

    // ===== | Properties | =====
    public Vector3 PlayerPosition
    {
        get { return player.transform.position; }
    }

    // ===== | Methods | =====
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        spawnPlayer();

        //int extraHives = 0;

        //if (GameManager.Instance.CurrentLevel >= 3)
        //    extraHives++;

        //if (GameManager.Instance.CurrentLevel >= 6)
        //    extraHives++;

        enemyManager.HiveNum = GameManager.Instance.CurrentLevel + 1;
    }

    private void Start()
    {
        if (rockMaterials != null)
        {
            GameObject wallContainer = GameObject.FindGameObjectWithTag("WallContainer");
            if (wallContainer != null)
            {
                MeshRenderer[] walls = wallContainer.GetComponentsInChildren<MeshRenderer>();
                Material rockMaterial = rockMaterials[Random.Range(0, rockMaterials.Length)];
                foreach (MeshRenderer wall in walls)
                {
                    if (wall != null)
                    {
                        wall.material = rockMaterial;
                    }
                }
            }
        }
        
        if (floorMaterials != null)
            Invoke(nameof(GenerateFloorMaterial), 0.5f);
    }

    public void GenerateFloorMaterial()
    {
        GameObject floor = GameObject.FindGameObjectWithTag("Floor");
        if (floor != null)
        {
            MeshRenderer floorRenderer = floor.GetComponent<MeshRenderer>();
            floorRenderer.material = floorMaterials[Random.Range(0, floorMaterials.Length)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyManager.importantList.Count <= 0)
        {
            GameManager.Instance.IncrementCurrentLevel();
            SceneManager.LoadScene(winScene);
        }

        string hiveCount = "";

        for (int i = 0; i < enemyManager.importantList.Count; ++i)
        {
            hiveCount += "<sprite index=0> ";
        }

        UIManager.Instance.HiveCount.text = "Hives: " + hiveCount;
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
    /// <param name="FogPlayer">Event stuff</param>
    private void CheckLose(FogPlayer fogPlayer)
    {
        //healthBar.SetHealth(player.Health);
        if (player.Health <= 0)
        {
            player.gameObject.SetActive(false);
            SceneManager.LoadScene("GameOver");
        }
    }

}
