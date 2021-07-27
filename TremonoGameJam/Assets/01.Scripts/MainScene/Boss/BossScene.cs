using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossScene : MonoBehaviour, IHitable
{
    [Header("�����յ�")]
    [SerializeField] GameObject breakableObjForAttack;
    [SerializeField] GameObject brokenObjForAttack;

    [Header("�ִϸ�����")]
    [SerializeField] Animator animator;
    [SerializeField] Animator rightArm;
    [SerializeField] Animator leftArm;

    [Header("���� ��ġ ������")]
    [SerializeField] GameObject point_LeftDash;
    [SerializeField] GameObject point_RightDash;
    [SerializeField] GameObject point_RightGrap;
    [SerializeField] GameObject point_LeftGrap;
    [SerializeField] GameObject Point_LeftTop;
    [SerializeField] GameObject Point_RightTop;

    [Header("���� �ӵ���")]
    [SerializeField] float dashSpeed;

    [Header("���� ��Ÿ��")]
    [SerializeField] float wallShakeAttackInterval = 2f;

    [Header("��ٿ� ȸ���� �ɸ��� �ð�")]
    [SerializeField] float knockdownTime;

    [Header("��Ÿ")]
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
                temp = rightArm; // �� ��� ���� ����
                rightArm.SetTrigger("Sto_");
                break;
            case Dir.left:
                temp = leftArm;
                leftArm.SetTrigger("Sto_"); // �� ��� ���� ����
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
        if(playerDirX > 0) { animator.SetTrigger("LookLeft"); mydir = Dir.right; } // �÷��̾ ������� �ݴ뿡 ������ �ݴ��� �Ĵٺ���
        else { animator.SetTrigger("LookRight"); mydir = Dir.left;  }
    }

    public IEnumerator Knockdown()
    {
        isKnockdown = true;
        yield return new WaitForSeconds(knockdownTime);
        rightArm.SetTrigger("Idle");
        leftArm.SetTrigger("Idle");
        isKnockdown = false; 
        
        // �ӽ� �ݺ��� �ڵ�
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
