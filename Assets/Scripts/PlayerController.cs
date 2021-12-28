using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float xControl, yControl;

    [Header("Control Inputs")] [SerializeField]
    InputActionAsset playerControlsNonVR;

    InputAction movementNonVR;

    InputAction fireNonVR;

    // For VR controls
    [Header("VR Control Inputs")] [SerializeField]
    InputActionAsset playerControlsVR;

    InputAction movementVR;

    InputAction fireVRLH;

    InputAction fireVRRH;
    //

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

    private AudioSource _laserSFX;

    [SerializeField] private AudioClip _laserSFXClip;

    [SerializeField] private float _laserSFXDelay = 0.2f;


    private void Start()
    {
        EnableVRControls();
        EnableNonVRControls();
        _laserSFX = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
    }

    private void EnableVRControls()
    {
        var gameplayActionMapLH = playerControlsVR.FindActionMap("XRI LeftHand");
        var gameplayActionMapRH = playerControlsVR.FindActionMap("XRI RightHand");

        movementVR = gameplayActionMapLH.FindAction("Move");
        movementVR.performed += ProcessMovementInputVR;
        movementVR.canceled += ProcessMovementInputVR;
        movementVR.Enable();

        fireVRLH = gameplayActionMapLH.FindAction("Activate");
        fireVRRH = gameplayActionMapRH.FindAction("Activate");
        fireVRLH.performed += ProcessFiring;
        fireVRRH.performed += ProcessFiring;
        fireVRLH.Enable();
        fireVRRH.Enable();
    }

    private void ProcessMovementInputVR(InputAction.CallbackContext context)
    {
        xControl = context.ReadValue<Vector2>().x;
        yControl = context.ReadValue<Vector2>().y;
    }

    private void EnableNonVRControls()
    {
        var gameplayActionMap = playerControlsNonVR.FindActionMap("Default");

        movementNonVR = gameplayActionMap.FindAction("Movement");
        movementNonVR.performed += ProcessMovementInputNonVR;
        movementNonVR.canceled += ProcessMovementInputNonVR;
        movementNonVR.Enable();

        fireNonVR = gameplayActionMap.FindAction("Fire");
        fireNonVR.performed += ProcessFiring;
        fireNonVR.Enable();
    }

    private void ProcessMovementInputNonVR(InputAction.CallbackContext context)
    {
        xControl = context.ReadValue<Vector2>().x;
        yControl = context.ReadValue<Vector2>().y;
    }

    private void ProcessTranslation()
    {
        float xOffset = xControl * xMovementScale * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, xMinRange, xMaxRange);

        float yOffset = yControl * yMovementScale * Time.deltaTime;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, yMinRange, yMaxRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
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
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed);
    }

    private void ProcessFiring(InputAction.CallbackContext context)
    {
        foreach (ParticleSystem laserParticleSystem in laserParticleSystems)
        {
            laserParticleSystem.Play();
        }
        _laserSFX.PlayOneShot(_laserSFXClip);
        _laserSFX.PlayDelayed(_laserSFXDelay);
    }
}