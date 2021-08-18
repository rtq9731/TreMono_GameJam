using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoSingleton<GameManager>
{
    public int stageTopScore;

    public bool isPause = false;

    public void SaveGame()
    {
        PlayerPrefs.SetInt("stageTopScore", stageTopScore);

#if UNITY_EDITOR
        Debug.Log(stageTopScore);
#endif

        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        stageTopScore = PlayerPrefs.GetInt("stageTopScore", 0);

#if UNITY_EDITOR
        Debug.Log(stageTopScore);
#endif
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isPause)
        {
            DOTween.CompleteAll();
            ExitPanel panel = FindObjectOfType<ExitPanel>();
            if (panel != null)
            {
                panel.OnPauseMenu();
            }
            else
            {
                GameObject temp = Instantiate(Resources.Load<GameObject>("ExitPanel"), FindObjectOfType<Canvas>().transform);
                temp.GetComponent<ExitPanel>().OnPauseMenu();
            }
            isPause = true;
        }
    }

    public void LoadScene(int stageNum)
    {

        if (stageTopScore < stageNum && stageNum <= 5)
        {
            stageTopScore = stageNum;
            SaveGame();
        }

        DOTween.CompleteAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene($"Stage {stageNum}");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
    public void LoadScene(string name)
    {
        DOTween.CompleteAll();
        Debug.Log(name);
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
}
