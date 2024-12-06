using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] Object sceneToSwitchTo;
    [SerializeField] private string sceneName;
    public Button sceneButton;

    void Start()
    {
        Button button = sceneButton.GetComponent<Button>();
        button.onClick.AddListener(SwitchScene);
    }

    void SwitchScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
