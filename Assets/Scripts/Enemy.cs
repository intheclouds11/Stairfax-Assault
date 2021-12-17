using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject deathVFX;
    private Transform parentGameObjectTransform;
    [SerializeField] private int killScoreAmount = 10;
    private ScoreManager scoreManager;
    [SerializeField] public int enemyHP = 2;
    private Color defaultEnemyColor;


    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        defaultEnemyColor = GetComponent<Renderer>().material.color;
        parentGameObjectTransform = GameObject.FindWithTag("Spawn At Runtime").transform;
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        if (enemyHP < 1)
        {
            DestroyEnemy();
        }
    }

    private void ProcessHit()
    {
        GetComponent<Renderer>().material.color = Color.white;
        Invoke("ReturnToDefaultColor", .1f);
        enemyHP--;
    }

    void ReturnToDefaultColor()
    {
        GetComponent<Renderer>().material.color = defaultEnemyColor;
    }

    private void DestroyEnemy()
    {
        scoreManager.IncreaseScore(killScoreAmount);
        GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObjectTransform; // cleaner Hierarchy
        Destroy(this.gameObject);
    }
}