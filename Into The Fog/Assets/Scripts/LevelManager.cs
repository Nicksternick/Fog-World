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

    [SerializeField] private Material[] rock1Materials;
    [SerializeField] private Material[] rock2Materials;
    [SerializeField] private Material[] boulder1Materials;
    [SerializeField] private Material[] boulder2Materials;
    [SerializeField] private Material[] spike1Materials;
    [SerializeField] private Material[] spike2Materials;

    [SerializeField] private Material[] tree1Materials;
    [SerializeField] private Material[] tree2Materials;

    private int rock1Selection;
    private int rock2Selection;
    private int boulder1Selection;
    private int boulder2Selection;
    private int spike1Selection;
    private int spike2Selection;
    private int tree1Selection;
    private int tree2Selection;

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

        enemyManager.HiveNum = GameManager.Instance.CurrentLevel + 1;
    }

    private void Start()
    {
        GameObject wallContainer = GameObject.FindGameObjectWithTag("WallContainer");
        if (wallContainer != null)
        {
            rock1Selection = Random.Range(0,3);
            rock2Selection = Random.Range(0, 3);
            boulder1Selection = Random.Range(0, 3);
            boulder2Selection = Random.Range(0, 3);
            spike1Selection = Random.Range(0, 3);
            spike2Selection = Random.Range(0, 3);

            MeshRenderer[] walls = wallContainer.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer wall in walls)
            {
                if (wall != null)
                {
                    switch (wall.tag)
                    {
                        case "Rock1":
                            wall.material = rock1Materials[rock1Selection];
                            break;
                        case "Rock2":
                            wall.material = rock2Materials[rock2Selection];
                            break;
                        case "Boulder1":
                            wall.material = boulder1Materials[boulder1Selection];
                            break;
                        case "Boulder2":
                            wall.material = boulder2Materials[boulder2Selection];
                            break;
                        case "Spike1":
                            wall.material = spike1Materials[spike1Selection];
                            break;
                        case "Spike2":
                            wall.material = spike2Materials[spike2Selection];
                            break;
                    }
                }
            }
        }

        GameObject treeContainer = GameObject.FindGameObjectWithTag("TreeContainer");
        if (treeContainer != null)
        {
            tree1Selection = Random.Range(0, 3);
            tree2Selection = Random.Range(0, 3);

            MeshRenderer[] trees = treeContainer.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer tree in trees)
            {
                if (tree != null)
                {
                    switch (tree.tag)
                    {
                        case "Tree1":
                            Debug.Log(tree1Selection);
                            tree.material = tree1Materials[tree1Selection];
                            break;
                        case "Tree2":
                            Debug.Log(tree2Selection);
                            tree.material = tree2Materials[tree2Selection];
                            break;
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
            if (SceneManager.GetActiveScene().name == "FinalLevel")
            {
                GameManager.Instance.IncrementCurrentLevel();
                GameManager.Instance.AddRandomToComponentToInventory();
                GameManager.Instance.AddRandomToComponentToInventory();
            }
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
