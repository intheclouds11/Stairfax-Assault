using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject deathFX;
    [SerializeField] private GameObject hitFX;
    private Transform parentGameObjectTransform;
    [SerializeField] private int killScoreAmount = 10;
    private ScoreManager scoreManager;
    [SerializeField] public int enemyHP = 2;
    private Color defaultEnemyColor;


    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>(); // This is an example using FindObjectOfType, could also use SerializeField.
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
        Invoke(nameof(ReturnToDefaultColor), .1f);
        GameObject fx = Instantiate(hitFX, transform.position, Quaternion.identity);
        fx.transform.parent = parentGameObjectTransform; // cleaner Hierarchy
        enemyHP--;
    }

    void ReturnToDefaultColor()
    {
        GetComponent<Renderer>().material.color = defaultEnemyColor;
    }

    private void DestroyEnemy()
    {
        scoreManager.IncreaseScore(killScoreAmount);
        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        fx.transform.parent = parentGameObjectTransform; // cleaner Hierarchy
        Destroy(this.gameObject);
    }
}