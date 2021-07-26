using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Player스탯 관련")]
    [SerializeField]
    private int _hp = 3;
    public int hp
    {
        get { return _hp; }
        set { 
            _hp = value;
            UIManager.Instance.HPChange(value);
        }
    }
    [SerializeField]
    private int _ap = 3;
    public int ap
    {
        get { return _ap; }
        set { _ap = value; }
    }

    [SerializeField]
    private float _speed = 1f;
    public float speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    [SerializeField]
    private float _jumpSpeed = 10f;
    public float jumpSpeed
    {
        get { return _jumpSpeed; }
    }
    [SerializeField]
    private float _dashRange = 10f;
    public float dashRange
    {
        get { return _dashRange; }
    }

    public void Dead()
    {
        Debug.Log("Dead -.-");
    }

}
