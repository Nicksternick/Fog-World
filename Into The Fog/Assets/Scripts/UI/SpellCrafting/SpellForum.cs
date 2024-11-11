using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellForum : MonoBehaviour
{
    public enum CurrentSpell
    {
        Spell1 = 0, 
        Spell2 = 1,
        Spell3 = 2,
        Spell4 = 3
    }

    // ===== | Variables | =====
    public static SpellForum Instance;

    [SerializeField] private ElementSlot elementSlot;
    [SerializeField] private FormSlot shapeSlot;
    [SerializeField] private SpellInventory elementInventory;
    [SerializeField] private SpellInventory shapeInventory;
    [SerializeField] private CurrentSpell selectedSpell;

    [SerializeField] TextMeshProUGUI elementText;
    [SerializeField] TextMeshProUGUI formText;

    [SerializeField] TextMeshProUGUI headsUpText;

    [SerializeField] GameObject[] elementItemPrefabs;
    [SerializeField] GameObject[] formItemPrefabs;

    [SerializeField] private List<Elements> currentElementInventory;
    [SerializeField] private List<Forms> currentFormInventory;

    // ===== | Methods | =====
    private void Start()
    {
        if (Instance == null)
            Instance = this;

        LoadSpell();

        LoadInventory();
    }

    private void Update()
    {
        elementText.text = elementSlot.ToString();

        formText.text = shapeSlot.ToString();
    }

    public void SendComponentToInventory(SpellMenuItem item)
    {
        if (item.ComponentType == SpellComponentType.Element)
        {
            item.parentAfterDrag = elementInventory.transform;
        }
        else
        {
            item.parentAfterDrag = shapeInventory.transform;
        }

        item.transform.SetParent(item.parentAfterDrag);
    }

    public void LoadSpell()
    {
        Spell spellToLoad = GameManager.Instance.GetPlayerSpell((int)selectedSpell);

        SpellMenuItem element = Instantiate(elementItemPrefabs[(int)spellToLoad.Element])
            .GetComponent<SpellMenuItem>();
        SpellMenuItem shape = Instantiate(formItemPrefabs[(int)spellToLoad.Form])
            .GetComponent<SpellMenuItem>();
        

        element.parentAfterDrag = elementSlot.transform;
        shape.parentAfterDrag = shapeSlot.transform;

        elementSlot.SetSlot(element);
        shapeSlot.SetSlot(shape);

        element.transform.SetParent(elementSlot.transform);
        shape.transform.SetParent(shapeSlot.transform);

        element.transform.localScale = Vector3.one;
        shape.transform.localScale = Vector3.one;
    }

    public void LoadInventory()
    {
        currentElementInventory = GameManager.Instance.SavedElementInventory;
        currentFormInventory = GameManager.Instance.SavedFormInventory;

        foreach (Elements element in GameManager.Instance.SavedElementInventory)
        {
            SpellMenuItem elementItem = Instantiate(elementItemPrefabs[(int)element])
            .GetComponent<SpellMenuItem>();

            elementItem.parentAfterDrag = elementInventory.transform;
            elementItem.transform.SetParent(elementInventory.transform);
            elementItem.transform.localScale = Vector3.one;
        }

        foreach (Forms form in GameManager.Instance.SavedFormInventory)
        {
            SpellMenuItem formItem = Instantiate(formItemPrefabs[(int)form])
            .GetComponent<SpellMenuItem>();

            formItem.parentAfterDrag = shapeInventory.transform;
            formItem.transform.SetParent(shapeInventory.transform);
            formItem.transform.localScale = Vector3.one;
        }
    }

    public void SaveInventory()
    {
        currentElementInventory.Clear();
        foreach (ElementItem element in elementInventory.GetComponentsInChildren<ElementItem>())
        {
            currentElementInventory.Add(element.Element);
        }

        currentFormInventory.Clear();
        foreach (FormItem form in shapeInventory.GetComponentsInChildren<FormItem>())
        {
            currentFormInventory.Add(form.Form);
        }

        GameManager.Instance.SavedElementInventory = currentElementInventory;
        GameManager.Instance.SavedFormInventory = currentFormInventory;
    }

    public void ComponentDropped(SpellMenuItem item)
    {
        if (item.ComponentType == SpellComponentType.Element)
        {
            if (elementSlot.IsChild())
            {
                elementSlot.SetSlot(item);
            }
            else
            {
                elementSlot.ClearSlot();
            }
        }
        else if (item.ComponentType == SpellComponentType.Shape)
        {
            if (shapeSlot.IsChild())
            {
                shapeSlot.SetSlot(item);
            }
            else
            {
                shapeSlot.ClearSlot();
            }
        }
    }

    public void SaveSpell()
    {
        Debug.Log($"{elementSlot.Element} && {shapeSlot.Form}");

        if (elementSlot.Element != Elements.None &&
            shapeSlot.Form != Forms.None)
        {
            Spell createdSpell = SpellCrafter.Instance.CraftSpell(
                elementSlot.Element, shapeSlot.Form, 1);
            GameManager.Instance.SetPlayerSpell(createdSpell, (int)selectedSpell);
        }
        else
        {
            GameManager.Instance.SetPlayerSpell(null, (int)selectedSpell);
        }

        ClearSpell();
    }

    public void SaveSpell(int index)
    {
        Debug.Log($"{elementSlot.Element} && {shapeSlot.Form}");

        if (elementSlot.Element != Elements.None &&
            shapeSlot.Form != Forms.None)
        {
            Spell createdSpell = SpellCrafter.Instance.CraftSpell(
                elementSlot.Element, shapeSlot.Form, 1);
            GameManager.Instance.SetPlayerSpell(createdSpell, (int)selectedSpell);
        }
        else
        {
            GameManager.Instance.SetPlayerSpell(null, (int)selectedSpell);
        }

        ClearSpell();

        selectedSpell = (CurrentSpell)index;

        if (GameManager.Instance.GetPlayerSpell(index) != null)
        {
            LoadSpell();
        }

        headsUpText.text = $"Spell {index + 1}";
    }

    public void ClearSpell()
    {
        if (elementSlot.HasChild && shapeSlot.HasChild)
        {
            Destroy(elementSlot.transform.GetChild(0).gameObject);
            Destroy(shapeSlot.transform.GetChild(0).gameObject);
        }
        else if (elementSlot.HasChild)
        {
            SendComponentToInventory(elementSlot.Item);
            elementSlot.ClearSlot();
        }
        else if (shapeSlot.HasChild)
        {
            SendComponentToInventory(shapeSlot.Item);
            shapeSlot.ClearSlot();
        }
    }
}