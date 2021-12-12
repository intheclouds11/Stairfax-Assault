using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    int currentSceneIndex;

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
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with enemy");
            GetComponent<PlayerController>().enabled = false;
            Invoke("ReloadScene", 1);
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collided with obstacle");
            GetComponent<PlayerController>().enabled = false;
            Invoke("ReloadScene", 1);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }
}