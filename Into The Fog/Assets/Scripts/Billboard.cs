using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AJ Wagner - 10/2/2024
/// Simple method used to help orient the enemy
/// health bars to always face the camera
/// </summary>
public class Billboard : MonoBehaviour
{
    public Transform cam;

    void Start()
    {
        if(null != Camera.main)
        {
            cam = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
