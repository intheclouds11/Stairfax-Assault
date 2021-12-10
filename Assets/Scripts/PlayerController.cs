using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float xControl, yControl;
    [SerializeField] private InputAction movement;
    [SerializeField] float xMovementScale = 20f;
    [SerializeField] float yMovementScale = 20f;
    [SerializeField] float positionPitchScale = -2f;
    [SerializeField] float controlPitchScale = -10f;
    [SerializeField] float positionYawScale = -10f;
    [SerializeField] float controlYawScale = -10f;
    [SerializeField] float controlRollScale = -10f;
    [SerializeField] private float rotationScale = 1f;
    [SerializeField] private float xMinRange = -10f;
    [SerializeField] private float xMaxRange = 10f;
    [SerializeField] private float yMinRange = -4f;
    [SerializeField] private float yMaxRange = 10f;


    void Start()
    {
    }

    private void OnEnable()
    {
        movement.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
    }

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
    }

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
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationScale);
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