using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{

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
