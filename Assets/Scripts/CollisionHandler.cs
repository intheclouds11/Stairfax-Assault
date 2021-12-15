using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private int currentSceneIndex;
    private bool hasDied;
    [SerializeField] private float sceneLoadDelay = 2f;
    [SerializeField] private ParticleSystem[] explosionParticleSystems;
    [SerializeField] public int playerHealth = 100;
    [SerializeField] public TextMeshProUGUI playerHealthUI;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        playerHealthUI.text = $"Health: {playerHealth}";
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCrashSequence(other);
    }

    private void StartCrashSequence(Collider other)
    {
        if (playerHealth > 0)
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Obstacle"))
            {
                playerHealth -= 25;
                playerHealthUI.text = $"Health: {playerHealth}";
            }
        }

        if (playerHealth <= 0)
        {
            if (other.gameObject.CompareTag("Enemy") && !hasDied)
            {
                Debug.Log("Collided with enemy");
                hasDied = true;
                playerHealthUI.text = $"Health: {playerHealth}";
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<MeshRenderer>().enabled = false;
                foreach (ParticleSystem explosionParticleSystem in explosionParticleSystems)
                {
                    explosionParticleSystem.Play();
                }

                GetComponent<PlayerController>().enabled = false;
                Invoke("ReloadScene", sceneLoadDelay);
            }

            if (other.gameObject.CompareTag("Obstacle") && !hasDied)
            {
                Debug.Log("Collided with obstacle");
                hasDied = true;
                playerHealthUI.text = $"Health: {playerHealth}";
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<MeshRenderer>().enabled = false;
                foreach (ParticleSystem explosionParticleSystem in explosionParticleSystems)
                {
                    explosionParticleSystem.Play();
                }

                GetComponent<PlayerController>().enabled = false;
                Invoke("ReloadScene", sceneLoadDelay);
            }
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }
}