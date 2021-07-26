using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Player스탯 관련")]
    [SerializeField]
    private float _hp = 3f;
    public float hp
    {
        get { return _hp; }
        set { _hp = value; }
    }
    [SerializeField]
    private float _ap = 3f;
    public float ap
    {
        get { return _ap; }
        set { _ap = value; }
    }
    [SerializeField]
    private float _dp = 3f;
    public float dp
    {
        get { return _dp; }
        set { _dp = value; }
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

    void Start()
    {

    }

}
