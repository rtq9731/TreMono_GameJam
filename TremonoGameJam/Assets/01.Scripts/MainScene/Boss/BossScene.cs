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
    [SerializeField] float snortPower;

    [Header("���� ��Ÿ��")]
    [SerializeField] float wallShakeAttackInterval = 2f;

    [Header("��ٿ� ȸ���� �ɸ��� �ð�")]
    [SerializeField] float knockdownTime;

    [Header("��Ÿ")]
    [SerializeField] Collider2D playerBlock;
    [SerializeField] short wallShakeAttackCount = 2;
    [SerializeField] GameObject rightTentacleFirst;
    [SerializeField] GameObject leftTentacleFirst;
    [SerializeField] LayerMask whatIsPlayer;

    short wallHitCount = 0;

    Transform playerTr = null;
    ParticleSystem myParticle = null;
    float playerDirX = 0;

    bool isWallAttack = false;
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
            if (!isArrived)
                transform.Translate(new Vector2(playerDirX, 0) * dashSpeed * Time.deltaTime);

            if (mydir == Dir.right)
            {
                if (transform.position.x > point_RightDash.transform.position.x)
                {
                    isArrived = true;
                }
            }
            else
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

    public void SnortAttack() // ���Ƶ��̱�
    {
        DashToPlayer();
        isSnortAttack = true;
    }

    IEnumerator SnortPlayer()
    {
        bool isMoveOver = false;
        RaycastHit2D hit;
        if (mydir == Dir.left)
        {
            transform.DOMove(point_LeftGrap.transform.position, 2).OnComplete(() => isMoveOver = true);
            while (!isMoveOver)
            {
                yield return null;
            }
            rightArm.Play("Strech");
            rightArm.enabled = false;
            Lookat(playerTr, rightTentacleFirst.transform, 0);
            hit = Physics2D.Raycast(rightTentacleFirst.transform.position, rightTentacleFirst.transform.right, 100, whatIsPlayer);
        }
        else
        {
            transform.DOMove(point_RightGrap.transform.position, 2).OnComplete(() => isMoveOver = true);
            while (!isMoveOver)
            {
                yield return null;
            }
            leftArm.Play("Strech");
            leftArm.enabled = false;
            Lookat(playerTr, leftTentacleFirst.transform, 0);
            hit = Physics2D.Raycast(leftTentacleFirst.transform.position, leftTentacleFirst.transform.right, 100, whatIsPlayer);
        }

        while (!isSnortAttackHit)
        {
            hit.transform.GetComponent<Rigidbody2D>().AddForce(hit.normal * snortPower);
        } 
        yield return null;
    }

    void Snort(RaycastHit2D hit)
    {
        Debug.DrawRay(hit.transform.position, hit.normal);
        hit.transform.GetComponent<Rigidbody2D>().AddForce(hit.normal * snortPower);
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
                rightArm.SetTrigger("Sto_"); // �� ��� ���� ����
                break;
            case Dir.left:
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
        }

        yield return null;
    }

    void InitBools()
    {
        mydir = Dir.none;
        isArrived = false;
        wallHitCount = 0;
        playerBlock.enabled = false;
        isSnortAttackHit = false;
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
        if(playerDirX > 0) { animator.SetTrigger("LookLeft"); mydir = Dir.right; } // �÷��̾ ������� �ݴ뿡 ������ �ݴ��� �Ĵٺ���
        else { animator.SetTrigger("LookRight"); mydir = Dir.left;  }
    }

    public IEnumerator Knockdown()
    {
        isKnockdown = true;
        InitBools();
        yield return new WaitForSeconds(knockdownTime);
        isKnockdown = false;
        rightArm.SetTrigger("Idle");
        leftArm.SetTrigger("Idle");

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
        hp--;

        if(isSnortAttack)
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
