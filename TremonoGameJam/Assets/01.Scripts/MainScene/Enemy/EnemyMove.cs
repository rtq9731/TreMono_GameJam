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
    private bool canAttackStart = false;
    private bool attackDone = false;
    private bool searchMove = true;

    [Header("발사체를 사용하는가")]
    [SerializeField]
    private bool isUseProjectTile = false;

    [Header("발사체")]
    [SerializeField]
    private GameObject projectTile = null;

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

    private Vector2 currentPosition = Vector2.zero;
    private Vector2 playerPosition = Vector2.zero;
    private Vector2 searchTargetPosition = Vector2.zero;

    private int attackNum = 0;

    void Start()
    {
        stagemanager = FindObjectOfType<StageManager>();

        enemyStat = GetComponent<EnemyStat>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
        else if (isHurt)
        {
            anim.Play("Hurt");
        }
    }

    private void FixedUpdate()
    {
        currentPosition = transform.position;
        playerPosition = stagemanager.playerTrm.position;

        if (!isDead && !isHurt)
        {
            Pursue();
            Attack();
            AttackCheck();
            Searching();
        }

        transform.position = currentPosition;
    }
    private void AttackCheck()
    {
        if (attacking)
        {
            attacking = false;
            if (spriteRenderer.flipX)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, enemyStat.attackRange, whatIsAttackable);

                if (isUseProjectTile)
                {
                    if (hit)
                    {
                        if (projectTiles.Count <= 0f)
                        {
                            GameObject shootIt = projectTile;

                            ProjectileScript projectileScript = Instantiate(shootIt, projectSpawnPosition).GetComponent<ProjectileScript>();
                            projectileScript.enemyMove = this;
                            projectileScript.flipX = spriteRenderer.flipX;
                            projectileScript.SetSpawn(projectSpawnPosition.position, enemyStat.attackRange);

                        }
                        else
                        {
                            GameObject shootIt = projectTiles[0];
                            ProjectileScript projectileScript = shootIt.GetComponent<ProjectileScript>();
                            projectileScript.flipX = spriteRenderer.flipX;
                            projectileScript.SetSpawn(projectSpawnPosition.position, enemyStat.attackRange);

                            shootIt.SetActive(true);
                            projectTiles.Remove(shootIt);
                        }
                    }
                }
                else
                {
                    if (hit)
                    {
                        hit.transform.GetComponent<PlayerStat>().Hit(1);
                    }
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, enemyStat.attackRange, whatIsAttackable);

                if (hit)
                {
                    if (projectTiles.Count <= 0f)
                    {
                        GameObject shootIt = projectTile;

                        ProjectileScript projectileScript = Instantiate(shootIt, projectSpawnPosition).GetComponent<ProjectileScript>();
                        projectileScript.enemyMove = this;
                        projectileScript.flipX = spriteRenderer.flipX;
                        projectileScript.SetSpawn(projectSpawnPosition.position, enemyStat.attackRange);

                    }
                    else
                    {
                        GameObject shootIt = projectTiles[0];
                        ProjectileScript projectileScript = shootIt.GetComponent<ProjectileScript>();
                        projectileScript.flipX = spriteRenderer.flipX;
                        projectileScript.SetSpawn(projectSpawnPosition.position, enemyStat.attackRange);

                        shootIt.SetActive(true);
                        projectTiles.Remove(shootIt);
                    }
                }
                else
                {
                    if (hit)
                    {
                        hit.transform.GetComponent<PlayerStat>().Hit(1);
                    }
                }

            }
        }
    }
    private void IsHurtReset()
    {
        enemyStat.isHurt = false;
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
        if (!isAttack)
        {
            attackDone = false;
            attackNum = 0;
        }

        if(isAttack && attackNum >= enemyStat.attackNum)
        {
            attackDone = true;
        }

        if (canAttackStart && canAttack && !attackDone)
        {
            attacking = true;
            canAttack = false;

            anim.Play("Attack");
            FlipCheck(playerPosition);
            Invoke("AttackRe", enemyStat.attackDelay);
        }
        else if (attackDone)
        {
            if (isAttack && canAttackStart)
            {
                canAttackStart = false;
                Invoke("AttackNumSet", enemyStat.attackTum);
            }
        }
    }
    private void AttackRe()
    {
        attackNum++;
        canAttack = true;
    }
    private void AttackNumSet()
    {
        attackNum = 0;
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
