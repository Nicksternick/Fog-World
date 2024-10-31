using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransitionButton : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        GameManager.Instance.ChangeScene(name);
    }
}
