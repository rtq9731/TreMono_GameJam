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
    [SerializeField] GameObject Point_RightTop;

    [Header("각종 속도들")]
    [SerializeField] float dashSpeed;

    [Header("여러 쿨타임")]
    [SerializeField] float wallShakeAttackInterval = 2f;

    [Header("녹다운 회복에 걸리는 시간")]
    [SerializeField] float knockdownTime;

    [Header("기타")]
    [SerializeField] Collider2D playerBlock;
    [SerializeField] short wallShakeAttackCount = 2;
    short wallHitCount = 0;

    Transform playerTr = null;
    ParticleSystem myParticle = null;
    float playerDirX = 0;

    bool isWallAttack = false;
       
    bool isKnockdown = false;

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
        WallAttack();
    }

    private void Update()
    {
        if(!isKnockdown)
        {
            if (isWallAttack)
            {
                transform.Translate(new Vector2(playerDirX, 0) * dashSpeed * Time.deltaTime);
                if(mydir == Dir.right)
                {
                    if (transform.position.x > point_RightDash.transform.position.x)
                    {
                        StartCoroutine(HitWall());
                    }
                }
                else
                {
                    if(transform.position.x < point_LeftDash.transform.position.x)
                    {
                        StartCoroutine(HitWall());
                    }
                }
            }
        }
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
        Animator temp = null;
        switch (mydir)
        {
            case Dir.right:
                temp = rightArm; // 쭉 뻗어서 벽에 박힘
                rightArm.SetTrigger("Sto_");
                break;
            case Dir.left:
                temp = leftArm;
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
            StartCoroutine(HitWall());
        }
        else
        {
            StartCoroutine(Knockdown());
            mydir = Dir.none;
            wallHitCount = 0;
            playerBlock.enabled = false;
        }

        yield return null;
    }

    void makeDamagableObj()
    {
        Vector2 makePos = new Vector2(0, Point_LeftTop.transform.position.y);
        makePos.x = Random.Range(Point_LeftTop.transform.position.x, Point_RightTop.transform.position.x);
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
        Debug.Log(GetPlayerDir());
        if(playerDirX > 0) { animator.SetTrigger("LookLeft"); mydir = Dir.right; } // 플레이어가 진행방향 반대에 있으니 반대쪽 쳐다보기
        else { animator.SetTrigger("LookRight"); mydir = Dir.left;  }
    }

    public IEnumerator Knockdown()
    {
        isKnockdown = true;
        yield return new WaitForSeconds(knockdownTime);
        rightArm.SetTrigger("Idle");
        leftArm.SetTrigger("Idle");
        isKnockdown = false; 
        
        // 임시 반복용 코드
        WallAttack();
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
        
    }
}
