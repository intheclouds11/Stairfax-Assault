using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRepairPickup : MonoBehaviour
{
    [SerializeField] private ParticleSystem pickupParticles;
    private Transform parentGameObjectTransform;

    private void Start()
    {
        parentGameObjectTransform = GameObject.FindWithTag("Spawn At Runtime").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickupShipRepair(other);
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

    void PickupShipRepair(Collider other)
    {
        CollisionHandler playerCollisionHandler = other.gameObject.GetComponentInParent<CollisionHandler>();
        playerCollisionHandler.playerHealth += 25;
        playerCollisionHandler.playerHealthUI.text = $"Health: {playerCollisionHandler.playerHealth}";
    }
}