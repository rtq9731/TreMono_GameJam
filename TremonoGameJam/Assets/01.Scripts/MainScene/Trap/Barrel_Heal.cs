using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel_Heal : MonoBehaviour, IHitable
{
    [SerializeField]
    private GameObject boomParticle = null;
    [SerializeField]
    private Transform particleSpawnTrm = null;
    private ParticleSpawn particleSpawn = null;
    private StageManager stagemanager = null;
    private PlayerStat playerStat = null;
    [SerializeField]
    private int healNum = 1;
    void Start()
    {
        stagemanager = FindObjectOfType<StageManager>();
        playerStat = stagemanager.playerTrm.GetComponent<PlayerStat>();
        particleSpawn = GetComponent<ParticleSpawn>();
    }
    public void Hit(int damage)
    {
        if (playerStat.hp < playerStat.firstHp)
        {
            playerStat.Heal(healNum);
        }
        
        particleSpawn.CallParticle(boomParticle, particleSpawnTrm.position);
        gameObject.SetActive(false);
    }

}
