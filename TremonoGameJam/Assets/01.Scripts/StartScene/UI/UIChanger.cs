using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIChanger : MonoBehaviour
{
    [Header("메뉴들")]
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject stageSelectMenu;

    [Header("게임 시작 메뉴에서 움직여야 할 오브젝트들")]
    [SerializeField] RectTransform[] startObjs;

    [Header("페이드인/아웃에 걸리는 시간")]
    [SerializeField] float startSceneWaitTime;
    [SerializeField] float FadeDuration;

    [Header("페이드인/아웃 화면 색깔")]
    [SerializeField] Color fadeColor;

    [Header("기타")]
    [SerializeField] Image fadeImage;

    public void ChangeToSelectMenu()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(startObjs[0].DOAnchorPosY(2000, FadeDuration));
        seq.Join(startObjs[1].DOAnchorPosX(2000, FadeDuration));
        seq.Join(startObjs[2].DOAnchorPosX(-2000, FadeDuration).SetEase(Ease.InOutQuad));

        seq.OnComplete(() => startMenu.SetActive(false));

        StartCoroutine(FadeIn(startSceneWaitTime, stageSelectMenu));
    }

    IEnumerator FadeIn(float waitTime, GameObject activeMenu)
    {
        yield return new WaitForSeconds(waitTime);
        Color color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
        fadeImage.color = fadeColor;

        fadeImage.gameObject.SetActive(true);

        fadeImage.DOFade(1, FadeDuration).OnComplete(() =>
        {
            StartCoroutine(FadeOut(waitTime, activeMenu));
        });
        yield return null;
    }

    IEnumerator FadeOut(float waitTime, GameObject activeMenu)
    {
        yield return new WaitForSeconds(waitTime);
        Color color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1);
        fadeImage.color = color;

        fadeImage.gameObject.SetActive(true);
        activeMenu.SetActive(true);

        fadeImage.DOFade(0, FadeDuration).OnComplete(() =>
        {
            fadeImage.gameObject.SetActive(false);
        });
    }
}
