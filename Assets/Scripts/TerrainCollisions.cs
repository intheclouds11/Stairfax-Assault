using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCollisions : MonoBehaviour
{
    [SerializeField] private GameObject terrainExplosionParticles;
    private Transform parentGameObjectTransform;
    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void Start()
    {
        parentGameObjectTransform = GameObject.FindWithTag("Spawn At Runtime").transform;
    }

    private void OnParticleCollision(GameObject other)
    {
        other.GetComponent<ParticleSystem>().GetCollisionEvents(this.gameObject, collisionEvents);

        foreach (ParticleCollisionEvent collisionEvent in collisionEvents)
        {
            Vector3 collisionHitLocation = collisionEvent.intersection;
            GameObject vfx = Instantiate(terrainExplosionParticles, collisionHitLocation, Quaternion.identity);
            vfx.transform.parent = parentGameObjectTransform;
        }
    }
}