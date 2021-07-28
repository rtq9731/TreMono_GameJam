using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExitPanel : MonoBehaviour
{
    [SerializeField] Button btnOk;
    [SerializeField] Button btnCancel;
    public void OnEnable()
    {
        OnPauseMenu();
    }

    bool isON = false;
    public void OnPauseMenu()
    {
        btnCancel.onClick.RemoveAllListeners();
        btnOk.onClick.RemoveAllListeners();
        Time.timeScale = 0;

        transform.localScale = Vector3.one;
        isON = true;
        btnCancel.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            transform.DOScale(0, 0.3f);
            GameManager.Instance.isPause = false;
            isON = false;
        });
        btnOk.onClick.AddListener(() => Application.Quit());
    }
}
