using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    [SerializeField] BossScene boss;
    [SerializeField] GameObject vcamBig;
    [SerializeField] GameObject vcamSmall;

    GameObject player = null;

    private void Start()
    {
        player = FindObjectOfType<PlayerStat>().gameObject;
    }

    public void CallBoss()
    {
        Debug.Log(UIManager.Instance.gameObject.name);
        UIManager.Instance.CallBossIntro(boss.gameObject);
        vcamBig.SetActive(true);
        vcamSmall.SetActive(false);
        StageManager.Instance.StopPlayer(2.5f);
    }

}
