using TMPro;
using UnityEngine;

public class SpellForum : MonoBehaviour
{
    public enum CurrentSpell
    {
        Spell1 = 0, 
        Spell2 = 1
    }

    // ===== | Variables | =====
    [SerializeField] private ElementItem elementItemPrefab;
    [SerializeField] private FormItem formItemPrefab;
    [SerializeField] private ElementSlot elementSlot;
    [SerializeField] private FormSlot shapeSlot;
    [SerializeField] private SpellInventory inventory;
    [SerializeField] private CurrentSpell selectedSpell;

    [SerializeField] TextMeshProUGUI elementText;
    [SerializeField] TextMeshProUGUI formText;

    [SerializeField] TextMeshProUGUI headsUpText;

    private void Update()
    {
        elementText.text = elementSlot.Element.ToString();

        formText.text = shapeSlot.Form.ToString();
    }

    public void SaveSpell()
    {
        if (elementSlot.Element != Elements.None && shapeSlot.Form != Forms.None)
        {
            Debug.Log($"{elementSlot.Element} && {shapeSlot.Form}");
            Spell createdSpell = SpellCrafter.Instance.CraftSpell(elementSlot.Element, shapeSlot.Form, 1);
            GameManager.Instance.SetPlayerSpell(createdSpell, (int)selectedSpell);

            headsUpText.text = "Spell Successfully Crafted!";
        }
        else
        {
            headsUpText.text = "Spell is missing an component";
        }
    }

    public void SaveSpell(int index)
    {
        if (elementSlot.Element != Elements.None && shapeSlot.Form != Forms.None)
        {
            Debug.Log($"{elementSlot.Element} && {shapeSlot.Form}");
            Spell createdSpell = SpellCrafter.Instance.CraftSpell(elementSlot.Element, shapeSlot.Form, 1);
            GameManager.Instance.SetPlayerSpell(createdSpell, index);

            headsUpText.text = $"Spell {index + 1} Successfully Crafted!";
        }
        else
        {
            headsUpText.text = "Spell is missing an component";
        }
    }
}