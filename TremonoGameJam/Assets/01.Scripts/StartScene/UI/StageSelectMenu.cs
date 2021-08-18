using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageSelectMenu : MonoBehaviour
{
    [SerializeField] Button[] selectBtns;
    [SerializeField] InputPanel inputPanel;

    bool bCanCallInputPanel = true;

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
    }

    void ChangeScene(int stageNum)
    {
        if (!bCanCallInputPanel)
            return;

        bCanCallInputPanel = false;
        inputPanel.gameObject.SetActive(true);
        inputPanel.msg.text = $"스테이지 {stageNum + 1}\n입장하시겠습니까?";

        inputPanel.GetComponent<RectTransform>().DOAnchorPosY(0, 0.25f).OnComplete(() =>
        {
            inputPanel.btnOK.onClick.AddListener(() => GameManager.Instance.LoadScene(stageNum));
            inputPanel.btnCancel.onClick.AddListener(() => inputPanel.GetComponent<RectTransform>().DOAnchorPosY(1000, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
            {
                inputPanel.gameObject.SetActive(false);
                bCanCallInputPanel = true;
                inputPanel.btnCancel.onClick.RemoveAllListeners();
                inputPanel.btnOK.onClick.RemoveAllListeners();
            }));
        });
    }
}
