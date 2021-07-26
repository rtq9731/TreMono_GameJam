using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectMenu : MonoBehaviour
{
    [SerializeField] Button[] selectBtns;

    private void Start()
    {
        GameManager.Instance.LoadGame();
        for (int i = 0; i <= GameManager.Instance.stageTopScore; i++)
        {
            selectBtns[i].interactable = true;
        }

        selectBtns[0].onClick.AddListener(() => ChangeScene(0));
        selectBtns[1].onClick.AddListener(() => ChangeScene(1));
        selectBtns[2].onClick.AddListener(() => ChangeScene(2));
        selectBtns[3].onClick.AddListener(() => ChangeScene(3));
        selectBtns[4].onClick.AddListener(() => ChangeScene(4));
        selectBtns[5].onClick.AddListener(() => ChangeScene(5));
        selectBtns[6].onClick.AddListener(() => ChangeScene(6));
    }

    void ChangeScene(int stageNum)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene($"Stage {stageNum}");
    }
}
