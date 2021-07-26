using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour, IHitable
{
    [Header("적유닛 스탯관련")]
    [SerializeField]
    private int hp;

    [SerializeField]
    private int _ap;
    public int ap
    {
        get { return _ap; }
    }

    [SerializeField]
    private float _speed = 1f;
    public float speed
    {
        get { return _speed; }
    }

    public void Hit(int damage)
    {
        hp -= damage;
    }
}
