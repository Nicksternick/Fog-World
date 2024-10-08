using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerSpawnLocation;
    [SerializeField] private GameObject enemyManager;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        PlayerController.playerDamageEvent += CheckLose;
    }

    private void OnDisable()
    {
        PlayerController.playerDamageEvent -= CheckLose;
    }


    private void spawnPlayer()
    {
        player = (Instantiate(playerPrefab, playerSpawnLocation, playerPrefab.transform.rotation));
        enemyManager.GetComponent<EnemyManager>().player = player;
    }

    private void CheckLose(PlayerController playerController)
    {
        if (player.GetComponent<PlayerController>().health <= 0)
        {
            player.SetActive(false);
            SceneManager.LoadScene("GameOver");
        }
    }
}
