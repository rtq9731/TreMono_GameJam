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
                Debug.Log("��������");
                transform.Translate(new Vector2(playerDirX, 0) * dashSpeed * Time.deltaTime);

                if (transform.position.x < point_LeftDash.transform.position.x || transform.position.x > point_RightDash.transform.position.x)
                {
                    isDash = false;
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
        Debug.Log("����");
    }

    public IEnumerator Knockdown()
    {
        isKnockdown = true;
        Debug.Log("���� Ÿ��");
        yield return new WaitForSeconds(knockdownTime);
        rightArm.SetTrigger("Idle");
        leftArm.SetTrigger("Idle");
        Debug.Log("���� ����");
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
