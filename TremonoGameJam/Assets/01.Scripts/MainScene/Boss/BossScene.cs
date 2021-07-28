using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossScene : MonoBehaviour, IHitable
{
    enum Pattern
    {
        WallAttack,
        SnortAttack,
        TentacleAttack,
    };

    [Header("공격 패턴 ( 순회 반복 )")]
    [SerializeField] Pattern[] patterns = null;
    Pattern currentPattern = Pattern.WallAttack;

    int patternCount = 0;

    [Header("프리팹들")]
    [SerializeField] GameObject breakableObjForAttack;
    [SerializeField] GameObject brokenObjForAttack;
    [SerializeField] GameObject breakWallEffect;
    private GameObject breakWallEffectInstance = null;

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
    [SerializeField] GameObject point_Ground;

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
    [SerializeField] LayerMask whatIsAttackable;

    short wallHitCount = 0;

    Transform playerTr = null;
    ParticleSystem myParticle = null;
    CameraShake cameraShake = null;
    RaycastHit2D playerHit;
    float playerDirX = 0;

    bool isWallAttack = false;
    bool isTentacleAttack = false;
    bool isTentacleAttacking = false;
    bool isTentacleAttackHit = false;
    bool isSnortAttack = false;
    bool isSnortAttackHit = false;

    bool isArrived = false;
    bool isDead = false;

    bool isKnockdown = false;

    short hp = 5;

    enum Dir
    {
        none,
        right,
        left
    };

    Dir mydir = Dir.none;

    private void OnEnable()
    {
        playerTr = FindObjectOfType<PlayerStat>().transform;
        myParticle = GetComponentInChildren<ParticleSystem>();
        cameraShake = GetComponent<CameraShake>();
        PlayNextAttack();
    }

    private void Update()
    {
        if (isDead)
            return;

        if (!isKnockdown)
        {
            if(isTentacleAttack)
            {
                isTentacleAttack = false;
                isTentacleAttacking = true;
                transform.DOMove(point_Center.transform.position, 2f).OnComplete(() =>
                {
                    StartCoroutine(TentacleAttackPlayer());
                });
            }
            else
            {
                if (isTentacleAttacking)
                    return;

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
        isTentacleAttack = true;
    }

    IEnumerator TentacleAttackPlayer()
    {
        isTentacleAttack = false;
        while (!isTentacleAttackHit)
        {
            playerDirX = GetPlayerDir();
            if (playerDirX > 0) { animator.SetTrigger("LookRight");  mydir = Dir.right; }
            else { animator.SetTrigger("LookLeft"); mydir = Dir.left; }

            if(mydir == Dir.right)
            {
                rightArm.SetTrigger("Cto_");
                Lookat(playerTr, rightArm.transform, 0);
            }
            else if (mydir == Dir.left)
            {
                leftArm.SetTrigger("Cto_");
                Lookat(playerTr, leftArm.transform, -180);
                
            }

            yield return new WaitForSeconds(0.5f);

            if (mydir == Dir.right)
            {
                if (point_Ground.transform.position.y >= rightTentacleLast.transform.position.y)
                    isTentacleAttackHit = true;
            }
            else if (mydir == Dir.left)
            {
                if (point_Ground.transform.position.y >= leftTentacleLast.transform.position.y)
                    isTentacleAttackHit = true;
            }
        }

        leftArm.transform.rotation = Quaternion.Euler(Vector3.zero);
        rightArm.transform.rotation = Quaternion.Euler(Vector3.zero);
        StartCoroutine(Knockdown());
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
                rightArm.Play("Snort");
            });
            while (!isMoveOver)
            {
                yield return null;
            }
            leftArm.SetTrigger("Grab");
            cameraShake.ShakeCamera(1.5f, 0.5f);

            bool isAnimOver = false;
            float timer = 0f;
            while (!isAnimOver)
            {
                timer += Time.deltaTime;
                if (timer > leftArm.GetCurrentAnimatorStateInfo(0).length)
                    isAnimOver = true;
            }
            PlayStoneBreakAnim(leftArm.transform);

            while (!isSnortAttackHit)
            {
                Lookat(playerTr, rightTentacleLast.transform, 0);
                playerHit = Physics2D.Raycast(rightTentacleLast.transform.position, rightTentacleLast.transform.right, 100, whatIsPlayer);
                if(playerHit)
                {
                    if (playerHit.distance < 2)
                    {
                        playerTr.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        isSnortAttackHit = true;
                    }
                    else
                    {
                        playerTr.GetComponent<Rigidbody2D>().AddForce(playerHit.normal * (20 + playerHit.distance * 5));
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
                leftArm.Play("Snort");
            });
            while (!isMoveOver)
            {
                yield return null;
            }
            rightArm.SetTrigger("Grab");
            cameraShake.ShakeCamera(1.5f, 0.5f);

            bool isAnimOver = false;
            float timer = 0f;
            Debug.Log(rightArm.GetCurrentAnimatorStateInfo(0).length);
            while (!isAnimOver)
            {
                timer += Time.deltaTime;
                if (timer > rightArm.GetCurrentAnimatorStateInfo(0).length)
                    isAnimOver = true;
            }
            PlayStoneBreakAnim(rightTentacleLast.transform);

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
                        playerTr.GetComponent<Rigidbody2D>().AddForce(playerHit.normal * (20 + playerHit.distance * 5));
                    }
                }
                yield return null;
            }
        }

        transform.DOMove(point_Center.transform.position, 2);
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
                cameraShake.ShakeCamera(1.5f, 0.5f);
                break;
            case Dir.left:
                leftArm.SetTrigger("Sto_"); // 쭉 뻗어서 벽에 박힘
                cameraShake.ShakeCamera(1.5f, 0.5f);
                break;
            default:
                break;
        }

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
        isTentacleAttackHit = false;
        isTentacleAttacking = false;
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

        PlayNextAttack();
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
        if (isDead) return;

        hp--;
        animator.SetTrigger("Hit");

        if (isSnortAttack)
        {
            isSnortAttackHit = true;
        }

        if(hp < 0)
        {
            Die();
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

    void PlayStoneBreakAnim(Transform target)
    {
        if (breakWallEffectInstance == null)
            breakWallEffectInstance = Instantiate(breakWallEffect);

        breakWallEffect.transform.position = target.position;
        breakWallEffect.GetComponentInChildren<ParticleSystem>().Play();
    }

    void PlayNextAttack()
    {
        switch (GetNextPattern())
        {
            case Pattern.WallAttack:
                WallAttack();
                break;
            case Pattern.SnortAttack:
                SnortAttack();
                break;
            case Pattern.TentacleAttack:
                TentacleAttack();
                break;
            default:
                break;
        }
    }

    Pattern GetNextPattern()
    {
        patternCount++;
        return patterns[( patternCount - 1 ) % patterns.Length];
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        UIManager.Instance.Clear();
        Destroy(this.gameObject, 2);
    }
}
