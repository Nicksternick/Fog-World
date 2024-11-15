using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoxPusher : MonoBehaviour
{
    private Material objectMaterial; // The material using the shader

    private void Start()
    {
        objectMaterial = GetComponent<MeshRenderer>().material;
    }
    void Update()
    {
        // Pass the player's position to the shader as the reference position
        objectMaterial.SetVector("_ReferencePosition", LevelManager.Instance.PlayerPosition);
    }
}
