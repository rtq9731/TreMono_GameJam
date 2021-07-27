using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField]
    private float destroyTime = 3f;
    private ParticleSpawn particleSpawn = null;

    public IEnumerator SpawnSet(Vector2 spawnPosition, ParticleSpawn ps)
    {
        transform.position = spawnPosition;
        particleSpawn = ps;

        yield return new WaitForSeconds(destroyTime);

        Disable();
    }
    void Disable()
    {
        particleSpawn.DisableParticle(gameObject);
    }

}
