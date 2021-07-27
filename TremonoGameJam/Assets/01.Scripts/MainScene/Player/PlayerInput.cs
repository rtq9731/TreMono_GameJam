using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float XMove { get; private set; }
    public bool isJump { get; private set; }
    public bool isAttack { get; private set; }
    public bool isSkill1 { get; private set; }
    public bool breakWallSkill { get; private set; }

    void Update()
    {
        XMove = Input.GetAxisRaw("Horizontal");
        isJump = Input.GetButtonDown("Jump");
        isSkill1 = Input.GetButtonDown("Skill1");
        isAttack = Input.GetMouseButtonDown(0);
    }
}