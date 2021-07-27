using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerStat : MonoBehaviour, IHitable
{
    private PlayerMove playerMove = null;

    [SerializeField]
    private LayerMask whatIsGround;

    [Header("Player스탯 관련")]
    [SerializeField]
    private int _hp = 3;
    public int hp
    {
        get { return _hp; }
        set
        {
            _hp = value;
            UIManager.Instance.HPChange(value);
        }
    }
    public int firstHp { get; private set; }
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

    [SerializeField]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
    }
    [SerializeField]
    private bool _isHurt = false;
    public bool isHurt
    {
        get { return _isHurt; }
        set { _isHurt = value; }
    }

    [SerializeField]
    private float hitMoveRange = 0.5f;
    private bool isHurtMove = false;
    private Vector2 currentPosition = Vector2.zero;
    private Vector2 targetPosition = Vector2.zero;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        firstHp = hp;
    }

    private void FixedUpdate()
    {
        currentPosition = transform.position;

        Dead();
        HitMove();

        transform.position = currentPosition;
    }

    private void HitMove()
    {

        if (isHurt && isHurtMove)
        {
            transform.DOMove(targetPosition, 0.1f).SetEase(Ease.InQuad);
        }
        else if (!isHurt)
        {
            isHurtMove = false;
        }

    }
    public void SetTargetPosition(Vector2 attackerPosition)
    {
        bool moveLeft = false;
        targetPosition = currentPosition;

        Vector2 endPosition = currentPosition;
        targetPosition = currentPosition;

        if (currentPosition.x >= attackerPosition.x)
        {
            endPosition.x += hitMoveRange;
            moveLeft = false;
        }
        else
        {
            endPosition.x -= hitMoveRange;
            moveLeft = true;
        }

        bool a = false;
        bool b = false;

        do
        {
            a = Physics2D.OverlapCircle(targetPosition, 0.1f, whatIsGround);
            if (!a)
            {
                if (moveLeft)
                {
                    targetPosition.x -= 0.1f;
                }
                else
                {
                    targetPosition.x += 0.1f;
                }
            }

            if (moveLeft)
            {
                if (targetPosition.x <= endPosition.x)
                {
                    b = true;
                }
            }
            else
            {
                if (targetPosition.x >= endPosition.x)
                {
                    b = true;
                }
            }
        } while (!a && !b);

        isHurtMove = true;
    }
    public void Hit(int damage)
    {
        if (!playerMove.dashMoving)
        {
            hp -= damage;
            isHurt = true;
        }
    }
    public void HitByBarrel(int damage)
    {
        hp -= damage;
        isHurt = true;
    }
    public void Heal(int heal)
    {
        hp += heal;
    }

    public void Dead()
    {
        if (hp <= 0)
        {
            _isDead = true;
            // 죽음애니메이션
        }
    }

}
