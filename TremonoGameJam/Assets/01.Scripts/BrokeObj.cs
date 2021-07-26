using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokeObj : MonoBehaviour
{
    public enum brokeObjPos
    {
        Right,
        Center,
        Left
    };

    [SerializeField] brokeObjPos whereItis;
    [SerializeField] BrokeObj[] linkedObjs;
    [SerializeField] public bool isBroke;

    private void Update()
    {
        if (isBroke)
            Break();
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
            linkedObjs[i].isBroke = true;
        }
    }

}
