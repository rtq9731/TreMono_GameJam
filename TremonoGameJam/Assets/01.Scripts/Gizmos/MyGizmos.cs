using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    [Header("어떤것에 관한 Gizmos인가")]
    [SerializeField]
    private string what = "";

    [SerializeField]
    private float radius = 1f;

    [SerializeField]
    private Color color;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
