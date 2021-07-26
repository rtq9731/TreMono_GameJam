using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPlayer : EnemyStatus
{
    
    [Header("벽과 플레이어가 들어가야함")]
    [SerializeField]
    private LayerMask whatIsHit;
    [Header("여긴 플레이어만")]
    [SerializeField]
    private LayerMask whatIsPlayer;
    private float distance = 0f;

    public Status CheckStatus(Vector2 playerPosition, float foundRange, float attackRange)
    {
        distance = Vector2.Distance(transform.position, playerPosition);

        Vector2 _playerPosition = playerPosition;
        Vector2 currentPosition = transform.position;
        _playerPosition.x = -playerPosition.x;


        Vector2 position = playerPosition - currentPosition;

        Ray2D ray = new Ray2D(transform.position, position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10f, whatIsHit);

        Debug.DrawRay(transform.position, hit.point - currentPosition, new Color(0, 1, 0));

        if (hit)
        {
            if (hit.collider.gameObject.layer == GetLayer(LayerMask.GetMask("PLAYER")))
            {
                if (distance <= attackRange)
                {
                    return Status.Attack;
                }
                else if (distance <= foundRange)
                {
                    return Status.Found;
                }
                else
                {
                    return Status.Searching;
                }
            }
            else
            {
                return Status.Searching;
            }
        }
        else
        {
            return Status.Searching;
        }
    }
    public bool CheckFlip(Vector2 targetPosition)
    {
        float a = targetPosition.x - transform.position.x;

        if (a < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private int GetLayer(LayerMask a)
    {
        int b = a;
        int result = 0;

        while (true)
        {
            if (b >= 2)
            {
                result++;
                b = b / 2;
            }
            else
            {
                break;
            }
        }

        return result;
    }
}
