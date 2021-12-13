using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private int currentSceneIndex;
    private bool hasCrashed;
    [SerializeField] private float sceneLoadDelay = 2f;
    [SerializeField] private ParticleSystem[] explosionParticleSystems;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCrashSequence(other);
    }

    private void StartCrashSequence(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !hasCrashed)
        {
            Debug.Log("Collided with enemy");
            hasCrashed = true;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<MeshRenderer>().enabled = false;
            foreach (ParticleSystem explosionParticleSystem in explosionParticleSystems)
            {
                explosionParticleSystem.Play();
            }
            GetComponent<PlayerController>().enabled = false;
            Invoke("ReloadScene", sceneLoadDelay);
        }

        if (other.gameObject.CompareTag("Obstacle") && !hasCrashed)
        {
            Debug.Log("Collided with obstacle");
            hasCrashed = true;
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

    private void ReloadScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }
}