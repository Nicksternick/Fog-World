using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCrafter : MonoBehaviour
{
    public static SpellCrafter Instance;

    // Start is called before the first frame update
    void Awake()
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

    /// <summary>
    /// Jay 10/13/2024
    /// </summary>
    /// <param name="element"> Spell element </param>
    /// <param name="form"> Spell form </param>
    /// <param name="cooldown"> Spell cooldown </param>
    /// <returns> The custom spell with the inputted properties </returns>
    public Spell CraftSpell(Elements element, Forms form, float cooldown)
    {
        Spell newSpell = ScriptableObject.CreateInstance<Spell>();
        newSpell.Initialize(element, form, cooldown);
        return newSpell;
    }
}
