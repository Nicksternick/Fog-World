using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] Object sceneToSwitchTo;
    public Button sceneButton;

    void Start()
    {
        Button button = sceneButton.GetComponent<Button>();
        button.onClick.AddListener(SwitchScene);
    }

    void SwitchScene()
    {
        SceneManager.LoadScene(sceneToSwitchTo.name);
    }
}
