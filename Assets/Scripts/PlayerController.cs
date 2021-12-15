using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    private float xControl, yControl;

    [Header("Control Inputs")] [SerializeField]
    InputAction movement;

    [SerializeField] InputAction fire;

    [Header("Control Scaling")] [Tooltip("How fast ship moves side-to-side")] [SerializeField]
    float xMovementScale = 20f;

    [Tooltip("How fast ship moves up and down")] [SerializeField]
    float yMovementScale = 20f;

    [Header("Rotation Scaling")] [Tooltip("Ship pitch amount based on screen position")] [SerializeField]
    float positionPitchScale = -2f;

    [Tooltip("Ship pitch amount based on player input")] [SerializeField]
    float controlPitchScale = -10f;

    [Tooltip("Ship yaw amount based on screen position")] [SerializeField]
    float positionYawScale = -10f;

    [Tooltip("Ship yaw amount based on player input")] [SerializeField]
    float controlYawScale = -10f;

    [Tooltip("Ship roll amount based on player input")] [SerializeField]
    float controlRollScale = -10f;

    [Tooltip("Ship rotation speed")] [SerializeField]
    float rotationSpeed = 1f;

    [Header("Position Clamping")] [Tooltip("Min horizontal position of ship")] [SerializeField]
    float xMinRange = -10f;

    [Tooltip("Max horizontal position of ship")] [SerializeField]
    float xMaxRange = 10f;

    [Tooltip("Min vertical position of ship")] [SerializeField]
    float yMinRange = -4f;

    [Tooltip("Max vertical position of ship")] [SerializeField]
    float yMaxRange = 10f;

    [SerializeField] public List<ParticleSystem> laserParticleSystems;
    
    private void OnEnable()
    {
        movement.Enable();
        fire.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        fire.Disable();
    }

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    private void ProcessFiring()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            ActivateLasers();
        }
        
        // else
        // {
        //     DeactivateLasers();
        // }
    }

    private void ActivateLasers()
    {
        foreach (ParticleSystem laserParticleSystem in laserParticleSystems)
        {
            laserParticleSystem.Play();
        }

        // Alternative way to enable particles (particles always playing just not emitting)
        // foreach (ParticleSystem laserParticleSystem in laserParticleSystems)
        // {
        //     var emissionModule = laserParticleSystem.emission;
        //     emissionModule.enabled = toggle;
        // }
    }

    // private void DeactivateLasers()
    // {
    //     foreach (ParticleSystem laserParticleSystem in laserParticleSystems)
    //     {
    //         laserParticleSystem.Stop();
    //
    //     }
    // }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchScale;
        float pitchDueToControl = yControl * controlPitchScale;
        float yawDueToPosition = transform.localPosition.x * positionYawScale;
        float yawDueToControl = xControl * controlYawScale;
        float rollDueToControl = xControl * controlRollScale;

        float pitch = pitchDueToPosition + pitchDueToControl; // coupled with position on screen and input
        float yaw = yawDueToPosition + yawDueToControl; // coupled with position on screen and input
        float roll = rollDueToControl; // coupled with input

        // Since using new input system, need to force an incremental rotation using RotateTowards
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed);
    }

    private void ProcessTranslation()
    {
        // OLD INPUT SYSTEM - interpolates between -1, 0 ,and +1
        // xControl = Input.GetAxis("Horizontal");
        // yControl = Input.GetAxis("Vertical");

        // NEW INPUT SYSTEM - instantaneously jumps between -1, 0, and +1
        xControl = movement.ReadValue<Vector2>().x;
        yControl = movement.ReadValue<Vector2>().y;

        float xOffset = xControl * xMovementScale * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, xMinRange, xMaxRange);

        float yOffset = yControl * yMovementScale * Time.deltaTime;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, yMinRange, yMaxRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }
}