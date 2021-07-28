using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{
    [SerializeField]
    private float playTime = 2f;

    void Update()
    {
        if (playTime >= 0f)
        {
            playTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
