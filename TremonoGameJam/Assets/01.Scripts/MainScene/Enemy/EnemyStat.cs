using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : EnemyStatus, IHitable
{
    private StageManager stageManager = null;
    private SearchPlayer _searchPlayer = null;
    public SearchPlayer searchPlayer
    {
        get { return _searchPlayer; }
    }

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


    public void Hit(int damage)
    {
        _hp -= damage;

        _isHurt = true;
    }
}
