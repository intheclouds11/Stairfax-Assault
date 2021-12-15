using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLaserPickup : MonoBehaviour
{
    [SerializeField] private ParticleSystem pickupParticles;
    [SerializeField] private GameObject extraLaser;
    private Transform parentGameObjectTransform;
    [SerializeField] private Vector3 extraLaser1Offset;
    [SerializeField] private Vector3 extraLaser2Offset;

    private void Start()
    {
        parentGameObjectTransform = GameObject.FindWithTag("Spawn At Runtime").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickupExtraLaser(other);
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

    void PickupExtraLaser(Collider other)
    {
        PlayerController playerController = other.gameObject.GetComponentInParent<PlayerController>();
        var shipRotation = playerController.transform.rotation;

        GameObject extraLaserGameObject1 = Instantiate(extraLaser, playerController.transform);
        extraLaserGameObject1.transform.localPosition = Vector3.zero + extraLaser1Offset;
        extraLaserGameObject1.transform.rotation = shipRotation;
        playerController.laserParticleSystems.Add(extraLaserGameObject1.GetComponent<ParticleSystem>());
        
        GameObject extraLaserGameObject2 = Instantiate(extraLaser, playerController.transform);
        extraLaserGameObject2.transform.localPosition = Vector3.zero + extraLaser2Offset;
        extraLaserGameObject2.transform.rotation = shipRotation;
        playerController.laserParticleSystems.Add(extraLaserGameObject2.GetComponent<ParticleSystem>());
    }
}