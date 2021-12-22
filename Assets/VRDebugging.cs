using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.XR;

public class VRDebugging : MonoBehaviour
{
    [SerializeField] private GameObject[] disableWhileUsingMockHMD;

    void Start()
    {
        // if using MockHMD, then set VR controllers to inactive

        if (XRSettings.loadedDeviceName == "MockHMD Display")
        {
            Debug.Log("MockHMD detected, disabling VR controllers");
            foreach (var gameObject in disableWhileUsingMockHMD)
            {
                gameObject.SetActive(false);
            }
        }

        else
        {
            Debug.Log("XR Device detected: " + XRSettings.loadedDeviceName);
        }
        
    }
}