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
    public SpriteRenderer spriteRenderer { get; private set; }

    [SerializeField]
    private Transform groundChecker = null;

    [SerializeField]
    private LayerMask WhatIsGround;

    [SerializeField]
    private float groundCheckDistance = 1f;

    [SerializeField]
    private float dashDelay = 1f;
    [SerializeField]
    private float attackDelay = 1f;
    [SerializeField]
    private float dashDoTime = 1f;

    [SerializeField]
    private float dashStopRange = 0.5f;

    private float XMove = 0f;

    private bool isGround = false;

    private bool isJump = false;
    private bool isDash = false;
    private bool isAttack = false;

    private bool canDash = true;
    private bool canAttack = true;
    private bool canSpawnAfterImage = true;

    private bool dashMoving = false;
    private bool attacking = false;

    private Vector2 dashPosition = Vector2.zero;
    public Vector2 currentPosition { get; private set; }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStat = GetComponent<PlayerStat>();
        spawnAfterImage = GetComponent<SpawnAfterImage>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (playerInput.isJump)
        {
            isJump = true;
        }

        if (playerInput.isDash)
        {
            if (canDash)
            {
                isDash = true;
            }
        }

        if (playerInput.isAttack)
        {
            isAttack = true;
        }
    }
    void FixedUpdate()
    {
        currentPosition = transform.position;

        XMove = playerInput.XMove;

        LRCheck();
        GroundCheck();

        Move();
        Jump();
        Dash();

        DashMove();
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
    }
    private void Attack()
    {
        attacking = true;
    }
    private void Dash()
    {
        if (XMove != 0 && isDash && canDash)
        {
            isDash = false;
            canDash = false;

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

            Invoke("CanDashSet", dashDelay);
        }
    }
    private void CanDashSet()
    {
        canDash = true;
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

        if (distance <= dashStopRange)
        {
            dashMoving = false;
        }
    }
    private void Move()
    {
        rigid.velocity = new Vector2(XMove * playerStat.speed, rigid.velocity.y);
    }
    private void Jump()
    {
        if (isJump && isGround)
        {
            isJump = false;
            rigid.AddForce(Vector2.up * playerStat.jumpSpeed, ForceMode2D.Impulse);
        }
    }
    private void LRCheck()
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
