using System.Collections.Generic;
using UnityEngine;

public class ExtraLaserPickup : MonoBehaviour
{
    [SerializeField] private ParticleSystem pickupParticles;
    private Transform parentGameObjectTransform;
    [SerializeField] private List<GameObject> extraLasers;

    private void Start()
    {
        parentGameObjectTransform = GameObject.FindWithTag("Spawn At Runtime").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickupExtraLasers(other);
            DestroyPickup(other);
        }
    }

    private void DestroyPickup(Collider other)
    {
        Debug.Log("Triggered pickup");
        ParticleSystem vfx = Instantiate(pickupParticles, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObjectTransform;

        Destroy(this.gameObject);
    }

    void PickupExtraLasers(Collider other)
    {
        foreach (GameObject laser in extraLasers)
        {
            laser.SetActive(true);
        }
    }
}