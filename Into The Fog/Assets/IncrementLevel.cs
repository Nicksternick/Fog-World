using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementLevel : MonoBehaviour
{
    public void Increment()
    {
        GameManager.Instance.IncrementCurrentLevel();
    }
}
