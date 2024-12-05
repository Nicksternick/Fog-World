using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Object sceneToSwitchTo;
    public Button sceneButton;
    public TMP_Text buttonText;

    void Start()
    {
        Button button = sceneButton.GetComponent<Button>();
        button.onClick.AddListener(SwitchScene);
    }

    void SwitchScene()
    {
        SceneManager.LoadScene(sceneToSwitchTo.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = new Color(130f / 255f, 106f / 255f, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = new Color(179f / 255f, 179f / 255f, 179f / 255f);
    }
}
