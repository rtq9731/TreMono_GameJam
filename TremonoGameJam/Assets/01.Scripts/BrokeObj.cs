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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(whereItis.GetType());
    }

    public void Break()
    {
        switch (whereItis)
        {
            case brokeObjPos.Right:
                break;
            case brokeObjPos.Center:
                break;
            case brokeObjPos.Left:
                break;
            default:
                break;
        }
    }

}
