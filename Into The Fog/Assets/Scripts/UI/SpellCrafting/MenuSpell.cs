using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpell : MonoBehaviour
{
    // ===== | Variables | =====
    [SerializeField] private SpellComponentSlot elementSlot;
    [SerializeField] private SpellComponentSlot formSlot;

    // ===== | Properties | =====
    public bool IncompleteSpell
    {
        get
        {
            return elementSlot.Component != null ^ 
                formSlot.Component != null;
        }
    }

    // ===== | Method | =====
    public Spell MakeSpell()
    {
        if (formSlot.Component != null &&
            elementSlot.Component != null)
        {
            Spell newSpell = null;
            switch (formSlot.Component.Form)
            {
                case Forms.Ball:
                    newSpell = SpellCrafter.Instance.CraftSpell(elementSlot.Component.Element, formSlot.Component.Form, 0.4f);
                    break;
                case Forms.Laser:
                    newSpell = SpellCrafter.Instance.CraftSpell(elementSlot.Component.Element, formSlot.Component.Form, 4f);
                    break;
                case Forms.AoE:
                    newSpell = SpellCrafter.Instance.CraftSpell(elementSlot.Component.Element, formSlot.Component.Form, 8f);
                    break;
            }
            return newSpell;
        }

        return null;
    }

    public void LoadSpell(Spell spell)
    {
       SetElement(spell.Element);
       SetForm(spell.Form);
    }

    private void SetElement(Elements element)
    {
        SpellComponent spell = null;
        switch (element)
        {
            case Elements.Fire:
                spell = Instantiate(SpellCraftManager.Instance.SpellComponents[0], elementSlot.transform).GetComponent<SpellComponent>();
                break;
            case Elements.Ice:
                spell = Instantiate(SpellCraftManager.Instance.SpellComponents[1], elementSlot.transform).GetComponent<SpellComponent>();
                break;
        }
        elementSlot.SetSpell(spell);
    }

    private void SetForm(Forms form)
    {
        SpellComponent spell = null;
        switch (form)
        {
            case Forms.Ball:
                spell = Instantiate(SpellCraftManager.Instance.SpellComponents[2], formSlot.transform).GetComponent<SpellComponent>();
                break;
            case Forms.Laser:
                spell = Instantiate(SpellCraftManager.Instance.SpellComponents[3], formSlot.transform).GetComponent<SpellComponent>();
                break;
            case Forms.AoE:
                spell = Instantiate(SpellCraftManager.Instance.SpellComponents[4], formSlot.transform).GetComponent<SpellComponent>();
                break;
        }
        formSlot.SetSpell(spell);
    }
}
