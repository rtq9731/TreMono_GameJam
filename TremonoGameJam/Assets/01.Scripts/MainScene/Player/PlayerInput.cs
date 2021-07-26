using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float XMove { get; private set; }
    public bool isJump { get; private set; }
    public bool isAttack { get; private set; }
    public bool breakWallSkill { get; private set; }

    void Update()
    {
        XMove = Input.GetAxisRaw("Horizontal");
        isJump = Input.GetButtonDown("Jump");
        isAttack = Input.GetMouseButtonDown(0);
        breakWallSkill = Input.GetButtonDown("skill1");
    }
}