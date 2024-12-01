using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellInventory : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    // ===== | Variables | =====
    [SerializeField] private Image backgroundImage;
    private Color baseColor;

    // ===== | Methods | =====
    private void Start()
    {
        baseColor = backgroundImage.color;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(nameof(OnDrop));
        if (eventData.pointerDrag.GetComponent<SpellComponent>())
        {
            eventData.pointerDrag.transform.SetParent(transform, false);
            backgroundImage.color = baseColor;
        }
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (SpellCraftManager.Instance.ObjectSelected)
        {
            backgroundImage.color = Color.white;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        backgroundImage.color = baseColor;
    }

}
