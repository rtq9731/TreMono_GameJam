using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObjForAttack : MonoBehaviour
{
    public GameObject brokenObj = null;

    private void OnEnable()
    {
        Invoke("Break()", 5);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("DMAGABLE") && collision.gameObject.layer != LayerMask.NameToLayer("BreakAble"))
        {
            Break();
        }
    }

    public void Break()
    {
        CancelInvoke();
        GameObject temp = Instantiate(brokenObj,transform.position,Quaternion.identity);
        Destroy(temp, 3);
        Destroy(gameObject);
    }
}
