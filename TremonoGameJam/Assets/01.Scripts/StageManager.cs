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


    [SerializeField] public GameObject[] brokeObjsPrefab;



    
}
