using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 2f;

    void Start()
    {
        Destroy(gameObject,destroyDelay);
    }
}
