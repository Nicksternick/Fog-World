using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public enum UIState
{ 
    KeyboardMouse,
    Controller
}

public class SpellCraftManager : MonoBehaviour
{
    // ===== | Variables | =====
    public static SpellCraftManager Instance;

    [SerializeField] private UIState state;
    [SerializeField] private VirtualMouseInput virtualMouse;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] private bool objectSelected;
    [SerializeField] private MenuSpell[] spells;
    [SerializeField] private TextMeshProUGUI warningBox;
    [SerializeField] private GameObject[] spellComponents;
    [SerializeField] private SpellInventory spellInventory;
    [SerializeField] private TextMeshProUGUI grabGlyph;

    // ===== | Properties | =====
    public GameObject[] SpellComponents
    {
        get { return spellComponents; }
    }

    public bool ObjectSelected
    {
        get { return objectSelected; }
        set { objectSelected = value; }
    }

    public Vector2 MousePosition
    {
        get
        {
            if (state == UIState.KeyboardMouse)
            {
                return Input.mousePosition;
            }
            else
            {
                return virtualMouse.virtualMouse.position.value;
            }
        }
    }

    // ===== | Methods | =====
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        playerInput.onControlsChanged += DeviceChange;
        DeviceChange(playerInput);
        LoadSpells();
        LoadInventory();
    }

    private void Update()
    {

    }

    private void DeviceChange(PlayerInput input)
    {
        virtualMouse.gameObject.SetActive(playerInput.currentControlScheme == "Controller");
        Cursor.visible = playerInput.currentControlScheme != "Controller";

        if (playerInput.currentControlScheme == "Controller")
        {
            state = UIState.Controller;
            grabGlyph.text = "<sprite index=12>";
        }
        else
        {
            state = UIState.KeyboardMouse;
            grabGlyph.text = "<sprite index=4>";
        }
    }

    public void ToggleWarningBox()
    {
        warningBox.transform.parent.gameObject.SetActive(
            !warningBox.transform.parent.gameObject.activeSelf);
    }

    private void LoadSpells()
    {
        for (int i = 0; i < 4; i++)
        {
            if (GameManager.Instance.GetPlayerSpell(i) != null)
            {
                spells[i].LoadSpell(GameManager.Instance.GetPlayerSpell(i));
            }
        }
    }

    private void LoadInventory()
    { 
        foreach (Elements element in GameManager.Instance.SavedElementInventory)
        {
            switch (element)
            {
                case Elements.Fire:
                    Instantiate(SpellComponents[0], spellInventory.transform);
                    break;
                case Elements.Ice:
                    Instantiate(SpellComponents[1], spellInventory.transform);
                    break;
            }
        }

        foreach (Forms form in GameManager.Instance.SavedFormInventory)
        {
            switch (form)
            {
                case Forms.Ball:
                    Instantiate(SpellComponents[2], spellInventory.transform);
                    break;
                case Forms.Laser:
                    Instantiate(SpellComponents[3], spellInventory.transform);
                    break;
                case Forms.AoE:
                    Instantiate(SpellComponents[4], spellInventory.transform);
                    break;
            }
        }
    }

    private void SaveInventory()
    {
        List<Elements> elements = new List<Elements>();
        List<Forms> forms = new List<Forms>();

        foreach (SpellComponent component in spellInventory.GetComponentsInChildren<SpellComponent>())
        {
            if (component.Type == SpellComponentType.Element)
            {
                elements.Add(component.Element);
            }
            else
            {
                forms.Add(component.Form);
            }
        }

        GameManager.Instance.SavedElementInventory = elements;
        GameManager.Instance.SavedFormInventory = forms;
    }

    public void TransitionToGame()
    {
        for (int i = 0; i < 4; i++)
        {
            if (spells[i].IncompleteSpell)
            {
                warningBox.text = $"Spell {i + 1} is missing an Element or Shape";
                ToggleWarningBox();
                Invoke(nameof(ToggleWarningBox), 2);
                Debug.LogWarning("Spell is Incomplete");
                return;
            }
        }

        bool spellMade = false;
        for (int i = 0; i < 4 ; i++)
        {
            Spell newSpell = spells[i].MakeSpell();

            if (newSpell != null)
            {
                spellMade = true;
            }

            GameManager.Instance.SetPlayerSpell(newSpell, i);
        }

        if (!spellMade)
        {
            Debug.LogWarning("No Spell Made");
            warningBox.text = $"No spells were made\nMake sure to make at least 1 spell";
            ToggleWarningBox();
            Invoke(nameof(ToggleWarningBox), 2);
            return;
        }

        SaveInventory();

        Debug.LogWarning("Transition to Game");
        Cursor.visible = true;
        SceneManager.LoadScene("FinalLevel");
    }

}
