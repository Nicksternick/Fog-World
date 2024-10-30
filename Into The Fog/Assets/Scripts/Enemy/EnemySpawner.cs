using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public GameObject spawnEnemy;
    [SerializeField] public Transform[] spawnPositions;
         
    // Start is called before the first frame update
    /// <summary>
    /// Makes a list of all spawn locations
    /// </summary>
    void Start()
    {
        List<Transform> list = new List<Transform>();

        foreach (Transform transform in GetComponentsInChildren<Transform>())
        {
           list.Add(transform);
        }

        list.RemoveAt(0);

        spawnPositions = list.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
