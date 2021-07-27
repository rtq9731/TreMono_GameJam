using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    private StageManager stageManager = null;
    private PlayerInput playerInput = null;
    private PlayerStat playerStat = null;
    private SpawnAfterImage spawnAfterImage = null;
    private ParticleSpawn particleSpawn = null;

    [SerializeField]
    private GameObject skill1Object = null;

    private Rigidbody2D rigid = null;
    private Animator anim = null;
    public SpriteRenderer spriteRenderer { get; private set; }


    [SerializeField]
    private Transform groundChecker = null;

    [SerializeField]
    private LayerMask whatIsAttackable;
    [SerializeField]
    private GameObject onHitParticle;
    [SerializeField]
    private GameObject wallDestroyParticle;

    [SerializeField]
    private LayerMask WhatIsGround;

    [Header("플레이어가 닿으면 데미지받는 오브젝트")]
    [SerializeField]
    private LayerMask WhatIsDamagable;

    [SerializeField]
    private float damagableRange = 1f;
    private bool canHurtByDamagable = true;

    [SerializeField]
    private float groundCheckDistance = 1f;

    [SerializeField]
    private float attackDelay = 1f;
    [SerializeField]
    private float skill1Delay = 5f;
    [SerializeField]
    private float dashDoTime = 1f;

    [SerializeField]
    private float dashStopRange = 0.5f;

    [SerializeField]
    private int inAirDashCount = 1;
    private int firstInAirDashCount = 0;

    private float XMove = 0f;

    private bool isGround = false;

    private bool isJump = false;
    private bool isAttack = false;
    private bool skill1 = false;
    private bool isAttacked = false; // 이미 공격 했는지

    private bool jumpAnimIsPlaying = false;
    private bool skill1AnimIsPlaying = false;
    private bool canAttack = true;
    private bool canSkill1 = true;
    private bool canAttackReStarted = false;
    private bool canDoubleJump = false;
    private bool canSpawnAfterImage = true;

    private bool _dashMoving = false;
    public bool dashMoving
    {
        get { return _dashMoving; }
    }
    private bool attacking = false;

    private bool whenDashStopMove = false; // 대쉬가 멈췄을 때 이동하는 것에 관한 변수
    private bool whenDashStopMoveSetStarted = false;

    private Vector2 dashPosition = Vector2.zero;
    private Vector2 mousePosition = Vector2.zero;
    public Vector2 currentPosition { get; private set; }

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();

        playerInput = GetComponent<PlayerInput>();
        playerStat = GetComponent<PlayerStat>();
        spawnAfterImage = GetComponent<SpawnAfterImage>();
        particleSpawn = GetComponent<ParticleSpawn>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        firstInAirDashCount = inAirDashCount;
    }

    void Update()
    {
        if (playerInput.isJump)
        {
            isJump = true;
        }

        if (playerInput.isAttack && canAttack)
        {
            canAttackReStarted = false;
            isAttack = true;
        }

        if (playerInput.isSkill1)
        {
            if (canSkill1)
            {
                skill1 = true;
            }
        }

        GroundCheck();

    }
    void FixedUpdate()
    {
        currentPosition = transform.position;

        XMove = playerInput.XMove;

        if (!playerStat.isHurt)
        {
            if (!playerStat.isDead && !skill1AnimIsPlaying)
            {

                LRCheck();

                Move();
                Attack();
                Jump();
                Skill1();

                AttackCheck();
                DashMove();
                WhenDashStopMove();
                SpawnAfterImage();
                DamagableCheck();
            }
            else if (playerStat.isDead)
            {
                anim.Play("Dead");
            }
        }
        else
        {
            anim.Play("Hurt");
            skill1AnimIsPlaying = false;
        }

        transform.position = currentPosition;
    }
    private void IsHurtReset()
    {
        playerStat.isHurt = false;
    }
    private void DamagableCheck()
    {
        if (canHurtByDamagable)
        {
            Collider2D a = Physics2D.OverlapCircle(currentPosition, damagableRange, WhatIsDamagable);
            
            if (a)
            {
                canHurtByDamagable = false;
                playerStat.Hit(1);
                playerStat.SetTargetPosition(a.transform.position);
                Invoke("CanHurtByDamagableReset", 1f);
            }
        }
    }
    private void CanHurtByDamagableReset()
    {
        canHurtByDamagable = true;
    }
    private void Destroye()
    {
        transform.parent.gameObject.SetActive(false);
    }
    private void SetTrueSkill1AnimIsPlaying()
    {
        skill1AnimIsPlaying = true;
    }
    private void SetFalseSkill1AnimIsPlaying()
    {
        skill1AnimIsPlaying = false;
    }
    private void SetTrueJumpAnimIsPlaying()
    {
        jumpAnimIsPlaying = true;
    }

    private void SetFlaseJumpAnimIsPlaying()
    {
        jumpAnimIsPlaying = false;
    }
    private void Skill1()
    {
        if (skill1 && canSkill1)
        {
            canSkill1 = false;
            skill1Object.SetActive(true);
            skill1Object.GetComponent<Skill1Script>().SetSpawn(currentPosition);
            anim.Play("ShorkWave");
            skill1 = false;
            Invoke("CanSkill1Set", skill1Delay);
        }
    }
    private void CanSkill1Set()
    {
        canSkill1 = true;
    }
    private void AttackCheck()
    {
        if (attacking && !isAttacked)
        {
            if (spriteRenderer.flipX)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 1, whatIsAttackable);

                if (hit)
                {
                    if (hit.transform.GetComponent<EnemyStat>() != null)
                    {
                        EnemyStat enemyStat = hit.transform.GetComponent<EnemyStat>();
                        enemyStat.Hit(1);
                        enemyStat.SetTargetPosition(currentPosition);

                        particleSpawn.CallParticle(onHitParticle, hit.point);
                    }
                    else
                    {
                        if (hit.transform.GetComponent<BrokeObj>() != null)
                        {
                            hit.transform.GetComponent<BrokeObj>().Hit(1);
                            particleSpawn.CallParticle(wallDestroyParticle, hit.point);
                        }
                        else
                        {
                            if (hit.transform.GetComponent<BossScene>() != null)
                            {
                                hit.transform.GetComponent<BossScene>().Hit(1);
                            }
                        }
                    }
                    isAttacked = true;
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1, whatIsAttackable);
                if (hit)
                {
                    if (hit.transform.GetComponent<EnemyStat>() != null)
                    {
                        EnemyStat enemyStat = hit.transform.GetComponent<EnemyStat>();
                        enemyStat.Hit(1);
                        enemyStat.SetTargetPosition(currentPosition);

                        particleSpawn.CallParticle(onHitParticle, hit.point);

                    }
                    else
                    {
                        if (hit.transform.GetComponent<BrokeObj>() != null)
                        {
                            hit.transform.GetComponent<BrokeObj>().Hit(1);
                            particleSpawn.CallParticle(wallDestroyParticle, hit.point);

                        }
                        else
                        {
                            if (hit.transform.GetComponent<BossScene>() != null)
                            {
                                hit.transform.GetComponent<BossScene>().Hit(1);
                            }
                        }
                    }

                    isAttacked = true;
                }
            }
        }
    }
    private int GetLayer(LayerMask a)
    {
        int b = a;
        int result = 0;

        while (true)
        {
            if (b >= 2)
            {
                result++;
                b = b / 2;
            }
            else
            {
                break;
            }
        }

        return result;
    }
    private void SpawnAfterImage()
    {
        if (dashMoving && canSpawnAfterImage)
        {
            float spawnAfterImageDelay = Random.Range(spawnAfterImage.spawnAfterImageDelayMinimum, spawnAfterImage.spawnAfterImageDelayMaximum);
            spawnAfterImage.SetAfterImage();
            canSpawnAfterImage = false;

            Invoke("SpawnAfterImageRe", spawnAfterImageDelay);
        }
    }
    private void SpawnAfterImageRe()
    {
        canSpawnAfterImage = true;
    }
    private void GroundCheck()
    {
        bool a = Physics2D.OverlapCircle(groundChecker.position, groundCheckDistance, WhatIsGround);
        isGround = a;

        if (a)
        {
            inAirDashCount = firstInAirDashCount;
            canDoubleJump = false;
            SetFlaseJumpAnimIsPlaying();
            WhenDashStopMoveSet();
        }

    }
    private void Attack()
    {
        if (isAttack && canAttack)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            isAttack = false;
            canAttack = false;
            attacking = true;

            SetFlaseJumpAnimIsPlaying();
            anim.Play("Attack");
            Dash();

            if (!canAttackReStarted)
            {
                Invoke("CanAttackReStarted", attackDelay);
                canAttackReStarted = true;
            }
        }
    }
    private void CanAttackReStarted()
    {
        canAttack = true;
        isAttacked = false;
        attacking = false;
    }
    public void ReAttacking()
    {
        attacking = false;
    }
    private void Dash()
    {
        if (mousePosition.x >= currentPosition.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        if (inAirDashCount > 0)
        {
            if (!isGround)
            {
                inAirDashCount--;
            }

            float _dashRange = playerStat.dashRange;
            dashPosition = currentPosition;
            Vector2 endPosition = currentPosition;

            if (spriteRenderer.flipX)
            {
                _dashRange = -_dashRange;
            }

            endPosition.x = currentPosition.x + _dashRange;

            bool a = false;
            bool b = false;
            do
            {
                a = Physics2D.OverlapCircle(dashPosition, 0.1f, WhatIsGround);
                if (!a)
                {
                    if (spriteRenderer.flipX)
                    {
                        dashPosition.x -= 0.1f;
                    }
                    else
                    {
                        dashPosition.x += 0.1f;
                    }
                }

                if (spriteRenderer.flipX)
                {
                    if (dashPosition.x <= endPosition.x)
                    {
                        b = true;
                    }
                }
                else
                {
                    if (dashPosition.x >= endPosition.x)
                    {
                        b = true;
                    }
                }
            } while (!a && !b);

            _dashMoving = true;
        }
    }
    private void DashMove()
    {
        if (dashMoving)
        {
            transform.DOMove(dashPosition, dashDoTime).SetEase(Ease.InQuad);
        }

        Vector2 _dashPosition = dashPosition;
        Vector2 _currentPosition = currentPosition;
        _dashPosition.y = 0f;
        _currentPosition.y = 0f;

        float distance = Vector2.Distance(_dashPosition, _currentPosition);

        if (dashMoving && distance <= dashStopRange)
        {
            _dashMoving = false;
            whenDashStopMoveSetStarted = false;
            whenDashStopMove = true;
        }
    }
    private void WhenDashStopMove()
    {
        if (whenDashStopMove)
        {
            if (spriteRenderer.flipX)
            {
                rigid.velocity = new Vector2(-1f * playerStat.speed, rigid.velocity.y);
            }
            else
            {
                rigid.velocity = new Vector2(1f * playerStat.speed, rigid.velocity.y);
            }

            if (!whenDashStopMoveSetStarted)
            {
                Invoke("WhenDashStopMoveSet", 2f);
                whenDashStopMoveSetStarted = true;
            }
        }
    }
    private void WhenDashStopMoveSet()
    {
        whenDashStopMove = false;
    }
    private void Move()
    {
        if (!dashMoving && !attacking)
        {
            rigid.velocity = new Vector2(XMove * playerStat.speed, rigid.velocity.y);
            if (XMove == 0f && isGround)
            {
                anim.Play("Idle");
                SetFlaseJumpAnimIsPlaying();

            }
            else if (XMove == 0f && !isGround && !jumpAnimIsPlaying)
            {
                anim.Play("InAir");
            }
            else if (!jumpAnimIsPlaying)
            {
                anim.Play("Move");

            }
        }
    }
    private void Jump()
    {
        if ((isJump && isGround) || (isJump && canDoubleJump))
        {
            isJump = false;
            rigid.AddForce(Vector2.up * playerStat.jumpSpeed, ForceMode2D.Impulse);

            if (canDoubleJump)
            {
                anim.Play("DoubleJump");
                canDoubleJump = false;
            }

            if (isGround)
            {
                anim.Play("Jump");
                canDoubleJump = true;
            }
        }
    }
    private void LRCheck()
    {
        if (XMove != 0f && !attacking)
        {
            if (XMove < 0f)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}
