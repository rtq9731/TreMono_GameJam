using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : MonoBehaviour, IHitable
{
    [Header("애니메이터")]
    [SerializeField] Animator animator;
    [SerializeField] Animator rightArm;
    [SerializeField] Animator leftArm;

    [Header("각종 위치 데이터")]
    [SerializeField] GameObject point_LeftDash;
    [SerializeField] GameObject point_RightDash;

    [Header("각종 속도들")]
    [SerializeField] float dashSpeed;

    [Header("녹다운 회복에 걸리는 시간")]
    [SerializeField] float knockdownTime;

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
                Debug.Log("돌진시작");
                transform.Translate(new Vector2(playerDirX, 0) * dashSpeed * Time.deltaTime);

                if (transform.position.x < point_LeftDash.transform.position.x || transform.position.x > point_RightDash.transform.position.x)
                {
                    isDash = false;
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
                    StartCoroutine(Knockdown());
                }
            }
        }
    }

    public void DashToPlayer()
    {
        playerDirX = GetPlayerDir();
        Debug.Log(GetPlayerDir());
        isDash = true;
        if(playerDirX > 0) { animator.SetTrigger("LookRight"); mydir = Dir.right; }
        else { animator.SetTrigger("LookLeft"); mydir = Dir.left;  }
        Debug.Log("돌진");
    }

    public IEnumerator Knockdown()
    {
        isKnockdown = true;
        Debug.Log("돌진 타격");
        yield return new WaitForSeconds(knockdownTime);
        rightArm.SetTrigger("Idle");
        leftArm.SetTrigger("Idle");
        Debug.Log("돌진 종료");
        isKnockdown = false;
    }

    public float GetPlayerDir()
    {
        return (playerTr.position - this.transform.position).normalized.x;
    }

    public void Hit(int damage)
    {

    }
}
