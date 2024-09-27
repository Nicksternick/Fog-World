using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationLock : MonoBehaviour
{
    //Prevents camera from rotating with player

    private Quaternion myRotation;
    // Start is called before the first frame update
    void Start()
    {
        myRotation = this.transform.rotation;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.rotation = myRotation;
    }
}
