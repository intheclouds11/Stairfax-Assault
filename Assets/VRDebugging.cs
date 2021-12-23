using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.XR;

public class VRDebugging : MonoBehaviour
{
    [SerializeField] private GameObject[] XRControllersToToggle;
    [SerializeField] private bool enableVRControllers;
    [SerializeField] private GameObject XRDeviceSim;
    
    void Start()
    {
        DetectHmd();
    }

    private void Update()
    {
        ToggleVRControllers(); // inefficient! better to use an event?
    }

    private void DetectHmd()
    {
        Debug.Log("XR Device detected: " + XRSettings.loadedDeviceName);

        if (XRSettings.loadedDeviceName == "MockHMD Display")
        {
            XRSettings.gameViewRenderMode = GameViewRenderMode.RightEye;
        }
        else
        {
            // disable XRDeviceSimulator while using HMD + controllers
            XRDeviceSim.SetActive(false);
        }
    }

    private void ToggleVRControllers()
    {
        if (enableVRControllers)
        {
            foreach (var gameObject in XRControllersToToggle)
            {
                gameObject.SetActive(enableVRControllers);
            }
        }

        if (!enableVRControllers)
        {
            foreach (var gameObject in XRControllersToToggle)
            {
                gameObject.SetActive(enableVRControllers);
            }
        }
    }
}