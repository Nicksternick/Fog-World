using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCrafter : MonoBehaviour
{
    public static SpellCrafter Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public Spell CraftSpell(Elements element, Forms form, float cooldown)
    {
        Spell newSpell = ScriptableObject.CreateInstance<Spell>();
        newSpell.Initialize(element, form, cooldown);
        return newSpell;
    }
}
