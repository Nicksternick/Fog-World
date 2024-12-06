using System.Collections.Generic;
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

    [SerializeField] private List<Elements> savedElementInventory;
    [SerializeField] private List<Forms> savedFormInventory;

    [SerializeField] private int currentLevel;

    // ===== | Properties | =====
    public int CurrentLevel { get { return currentLevel; } }

    public List<Elements> SavedElementInventory 
    { 
        get { return savedElementInventory; } 
        set { savedElementInventory = value; }
    }

    public List<Forms> SavedFormInventory 
    { 
        get { return savedFormInventory; } 
        set { savedFormInventory = value; }
    }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentLevel = 0;
            ChangeScene("Main Menu");
        }

        if (Input.GetKeyDown(KeyCode.Alpha0) &&
            Input.GetKey(KeyCode.LeftShift))
            currentLevel = 0;

        if (Input.GetKeyDown(KeyCode.Alpha1) &&
            Input.GetKey(KeyCode.LeftShift))
            currentLevel = 1;

        if (Input.GetKeyDown(KeyCode.Alpha2) &&
            Input.GetKey(KeyCode.LeftShift))
            currentLevel = 2;

        if (Input.GetKeyDown(KeyCode.Alpha3) &&
            Input.GetKey(KeyCode.LeftShift))
            currentLevel = 3;

        if (Input.GetKeyDown(KeyCode.Alpha4) &&
            Input.GetKey(KeyCode.LeftShift))
            currentLevel = 4;

        if (Input.GetKeyDown(KeyCode.Alpha5) &&
            Input.GetKey(KeyCode.LeftShift))
            currentLevel = 5;

        if (Input.GetKeyDown(KeyCode.Alpha6) &&
            Input.GetKey(KeyCode.LeftShift))
            currentLevel = 6;

        if (Input.GetKeyDown(KeyCode.Alpha7) &&
            Input.GetKey(KeyCode.LeftShift))
            currentLevel = 7;

        if (Input.GetKeyDown(KeyCode.Alpha8) &&
            Input.GetKey(KeyCode.LeftShift))
            currentLevel = 8;

        if (Input.GetKeyDown(KeyCode.Alpha9) &&
            Input.GetKey(KeyCode.LeftShift))
            currentLevel = 9;

        //if (Input.GetKeyDown(KeyCode.Escape) &&
        //    Input.GetKey(KeyCode.LeftShift))
        //{
        //    ChangeScene("SpellCraftingScene");
        //}
    }

    public void SetPlayerSpell(Spell spell, int index)
    {
        playerSpells[index] = spell;
    }

    public Spell GetPlayerSpell(int index)
    {
        return playerSpells[index] != null ? playerSpells[index] : null; 
    }

    public void IncrementCurrentLevel() { currentLevel++; }

    public void ResetLevel() { currentLevel = 0; }

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
