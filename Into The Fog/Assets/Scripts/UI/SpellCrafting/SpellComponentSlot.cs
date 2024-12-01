using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SpellComponentType
{
    None = -1,
    Element,
    Shape
}

public class SpellComponentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    // ===== | Variables | =====
    [SerializeField] protected SpellComponentType componentType;
    [SerializeField] private bool selectionOverSlot;
    [SerializeField] private Image backdrop;
    [SerializeField] private TextMeshProUGUI componentText;
    private Color baseColor;
    [SerializeField] private SpellComponent spell;

    // ===== | Properties | =====
    public SpellComponent Component { get { return spell; } }

    // ===== | Methods | =====
    private void Start()
    {
        baseColor = backdrop.color;
        if (spell == null)
        {
            componentText.text = "None";
        }
    }

    private void Update()
    {
        if (spell && transform.childCount == 0)
        {
            spell = null;
            componentText.text = "None";
        }
    }

    public void SetSpell(SpellComponent spell)
    {
        Debug.Log(spell.Name);
        this.spell = spell;
        componentText.text = spell.Name;

    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<SpellComponent>())
        {
            if (eventData.pointerDrag.GetComponent<SpellComponent>().Type == componentType)
            {
                spell = eventData.pointerDrag.GetComponent<SpellComponent>();
                eventData.pointerDrag.transform.SetParent(transform, false);
                backdrop.color = baseColor;
                componentText.text = spell.Name;
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (SpellCraftManager.Instance.ObjectSelected)
        {
            if (pointerEventData.pointerDrag.GetComponent<SpellComponent>())
            {
                if (pointerEventData.pointerDrag.GetComponent<SpellComponent>().Type == componentType)
                {
                    selectionOverSlot = true;
                    backdrop.color = Color.white;
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        selectionOverSlot = false;
        backdrop.color = baseColor;
    }
}
