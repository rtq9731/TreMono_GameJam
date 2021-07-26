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
    }

    void ChangeScene()
    {

    }
}
