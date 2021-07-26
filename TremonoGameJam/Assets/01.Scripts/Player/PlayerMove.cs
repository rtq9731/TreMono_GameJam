using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput playerInput = null;
    private SpriteRenderer spriteRenderer = null;

    private float XMove = 0f;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();    
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }
    void FixedUpdatq()
    {
        
    }
    private void Move()
    {
        
    }
}
