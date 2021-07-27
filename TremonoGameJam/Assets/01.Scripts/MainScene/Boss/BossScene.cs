using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : MonoBehaviour, IHitable
{
    [Header("�ִϸ�����")]
    [SerializeField] Animator animator;
    [SerializeField] Animator rightArm;
    [SerializeField] Animator leftArm;

    [Header("���� ��ġ ������")]
    [SerializeField] GameObject point_LeftDash;
    [SerializeField] GameObject point_RightDash;

    [Header("���� �ӵ���")]
    [SerializeField] float dashSpeed;

    [Header("��ٿ� ȸ���� �ɸ��� �ð�")]
    [SerializeField] float knockdownTime;

    [Header("��Ÿ")]
    [SerializeField] Collider2D playerBlock;
    [SerializeField] short wallShakeAttackCount = 2;
    [SerializeField] float wallShakeAttackInterval = 2f;
    short wallHitCount = 0;

    Transform playerTr = null;
    ParticleSystem myParticle = null;
    float playerDirX = 0;

    bool isDash = false;
    bool isDashAttack = false;

    bool isKnockdown = false;

    enum Dir
    {
        right,
        left
    };

    Dir mydir = Dir.right;

    private void Start()
    {
        playerTr = FindObjectOfType<PlayerStat>().transform;
        myParticle = GetComponentInChildren<ParticleSystem>();
        DashToPlayer();
    }

    private void Update()
    {
        if(!isKnockdown)
        {
            if (isDash)
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

    public IEnumerator HitWall()
    {
        playerBlock.enabled = true;
        isDash = false;
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

        yield return new WaitForSeconds(wallShakeAttackInterval);

        if(wallHitCount < wallShakeAttackCount)
        {
            StartCoroutine(HitWall());
        }
        else
        {
            StartCoroutine(Knockdown());
            wallHitCount = 0;
            playerBlock.enabled = false;
        }

        yield return null;
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
        isDash = true;
        if(playerDirX > 0) { animator.SetTrigger("LookRight"); mydir = Dir.right; }
        else { animator.SetTrigger("LookLeft"); mydir = Dir.left;  }
    }

    public IEnumerator Knockdown()
    {
        isKnockdown = true;
        yield return new WaitForSeconds(knockdownTime);
        rightArm.SetTrigger("Idle");
        leftArm.SetTrigger("Idle");
        isKnockdown = false;
        DashToPlayer();
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
