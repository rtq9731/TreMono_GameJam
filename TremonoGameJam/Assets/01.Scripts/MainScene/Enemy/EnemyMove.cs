using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : EnemyStatus
{
    private StageManager stagemanager = null;
    private EnemyStat enemyStat = null;
    public ParticleSpawn particleSpawn { get; private set; }

    private SpriteRenderer spriteRenderer = null;
    private Animator anim = null;

    private bool canAttack = true;
    private bool canAttackStart = false;
    private bool searchMove = true;

    [Header("발사체를 사용하는가")]
    [SerializeField]
    private bool isUseProjectTile = false;
    [Header("자폭형 유닛인가")]
    [SerializeField]
    private bool isBoomer = false;

    [SerializeField]
    private GameObject attackSound = null;

    [Header("발사체")]
    [SerializeField]
    private GameObject projectTile = null;
    [SerializeField]
    private GameObject _onhitParticle = null;
    public GameObject onhitParticle
    {
        get { return _onhitParticle; }
    }

    [Header("발사체들의 부모오브젝트")]
    [SerializeField]
    private Transform projectSpawnPosition = null;
    [SerializeField]
    private List<GameObject> _projectTiles;
    public List<GameObject> projectTiles
    {
        get { return _projectTiles; }
        set { _projectTiles = value; }
    }

    private bool isDead = false;
    private bool isHurt = false;
    private bool isAttack = false;
    private bool attacking = false;
    private bool isPursue = false;
    private bool isSearching = false;
    private bool isBoom = false; // 자폭
    private bool _moveByPlayerSkill = false;
    public bool moveBYPlayerSkill
    {
        get { return _moveByPlayerSkill; }
    }

    private bool attackAnimIsPlaying = false;

    [SerializeField]
    private float searchRangeX = 1f;
    [Header("이 유닛이 공중유닛일 경우에만 적용되는 값")]
    [SerializeField]
    private float searchRangeY = 1f;
    [SerializeField]
    private float searchResetDelay = 1f;
    [SerializeField]
    private float searchResetDistance = 0.5f;

    [SerializeField]
    private LayerMask whatIsAttackable;
    [Header("DAMAGABLE포함")]
    [SerializeField]
    private LayerMask whatIsGround;

    [Header("뒤집을 때 보정값")]
    [SerializeField] // 오른쪽 바라보면 True 아니면 False
    private bool isTrueAngle = true;

    private Vector2 currentPosition = Vector2.zero;
    private Vector2 playerPosition = Vector2.zero;
    private Vector2 searchTargetPosition = Vector2.zero;
    private Vector2 moveByPlayerPosition = Vector2.zero;

    void Start()
    {
        stagemanager = FindObjectOfType<StageManager>();
        particleSpawn = FindObjectOfType<ParticleSpawn>();

        enemyStat = GetComponent<EnemyStat>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        currentPosition = transform.position;
        SearchPositionSet();
    }
    void Update()
    {
        isHurt = enemyStat.isHurt;

        if (!isDead && !isHurt)
        {
            if (enemyStat.currentStatus == Status.Attack)
            {
                isAttack = true;
                canAttackStart = true;
                isPursue = false;
                isSearching = false;
            }
            else if (enemyStat.currentStatus == Status.Found)
            {
                isAttack = false;
                isPursue = true;
                isSearching = false;
                attackAnimIsPlaying = false;
            }
            else if (enemyStat.currentStatus == Status.Searching)
            {
                isAttack = false;
                isPursue = false;
                isSearching = true;
                attackAnimIsPlaying = false;
            }
        }
        else if (isHurt && enemyStat.hp > 0f && !attackAnimIsPlaying)
        {
            anim.Play("Hurt");
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

    private void FixedUpdate()
    {
        currentPosition = transform.position;
        playerPosition = stagemanager.playerTrm.position;

        if (!isDead && !isHurt)
        {
            if (!moveBYPlayerSkill)
            {
                if (isBoomer)
                {
                    if (isBoom)
                    {
                        AttackCheck();
                    }
                    else if(!attackAnimIsPlaying)
                    {
                        Pursue();
                        Attack();
                        Searching();
                    }
                }
                else if(!attackAnimIsPlaying)
                {
                    AttackCheck();
                    Pursue();
                    Attack();
                    Searching();
                }
            }

            MoveBYPlayerSkill1();
        }

        transform.position = currentPosition;
    }
    private void PlayAttackSound()
    {
        Instantiate(attackSound, transform);
    }
    private void isBoomSet()
    {
        isBoom = true;
    }
    private void AttackCheck()
    {
        if (attacking)
        {
            float distance = Vector2.Distance(playerPosition, currentPosition);

            attacking = false;
            Vector2 dir = (playerPosition - (Vector2)transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, enemyStat.attackRange, whatIsAttackable);

            if (isUseProjectTile)
            {
                if (hit)
                {
                    if (projectTiles.Count <= 0f)
                    {
                        GameObject shootIt = projectTile;

                        ProjectileScript projectileScript = Instantiate(shootIt, stagemanager.enemys).GetComponent<ProjectileScript>();
                        projectileScript.enemyMove = this;
                        projectileScript.flipX = spriteRenderer.flipX;
                        projectileScript.SetSpawn(this, projectSpawnPosition.position, enemyStat.attackRange, enemyStat.ap);

                    }
                    else
                    {
                        GameObject shootIt = projectTiles[0];
                        ProjectileScript projectileScript = shootIt.GetComponent<ProjectileScript>();
                        projectileScript.flipX = spriteRenderer.flipX;
                        projectileScript.SetSpawn(this, projectSpawnPosition.position, enemyStat.attackRange, enemyStat.ap);

                        shootIt.SetActive(true);
                        projectTiles.Remove(shootIt);
                    }
                }
            }
            else
            {
                if (hit && distance <= enemyStat.attackRange)
                {
                    PlayerStat playerStat = hit.transform.GetComponent<PlayerStat>();
                    particleSpawn.CallParticle(_onhitParticle, hit.point);

                    playerStat.Hit(enemyStat.ap);
                    playerStat.SetTargetPosition(currentPosition);
                }
            }
        }
    }
    //각각 Attack.anim의 양 끝부분에 넣을것
    private void SetTrueAttackAnimIsPlaying()
    {
        attackAnimIsPlaying = true;
    }
    private void SetFlaseAttackAnimIsPlaying()
    {
        attackAnimIsPlaying = false;
        IsHurtReset();
    }

    private void IsHurtReset()
    {
        enemyStat.isHurt = false;
    }
    private void FlipCheck(Vector2 targetPosition)
    {
        if (isTrueAngle)
            spriteRenderer.flipX = enemyStat.searchPlayer.CheckFlip(targetPosition);
        else
            spriteRenderer.flipX = !enemyStat.searchPlayer.CheckFlip(targetPosition);
    }
    private void Pursue()
    {
        if (isPursue && !attackAnimIsPlaying)
        {
            anim.Play("Move");
            currentPosition = Vector2.MoveTowards(currentPosition, playerPosition, enemyStat.pursueSpeed * Time.fixedDeltaTime);
            FlipCheck(playerPosition);
        }
    }
    private void Attack()
    {


        if (canAttackStart && canAttack)
        {
            attacking = true;
            canAttack = false;

            anim.Play("Attack");
            FlipCheck(playerPosition);
            Invoke("AttackRe", enemyStat.attackDelay);
        }
        else
        {
            if (isAttack && canAttackStart)
            {
                canAttackStart = false;
            }
        }
    }
    public void SetMoveByPlayerSkill(Vector2 targetPosition)
    {
        _moveByPlayerSkill = true;
        moveByPlayerPosition = targetPosition;
    }
    private void MoveBYPlayerSkill1()
    {
        if (_moveByPlayerSkill)
        {
            moveByPlayerPosition.y = Mathf.Clamp(moveByPlayerPosition.y, currentPosition.y, currentPosition.y + 0.4f);
            currentPosition = Vector2.MoveTowards(currentPosition, moveByPlayerPosition, 10f * Time.fixedDeltaTime);

            float distance = Vector2.Distance(currentPosition, moveByPlayerPosition);

            RaycastHit2D hit = CheckRayHit(moveByPlayerPosition);

            if (hit)
            {
                _moveByPlayerSkill = false;
            }

            Debug.DrawRay(transform.position, hit.point - currentPosition, new Color(0, 1, 0));

            if (distance <= 0.5f)
            {
                _moveByPlayerSkill = false;
            }
        }
    }
    private RaycastHit2D CheckRayHit(Vector2 targetPosition)
    {
        Vector2 position = targetPosition - currentPosition;

        Ray2D ray = new Ray2D(transform.position, position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1f, whatIsGround);

        return hit;
    }
    private void AttackingReset()
    {
        attacking = false;
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
            if (searchMove && !attackAnimIsPlaying)
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
