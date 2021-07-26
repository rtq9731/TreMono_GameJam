using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    private StageManager stageManager = null;
    private Transform playerPosition = null;
    void Start()
    {
        playerPosition = stageManager.playerTrm;
    }

    void Update()
    {
        
    }
    private void CheckDistance()
    {
        float distance = Vector2.Distance(playerPosition.position, transform.position);

        
    }
}
