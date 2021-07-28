using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel_Boom : MonoBehaviour, IHitable
{
    private ParticleSpawn particleSpawn = null;

    [SerializeField]
    private GameObject boomParticle = null;
    [SerializeField]
    private Transform particleSpawnTrm = null;

    private StageManager stagemanager = null;
    private PlayerStat playerStat = null;
    [SerializeField]
    private GameObject boomSound = null;
    [SerializeField]
    private int dm = 1;
    void Start()
    {
        stagemanager = FindObjectOfType<StageManager>();
        particleSpawn = GetComponent<ParticleSpawn>();
        playerStat = stagemanager.playerTrm.GetComponent<PlayerStat>();
    }
    public void Hit(int damage)
    {
        playerStat.HitByBarrel(dm);
        playerStat.SetTargetPosition(transform.position);

        particleSpawn.CallParticle(boomParticle, particleSpawnTrm.position);

        Instantiate(boomSound, Vector3.zero, Quaternion.identity);

        gameObject.SetActive(false);
    }
}
