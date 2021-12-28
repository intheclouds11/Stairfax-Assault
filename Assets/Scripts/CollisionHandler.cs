using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private int currentSceneIndex;
    private bool hasDied;
    [SerializeField] private float sceneLoadDelay = 2f;
    [SerializeField] private GameObject[] explosionFX;
    [SerializeField] public int playerHealth = 100;
    [SerializeField] public TextMeshProUGUI playerHealthUI;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private PlayerController playerController;
    private AudioSource shipHitSFX;
    [SerializeField] private AudioClip shipHitSFXClip;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        playerHealthUI.text = $"Health: {playerHealth}";
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        playerController = GetComponent<PlayerController>();
        shipHitSFX = GetComponents<AudioSource>()[1]; // grab second AudioSource on PlayerShip GO
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Pickup"))
        {
            StartCrashSequence(other);
        }
    }

    private void StartCrashSequence(Collider other)
    {
        if (playerHealth > 0)
        {
            playerHealth -= 25;
            playerHealthUI.text = $"Health: {playerHealth}";
            shipHitSFX.PlayOneShot(shipHitSFXClip);
        }

        if (playerHealth <= 0)
        {
            if (hasDied) return;
            hasDied = true;
            playerHealthUI.text = $"Health: {playerHealth}";
            shipHitSFX.PlayOneShot(shipHitSFXClip);

            rb.useGravity = true;
            meshRenderer.enabled = false;
            playerController.enabled = false;

            foreach (GameObject fx in explosionFX)
            {
                fx.SetActive(true);
            }

            Invoke(nameof(ReloadScene), sceneLoadDelay);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }
}