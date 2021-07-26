using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput playerInput = null;
    private PlayerStat playerStat = null;

    private Rigidbody2D rigid = null;
    private SpriteRenderer spriteRenderer = null;

    private float XMove = 0f;

    private bool isJump = false;
    private bool isAttack = false;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStat = GetComponent<PlayerStat>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
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
    }
    void FixedUpdate()
    {
        XMove = playerInput.XMove;

        LRCheck();
        Move();
        Jump();
    }
    private void Move()
    {
        rigid.velocity = new Vector2(XMove * playerStat.speed, rigid.velocity.y);
    }
    private void Jump()
    {
        if (isJump)
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
