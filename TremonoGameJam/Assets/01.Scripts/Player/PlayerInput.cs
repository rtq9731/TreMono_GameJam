using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float XMove { get; private set; }

    void Update()
    {
        XMove = Input.GetAxisRaw("Horizontal");
    }
}
