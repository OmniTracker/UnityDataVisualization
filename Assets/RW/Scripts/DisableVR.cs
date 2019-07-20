using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DisableVR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Disable VR 
        XRSettings.enabled = false;
    }
}
