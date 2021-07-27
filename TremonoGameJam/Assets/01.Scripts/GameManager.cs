using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] AudioClip bgm_Start;
    [SerializeField] AudioClip bgm_Main;
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

    public void LoadScene(int stageNum)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene($"Stage {stageNum}");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
}
