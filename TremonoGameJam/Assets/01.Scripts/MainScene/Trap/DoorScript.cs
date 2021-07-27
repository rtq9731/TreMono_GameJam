using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorScript : MonoBehaviour
{
    [SerializeField] GameObject doorObj;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.position.x > this.transform.position.x) // 오른쪽으로 지나갔음
        {
            doorObj.transform.DOMoveY(-8.25f, 0.3f);
            FindObjectOfType<BossRoomManager>().CallBoss();
            this.GetComponent<Collider2D>().enabled = false;
        }
    }
}
