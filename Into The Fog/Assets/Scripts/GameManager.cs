using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] private Spell defaultSpell;

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
        if (Input.GetKeyDown(KeyCode.Escape) &&
            Input.GetKey(KeyCode.LeftShift))
        {
            ResetGame();
            ChangeScene("Main Menu");
        }
    }

    public void SetPlayerSpell(Spell spell, int index)
    {
        playerSpells[index] = spell;
    }

    public Spell GetPlayerSpell(int index)
    {
        if (playerSpells.Length >= index)
            return playerSpells[index] != null ? playerSpells[index] : null; 
        else
            return null;
    }

    public void AddRandomToComponentToInventory()
    {
        int randomNumber = Random.Range(0, 2);

        if (randomNumber == 0)
        {
            savedElementInventory.Add((Elements)Random.Range(0, 2));
        }
        else
        {
            savedFormInventory.Add((Forms)Random.Range(0, 3));
        }
    }

    public void ResetGame()
    {
        savedElementInventory.Clear();
        savedElementInventory.Add(Elements.Ice);

        savedFormInventory.Clear();
        savedFormInventory.Add(Forms.Ball);

        for (int i = 0; i < playerSpells.Length; i++)
        {
            playerSpells[i] = null;
        }

        playerSpells[0] = defaultSpell;

        currentLevel = 0;
    }

    public void IncrementCurrentLevel() { currentLevel++; }

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
