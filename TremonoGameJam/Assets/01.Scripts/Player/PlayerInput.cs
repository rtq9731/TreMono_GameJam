using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float XMove { get; private set; }
    public bool isJump { get; private set; }
    public bool isAttack { get; private set; }
    public bool isDash { get; private set; }
    public bool breakWallSkill { get; private set; }

    void Update()
    {
        XMove = Input.GetAxisRaw("Horizontal");
        isJump = Input.GetButtonDown("Jump");
        isAttack = Input.GetButtonDown("Attack");
        isDash = Input.GetButtonDown("Dash");
        breakWallSkill = Input.GetButtonDown("skill1");
    }
}