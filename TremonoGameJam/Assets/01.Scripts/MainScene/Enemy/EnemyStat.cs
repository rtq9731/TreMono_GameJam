using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyStat : EnemyStatus, IHitable
{
    private StageManager stageManager = null;
    private SearchPlayer _searchPlayer = null;
    public SearchPlayer searchPlayer
    {
        get { return _searchPlayer; }
    }

    [SerializeField]
    private LayerMask whatIsGround;

    private Animator anim = null;

    [Header("공중유닛인가?")]
    [SerializeField]
    private bool _isAirEnemy = false;
    public bool isAirEnemy
    {
        get { return _isAirEnemy; }
    }
    [SerializeField]
    private bool _isHurt = false;
    public bool isHurt
    {
        get { return _isHurt; }
        set { _isHurt = value; }
    }
    [Header("적유닛 스탯관련")]
    [SerializeField]
    private int _hp;
    public int hp
    {
        get { return _hp; }
    }

    [SerializeField]
    private int _ap;
    public int ap
    {
        get { return _ap; }
    }
    [SerializeField]
    private float _attackDelay = 1f;
    public float attackDelay
    {
        get { return _attackDelay; }
    }
    [SerializeField]
    private int _attackNum = 6;
    public int attackNum
    {
        get { return _attackNum; }
    }

    [SerializeField]
    private float _searchSpeed = 1f;
    public float searchSpeed
    {
        get { return _searchSpeed; }
    }
    [SerializeField]
    private float _pursueSpeed = 3f;
    public float pursueSpeed
    {
        get { return _pursueSpeed; }
    }

    [Header("스테이터스 변경 관련 함수")]
    [SerializeField]
    private float _foundRange = 5f;
    public float foundRange
    {
        get { return _foundRange; }
    }
    [SerializeField]
    private float _attackRange = 1f;
    public float attackRange
    {
        get { return _attackRange; }
    }

    [SerializeField]
    private Status _currentStatus;
    public Status currentStatus
    {
        get { return _currentStatus; }
        set { _currentStatus = value; }
    }

    [Header("스테이터스 변경을 위한 플레이어 포지션값")]
    [SerializeField]
    private Transform _playerPosition = null;
    public Transform playerPosition
    {
        get { return _playerPosition; }
    }

    [SerializeField]
    private float hitMoveRange = 0.5f;
    private bool isHurtMove = false;
    private Vector2 currentPosition = Vector2.zero;
    private Vector2 targetPosition = Vector2.zero;

    private void Start()
    {
        _searchPlayer = GetComponent<SearchPlayer>();
        stageManager = FindObjectOfType<StageManager>();

        _playerPosition = stageManager.playerTrm;
    }
    void Update()
    {
        currentStatus = searchPlayer.CheckStatus(playerPosition.position, foundRange, attackRange);

    }
    private void FixedUpdate()
    {
        currentPosition = transform.position;

        HitMove();

        transform.position = currentPosition;
    }
    private void HitMove()
    {
        bool a = Physics2D.OverlapCircle(currentPosition, 1f, whatIsGround);

        if (!a)
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
        else
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
        _hp -= damage;

        _isHurt = true;
    }
}
