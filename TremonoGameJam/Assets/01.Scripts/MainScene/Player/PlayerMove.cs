using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput playerInput = null;
    private PlayerStat playerStat = null;
    private SpawnAfterImage spawnAfterImage = null;

    private Rigidbody2D rigid = null;
    private Animator anim = null;
    public SpriteRenderer spriteRenderer { get; private set; }

    [SerializeField]
    private Transform groundChecker = null;

    [SerializeField]
    private LayerMask WhatIsGround;

    [SerializeField]
    private float groundCheckDistance = 1f;

    [SerializeField]
    private float attackDelay = 1f;
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

    private bool canDoubleJump = false;
    private bool canSpawnAfterImage = true;

    private bool dashMoving = false;
    private bool attacking = false;

    private bool whenDashStopMove = false; // 대쉬가 멈췄을 때 이동하는 것에 관한 변수
    private bool whenDashStopMoveSetStarted = false;

    private Vector2 dashPosition = Vector2.zero;
    public Vector2 currentPosition { get; private set; }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStat = GetComponent<PlayerStat>();
        spawnAfterImage = GetComponent<SpawnAfterImage>();

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

        if (playerInput.isAttack)
        {
            isAttack = true;
        }

        GroundCheck();
    }
    void FixedUpdate()
    {
        currentPosition = transform.position;

        XMove = playerInput.XMove;

        LRCheck();

        Move();
        Attack();
        Jump();

        DashMove();
        WhenDashStopMove();
        SpawnAfterImage();

        transform.position = currentPosition;
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
            WhenDashStopMoveSet();
        }

    }
    private void Attack()
    {
        if (isAttack)
        {
            isAttack = false;
            attacking = true;
            anim.Play("Attack");
            Dash();
        }
    }
    public void ReAttacking()
    {
        attacking = false;
    }
    private void Dash()
    {
        if (XMove != 0 && inAirDashCount > 0)
        {
            if (!isGround)
            {
                inAirDashCount--;
            }

            float _dashRange = playerStat.dashRange;
            dashPosition = currentPosition;
            Vector2 endPosition = currentPosition;

            attacking = false;

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

            dashMoving = true;
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
            dashMoving = false;
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
            anim.Play("Move");
        }
    }
    private void Jump()
    {
        if (isJump && isGround || isJump && canDoubleJump)
        {
            isJump = false;
            rigid.AddForce(Vector2.up * playerStat.jumpSpeed, ForceMode2D.Impulse);

            if (canDoubleJump)
            {
                canDoubleJump = false;
            }

            if (isGround)
            {
                canDoubleJump = true;
            }
        }
    }
    private void LRCheck()
    {
        if (XMove != 0f)
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
