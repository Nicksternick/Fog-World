using UnityEngine;

public class LevelTransitionButton : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        GameManager.Instance.ChangeScene(name);
    }
}
