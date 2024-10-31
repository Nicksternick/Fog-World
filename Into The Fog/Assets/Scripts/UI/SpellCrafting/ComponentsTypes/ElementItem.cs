using UnityEngine;

public class ElementItem : SpellMenuItem
{
    [SerializeField] private Elements spellElement;

    public ElementItem Item { get { return this; } }

    public Elements Element
    {
        get { return spellElement; }
        set { spellElement = value; }
    }
}
