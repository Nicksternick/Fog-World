using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Nicholas 10/1/2024
/// Holds information that is needed between scenes
/// And handles saving, and loading between scenes
/// </summary>
public class GameManager : MonoBehaviour
{
    // ===== | Variables | =====
    public static GameManager Instance;

    [SerializeField] private Spell[] playerSpells;

    // ===== | Methods | =====
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            playerSpells = new Spell[2];
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeScene("Cave Level");
        }
    }

    public void SetPlayerSpell(Spell spell, int index)
    {
        playerSpells[index] = spell;
    }

    public Spell GetPlayerSpell(int index)
    {
        return playerSpells[index] != null ? playerSpells[index] : null; 
    }

    /// <summary>
    /// Nicholas 10/1/2024
    /// Loads the scene that is passed in as a parameter
    /// </summary>
    /// <param name="sceneName"></param>
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
