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

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStat = GetComponent<PlayerStat>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }
    void FixedUpdate()
    {
        XMove = playerInput.XMove;

        LRCheck();
        Move();
    }
    private void Move()
    {
        rigid.velocity = new Vector2(XMove * playerStat.speed, rigid.velocity.y);
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
