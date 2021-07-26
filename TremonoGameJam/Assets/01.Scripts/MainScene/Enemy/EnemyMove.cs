using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : EnemyStatus
{
    private StageManager stagemanager = null;
    private EnemyStat enemyStat = null;

    private SpriteRenderer spriteRenderer = null;
    private Animator anim = null;

    private bool canAttack = true;
    private bool searchMove = true;

    private bool isDead = false;
    private bool isAttack = false;
    private bool isPursue = false;
    private bool isSearching = false;

    [SerializeField]
    private float searchRangeX = 1f;
    [Header("이 유닛이 공중유닛일 경우에만 적용되는 값")]
    [SerializeField]
    private float searchRangeY = 1f;
    [SerializeField]
    private float searchResetDelay = 1f;
    [SerializeField]
    private float searchResetDistance = 0.5f;

    private Vector2 currentPosition = Vector2.zero;
    private Vector2 playerPosition = Vector2.zero;
    private Vector2 searchTargetPosition = Vector2.zero;

    void Start()
    {
        stagemanager = FindObjectOfType<StageManager>();

        enemyStat = GetComponent<EnemyStat>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (!isDead)
        {
            if (enemyStat.currentStatus == Status.Attack)
            {
                isAttack = true;
                isPursue = false;
                isSearching = false;
            }
            else if (enemyStat.currentStatus == Status.Found)
            {
                isAttack = false;
                isPursue = true;
                isSearching = false;
            }
            else if (enemyStat.currentStatus == Status.Searching)
            {
                isAttack = false;
                isPursue = false;
                isSearching = true;
            }

            if (enemyStat.hp <= 0f)
            {
                isAttack = false;
                isPursue = false;
                isSearching = false;
                isDead = true;

                Dead();
            }
        }
    }
    private void FixedUpdate()
    {
        currentPosition = transform.position;
        playerPosition = stagemanager.playerTrm.position;

        Pursue();
        Attack();
        Searching();

        transform.position = currentPosition;
    }
    private void FlipCheck(Vector2 targetPosition)
    {
        spriteRenderer.flipX = enemyStat.searchPlayer.CheckFlip(targetPosition);
    }
    private void Pursue()
    {
        if (isPursue)
        {
            anim.Play("Move");
            currentPosition = Vector2.MoveTowards(currentPosition, playerPosition, enemyStat.pursueSpeed * Time.fixedDeltaTime);
            FlipCheck(playerPosition);
        }
    }
    private void Attack()
    {
        if (canAttack && isAttack)
        {
            canAttack = false;
            anim.Play("Attack");
            FlipCheck(playerPosition);
            Invoke("AttackRe", enemyStat.attackDelay);
        }
    }
    private void AttackRe()
    {
        canAttack = true;
    }
    private void Searching()
    {
        float distance;
        if (isSearching)
        {
            if (searchMove)
            {
                anim.Play("Move");
                currentPosition = Vector2.MoveTowards(currentPosition, searchTargetPosition, enemyStat.searchSpeed * Time.fixedDeltaTime);

                if (enemyStat.isAirEnemy)
                {
                    distance = Vector2.Distance(currentPosition, searchTargetPosition);
                }
                else
                {
                    Vector2 _currentPosition = currentPosition;
                    _currentPosition.y = searchTargetPosition.y;

                    distance = Vector2.Distance(_currentPosition, searchTargetPosition);
                }

                if (distance <= searchResetDistance)
                {
                    searchMove = false;
                    SearchPositionSet();
                    Invoke("SearMoveReset", searchResetDelay);
                }
            }
            else
            {
                anim.Play("Idle");
            }

            FlipCheck(searchTargetPosition);
        }
    }
    private void SearchPositionSet()
    {
        searchTargetPosition = currentPosition;
        float _searchX = Random.Range(-searchRangeX, searchRangeX);

        searchTargetPosition.x += _searchX;

        if (enemyStat.isAirEnemy)
        {
            float _searchY = Random.Range(-searchRangeY, searchRangeY);

            searchTargetPosition.y += _searchY;
        }
    }
    private void SearMoveReset()
    {
        searchMove = true;
    }
    private void Dead()
    {
        anim.Play("Dead");
    }
    public void Destroye()
    {
        gameObject.SetActive(false);
    }
}
