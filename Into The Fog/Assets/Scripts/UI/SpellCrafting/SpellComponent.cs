using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpellComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // ===== | Variables | =====
    [SerializeField] SpellComponentType type;
    [SerializeField] Elements element;
    [SerializeField] Forms form;
    [SerializeField] Image outerBorder;
    private Color baseColor;
    private bool isHovered;
    private bool isSelected;

    // ===== | Properties | =====
    public SpellComponentType Type { get { return type; } }
    public Elements Element { get { return element; } }
    public Forms Form { get { return form; } }

    public string Name
    {
        get 
        {
            string name = "";
            if (type == SpellComponentType.Element)
            {
                name = element.ToString();
            }
            else
            {
                name = form.ToString();
            }

            return name; 
        }
    }

    // ===== | Methods | =====
    private void Start()
    {
        baseColor = outerBorder.color;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isSelected = true;
        outerBorder.color = Color.yellow;

        outerBorder.raycastTarget = false;

        SpellCraftManager.Instance.ObjectSelected = true;
        AudioManager.Instance.PlaySound("On Component Click", true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSelected)
        {
            transform.position = SpellCraftManager.Instance.MousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isSelected = false;
        outerBorder.color = baseColor;

        outerBorder.raycastTarget = true;
        Transform parent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetParent(parent);

        SpellCraftManager.Instance.ObjectSelected = false;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (isSelected)
            return;

        if (!SpellCraftManager.Instance.ObjectSelected)
        {
            isHovered = true;
            outerBorder.color = Color.white;
        }
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (isSelected)
            return;

        if (isHovered)
        {
            isHovered = false;
            outerBorder.color = baseColor;
        }
    }
}
