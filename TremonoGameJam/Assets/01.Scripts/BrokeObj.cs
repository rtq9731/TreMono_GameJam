using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokeObj : MonoBehaviour, IHitable
{
    public enum brokeObjPos
    {
        Right,
        Center,
        Left
    };

    [SerializeField] brokeObjPos whereItis;
    [SerializeField] BrokeObj[] linkedObjs;
    [SerializeField] public bool isBreak;

    private void Update()
    {
        if (isBreak)
            Break();
    }

    public void Hit(int damage)
    {
        isBreak = true;
    }

    public void Break()
    {
        GameObject temp = null;
        switch (whereItis)
        {
            case brokeObjPos.Right:
                temp = Instantiate(StageManager.Instance.brokeObjsPrefab[0], gameObject.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                Destroy(temp, 2f);
                break;
            case brokeObjPos.Center:
                temp = Instantiate(StageManager.Instance.brokeObjsPrefab[1], gameObject.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                Destroy(temp, 2f);
                break;
            case brokeObjPos.Left:
                temp = Instantiate(StageManager.Instance.brokeObjsPrefab[2], gameObject.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                Destroy(temp, 2f);
                break;
            default:
                break;
        }

        for (int i = 0; i < linkedObjs.Length; i++)
        {
            linkedObjs[i].isBreak = true;
        }
    }

}
