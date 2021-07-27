using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorScript : MonoBehaviour
{
    [SerializeField] GameObject doorObj;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.position.x > this.transform.position.x) // ���������� ��������
        {
            doorObj.transform.DOMoveY(0, 0.3f);
            FindObjectOfType<BossRoomManager>().CallBoss();
        }
    }
}
