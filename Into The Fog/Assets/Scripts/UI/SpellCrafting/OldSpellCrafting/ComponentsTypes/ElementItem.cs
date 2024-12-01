using UnityEngine;

public class ElementItem : SpellMenuItem
{
    [SerializeField] private Elements spellElement;

    public Elements Element
    {
        get { return spellElement; }
        set { spellElement = value; }
    }
}
