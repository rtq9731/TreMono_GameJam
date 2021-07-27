using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    [Header("현재 소환되어있는 Enemy들의 부모오브젝트")]
    [SerializeField]
    private Transform _enemys = null;
    public Transform enemys
    {
        get { return _enemys; }
    }

    [SerializeField]
    private Transform _playerTrm = null;
    public Transform playerTrm
    {
        get { return _playerTrm; }
    }

    [SerializeField]
    private bool _stopPlayer = false;
    public bool stopPlayer
    {
        get{return _stopPlayer;}
    } 
    private float playerStopTimer = 0f;

    private void Update()
    {
        CheckStop();
    }

    private void CheckStop()
    {
        if (stopPlayer)
        {
            playerStopTimer -= Time.deltaTime;
            if (playerStopTimer <= 0f)
            {
                _stopPlayer = false;
            }
        }
    }

    [SerializeField] public GameObject[] brokeObjsPrefab;
    public void StopPlayer(float stopTime)
    {
        _stopPlayer = true;
        playerStopTimer = stopTime;
    }



    
}
