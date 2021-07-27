using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossScene : MonoBehaviour, IHitable
{
    [Header("프리팹들")]
    [SerializeField] GameObject breakableObjForAttack;
    [SerializeField] GameObject brokenObjForAttack;

    [Header("애니메이터")]
    [SerializeField] Animator animator;
    [SerializeField] Animator rightArm;
    [SerializeField] Animator leftArm;

    [Header("각종 위치 데이터")]
    [SerializeField] GameObject point_LeftDash;
    [SerializeField] GameObject point_RightDash;
    [SerializeField] GameObject point_RightGrap;
    [SerializeField] GameObject point_LeftGrap;
    [SerializeField] GameObject Point_LeftTop;
    [SerializeField] GameObject point_RightTop;
    [SerializeField] GameObject point_Center;

    [Header("각종 속도들")]
    [SerializeField] float dashSpeed;
    [SerializeField] float snortPower;

    [Header("여러 쿨타임")]
    [SerializeField] float wallShakeAttackInterval = 2f;

    [Header("녹다운 회복에 걸리는 시간")]
    [SerializeField] float knockdownTime;

    [Header("기타")]
    [SerializeField] Collider2D playerBlock;
    [SerializeField] short wallShakeAttackCount = 2;
    [SerializeField] GameObject rightTentacleFirst;
    [SerializeField] GameObject rightTentacleLast;
    [SerializeField] GameObject leftTentacleFirst;
    [SerializeField] GameObject leftTentacleLast;
    [SerializeField] LayerMask whatIsPlayer;

    short wallHitCount = 0;

    Transform playerTr = null;
    ParticleSystem myParticle = null;
    RaycastHit2D playerHit;
    float playerDirX = 0;

    bool isWallAttack = false;
    bool isTentacleAttack = false;
    bool isTentacleAttackHit = false;
    bool isSnortAttack = false;
    bool isSnortAttackHit = false;

    bool isArrived = false;

    bool isKnockdown = false;

    short hp = 40;

    enum Dir
    {
        none,
        right,
        left
    };

    Dir mydir = Dir.none;

    private void Start()
    {
        playerTr = FindObjectOfType<PlayerStat>().transform;
        myParticle = GetComponentInChildren<ParticleSystem>();
        SnortAttack();
    }

    private void Update()
    {
        if (!isKnockdown)
        {
            if(isTentacleAttack)
            {
                transform.DOMove(Vector2.zero, 2f).OnComplete(() =>
                {
                    StartCoroutine(TentacleAttackPlayer());
                });
            }
            else
            {
                if (!isArrived)
                    transform.Translate(new Vector2(playerDirX, 0) * dashSpeed * Time.deltaTime);

                if (mydir == Dir.right)
                {
                    if (transform.position.x > point_RightDash.transform.position.x)
                    {
                        isArrived = true;
                    }
                }
                else if (mydir == Dir.left)
                {
                    if (transform.position.x < point_LeftDash.transform.position.x)
                    {
                        isArrived = true;
                    }
                }

                if (isWallAttack && isArrived)
                {
                    StartCoroutine(HitWall());
                }

                if (isSnortAttack && isArrived)
                {
                    StartCoroutine(SnortPlayer());
                }
            }
        }
    }

    public void TentacleAttack()
    {
        DashToPlayer();
        isTentacleAttack = true;
    }

    IEnumerator TentacleAttackPlayer()
    {
        isTentacleAttack = false;
        if (playerDirX > 0) { animator.SetTrigger("LookLeft"); mydir = Dir.right; } // 플레이어가 진행방향 반대에 있으니 반대쪽 쳐다보기
        else { animator.SetTrigger("LookRight"); mydir = Dir.left; }
        yield return null;
    }

    public void SnortAttack() // 빨아들이기
    {
        DashToPlayer();
        isSnortAttack = true;
    }

    IEnumerator SnortPlayer()
    {
        isSnortAttack = false;
        bool isMoveOver = false;
        if (mydir == Dir.left)
        {
            transform.DOMove(point_LeftGrap.transform.position, 2).OnComplete(() =>
            {
                isMoveOver = true;
                rightArm.Play("Strech");
            });
            while (!isMoveOver)
            {
                yield return null;
            }
            leftArm.SetTrigger("Grab");
            while (!isSnortAttackHit)
            {
                Lookat(playerTr, rightTentacleLast.transform, 0);
                playerHit = Physics2D.Raycast(rightTentacleLast.transform.position, rightTentacleLast.transform.right, 100, whatIsPlayer);
                if(playerHit)
                {
                    if (playerHit.distance < 1)
                    {
                        playerTr.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        isSnortAttackHit = true;
                    }
                    else
                    {
                        playerTr.GetComponent<Rigidbody2D>().AddForce(playerHit.normal * (50 + playerHit.distance * 5));
                        Debug.Log(50 + playerHit.distance * 5);
                    }
                }
                yield return null;
            }
        }
        else if(mydir == Dir.right)
        {
            transform.DOMove(point_RightGrap.transform.position, 2).OnComplete(() =>
            {
                isMoveOver = true;
                leftArm.Play("Strech");
            });
            while (!isMoveOver)
            {
                yield return null;
            }
            rightArm.SetTrigger("Grab");
            while (!isSnortAttackHit)
            {
                Lookat(playerTr, leftTentacleLast.transform, 180);
                playerHit = Physics2D.Raycast(leftTentacleLast.transform.position, -leftTentacleLast.transform.right, 100, whatIsPlayer);
                if (playerHit)
                {
                    if (playerHit.distance < 1)
                    {
                        playerTr.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        isSnortAttackHit = true;
                    }
                    else
                    {
                        playerTr.GetComponent<Rigidbody2D>().AddForce(playerHit.normal * (50 + playerHit.distance * 5));
                        Debug.Log(300 % playerHit.distance);
                    }
                }
                yield return null;
            }
        }

        transform.DOMove(Vector2.zero, 2);
        leftTentacleLast.transform.eulerAngles = Vector3.zero;
        rightTentacleLast.transform.eulerAngles = Vector3.zero;
        StartCoroutine(Knockdown());
        yield return null;
    }

    public void WallAttack()
    {
        DashToPlayer();
        isWallAttack = true;
    }

    public IEnumerator HitWall()
    {
        playerBlock.enabled = true;
        isWallAttack = false;
        wallHitCount++;

        switch (mydir)
        {
            case Dir.right:
                rightArm.SetTrigger("Sto_"); // 쭉 뻗어서 벽에 박힘
                break;
            case Dir.left:
                leftArm.SetTrigger("Sto_"); // 쭉 뻗어서 벽에 박힘
                break;
            default:
                break;
        }

        makeDamagableObj();
        makeDamagableObj();
        makeDamagableObj();
        makeDamagableObj();

        yield return new WaitForSeconds(wallShakeAttackInterval);

        if(wallHitCount < wallShakeAttackCount)
        {
            StopAllCoroutines();
            StartCoroutine(HitWall());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Knockdown());
        }

        yield return null;
    }

    void InitBools()
    {
        leftArm.enabled = true;
        rightArm.enabled = true;

        leftArm.SetTrigger("Idle");
        rightArm.SetTrigger("Idle");

        mydir = Dir.none;
        isArrived = false;
        wallHitCount = 0;
        playerBlock.enabled = false;
        isSnortAttackHit = false;
    }

    void makeDamagableObj()
    {
        Vector2 makePos = new Vector2(0, Point_LeftTop.transform.position.y);
        makePos.x = Random.Range(Point_LeftTop.transform.position.x, point_RightTop.transform.position.x);
        GameObject temp = Instantiate(breakableObjForAttack, makePos, Quaternion.identity).GetComponent<BreakObjForAttack>().brokenObj = brokenObjForAttack;
    }

    public void DashToPlayer()
    {
        if(GetPlayerDir() != 0)
        {
            playerDirX = GetPlayerDir();
        }
        else
        {
            playerDirX = 1;
        }
        if(playerDirX > 0) { animator.SetTrigger("LookLeft"); mydir = Dir.right; } // 플레이어가 진행방향 반대에 있으니 반대쪽 쳐다보기
        else { animator.SetTrigger("LookRight"); mydir = Dir.left;  }
    }

    public IEnumerator Knockdown()
    {
        isKnockdown = true;
        InitBools();
        animator.SetTrigger("Heat");
        yield return new WaitForSeconds(knockdownTime);
        isKnockdown = false;
        rightArm.SetTrigger("Idle");
        leftArm.SetTrigger("Idle");

        SnortAttack();
    }

    public float GetPlayerDir()
    {
        if((playerTr.position - this.transform.position).normalized.x > 0)
            return (playerTr.position - this.transform.position).normalized.x / (playerTr.position - this.transform.position).normalized.x;
        else
            return (playerTr.position - this.transform.position).normalized.x / -(playerTr.position - this.transform.position).normalized.x;
    }

    public void Hit(int damage)
    {
        hp--;
        Debug.Log(hp);
        animator.SetTrigger("Hit");

        if (isSnortAttack)
        {
            isSnortAttackHit = true;
        }
    }

    private void Lookat(Transform target, Transform thisObj , float offset)
    {
        Vector3 targetPos = target.position;
        Vector3 thisPos = thisObj.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        thisObj.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
    }
}
