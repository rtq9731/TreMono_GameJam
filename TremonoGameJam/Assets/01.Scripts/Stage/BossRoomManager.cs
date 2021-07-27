using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    [SerializeField] BossScene boss;

    GameObject player = null;

    private void Start()
    {
        player = FindObjectOfType<PlayerStat>().gameObject;
    }

    public void CallBoss()
    {

        boss.gameObject.SetActive(true);
    }

}
