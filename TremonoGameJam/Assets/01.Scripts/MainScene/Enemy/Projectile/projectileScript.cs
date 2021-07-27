using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    private StageManager stageManager = null;
    private Transform playerPosition = null;

    private EnemyMove _enemyMove = null;
    public EnemyMove enemyMove
    {
        get { return _enemyMove; }
        set { _enemyMove = value; }
    }
    private bool _flipX = false;
    public bool flipX
    {
        get { return _flipX; }
        set { _flipX = value; }
    }
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float hitDistance = 0.5f;
    [SerializeField]
    private float hitWallDistance = 0.1f;

    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Vector2 firstPosition = Vector2.zero;
    private float moveRange = 0;
    private int ap = 0;
    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();

        spriteRenderer = FindObjectOfType<SpriteRenderer>();

        playerPosition = stageManager.playerTrm;
    }

    void Update()
    {
        spriteRenderer.flipX = flipX;

        Move();
        CheckWall();
        CheckDistance();
        CheckMove();
    }
    public void SetSpawn(Vector2 spawnPosition, float atttackRange, int damage)
    {
        transform.position = spawnPosition;
        firstPosition = spawnPosition;
        moveRange = atttackRange * 2;
        ap = damage;
    }
    private void Move()
    {
        if (flipX)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }
    private void Destroye()
    {
        enemyMove.projectTiles.Add(gameObject);
        
        gameObject.SetActive(false);
    }
    private void CheckMove()
    {
        float distance = Vector2.Distance(transform.position, firstPosition);

        if(distance >= moveRange)
        {
            Destroye();
        }
    }
    private void CheckWall()
    {
        bool a = Physics2D.OverlapCircle(transform.position, hitWallDistance, whatIsGround);

        if (a)
        { 
            Destroye();
        }
    }
    private void CheckDistance()
    {
        float distance = Vector2.Distance(playerPosition.position, transform.position);

        if (distance <= hitDistance)
        {
            PlayerStat playerStat = playerPosition.GetComponent<PlayerStat>();

            Destroye();

            playerStat.Hit(ap);
            
        }
    }
}
