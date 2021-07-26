using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
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
    public int stageTopScore = 0;
    public void SaveGame()
    {
        PlayerPrefs.SetInt("stageTopScore", stageTopScore);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        stageTopScore = PlayerPrefs.GetInt("stageTopScore", stageTopScore);
    }
}
