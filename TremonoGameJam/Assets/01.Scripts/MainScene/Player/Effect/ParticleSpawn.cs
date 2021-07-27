using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawn : MonoBehaviour
{
    [SerializeField]
    private Transform particleSpawnTrm = null;
    private List<GameObject> particles = new List<GameObject>();

    public void CallParticle(GameObject Particle, Vector2 ParticleSpawnPosition)
    {
        if (particles.Count <= 0f)
        {
            GameObject nParticle = Instantiate(Particle, particleSpawnTrm);
            Particle particle = nParticle.GetComponent<Particle>();

            StartCoroutine(particle.SpawnSet(ParticleSpawnPosition, this));
        }
        else
        {
            bool nParticleSet = false;
            GameObject nParticle = null;

            foreach (var item in particles)
            {
                if (item.name == Particle.name)
                {
                    nParticle = item;
                    nParticleSet = true;
                }
            }

            if (!nParticleSet)
            {
                nParticle = Instantiate(Particle, particleSpawnTrm);
            }

            Particle particle = nParticle.GetComponent<Particle>();

            StartCoroutine(particle.SpawnSet(ParticleSpawnPosition, this));
            particles.Remove(nParticle);
        }
    }
    public void DisableParticle(GameObject Particle)
    {
        particles.Add(Particle);
        Particle.SetActive(false);
    }
}
