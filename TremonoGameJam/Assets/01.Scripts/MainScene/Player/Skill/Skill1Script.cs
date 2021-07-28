using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1Script : MonoBehaviour
{
    [SerializeField]
    private float maxScale = 5f;
    [SerializeField]
    private float scaleSetSpeed = 1f;
    [SerializeField]
    private LayerMask whatIsAttackable;
    [SerializeField]
    private PlayerStat playerStat = null;
    [SerializeField]
    private GameObject skillSound = null;

    private Vector2 currentScale = Vector2.zero;
    [SerializeField]
    private float moveBackRange = 5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentScale.x);
    }
    private void FixedUpdate()
    {
        currentScale = transform.localScale;

        SetScale();
        AttackCheck();

        transform.localScale = currentScale;
    }
    public void SetSpawn(Vector2 spawnPosition)
    {
        transform.position = spawnPosition;

        currentScale.x = 0f;
        currentScale.y = 0f;
        playerStat.Hit(1);
        SkillSound();

        transform.localScale = currentScale;
    }
    private void SkillSound()
    {
        Instantiate(skillSound, transform);
    }
    private void AttackCheck()
    {
        Collider2D[] attackedObj;
        Transform[] attackedTrm;
        EnemyMove enemyMove;
        EnemyStat enemyStat;

        Vector2 movePosition = Vector2.zero;

        attackedObj = Physics2D.OverlapCircleAll(transform.position, currentScale.x, whatIsAttackable);

        attackedTrm = new Transform[attackedObj.Length];

        for (int i = 0; i < attackedObj.Length; i++)
        {
            attackedTrm[i] = attackedObj[i].transform;
        }

        bool left = false;
        foreach (var item in attackedTrm)
        {
            enemyMove = item.GetComponent<EnemyMove>();
            enemyStat = item.GetComponent<EnemyStat>();

            if (!enemyMove.moveBYPlayerSkill)
            {
                if (item.position.x <= transform.position.x)
                {
                    left = true;
                }
                else
                {
                    left = false;
                }

                if(left)
                {
                    movePosition.x = item.position.x - moveBackRange;
                }
                else
                {
                    movePosition.x = item.position.x + moveBackRange;
                }

                enemyMove.SetMoveByPlayerSkill(movePosition);
                enemyStat.Hit(1);
            }
        }
    }
    private void SetScale()
    {
        currentScale.x = Mathf.Lerp(currentScale.x, maxScale, scaleSetSpeed * Time.fixedDeltaTime);
        currentScale.y = Mathf.Lerp(currentScale.y, maxScale, scaleSetSpeed * Time.fixedDeltaTime);

        float x = maxScale - currentScale.x;
        float y = maxScale - currentScale.y;

        if (x <= 0.5f && y <= 0.5f)
        {
            gameObject.SetActive(false);
        }
    }
}
