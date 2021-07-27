using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    
    [SerializeField] GameObject bossPanel;
    [SerializeField] GameObject playerProfile;
    [SerializeField] GameObject bossProfile;
    [SerializeField] GameObject nameText;
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject backGroundUp;
    [SerializeField] GameObject backGroundDown;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    private void Start()
    {
        CallBossIntro();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            FindObjectOfType<PlayerStat>().hp--;
        }
        else if(Input.GetKeyDown(KeyCode.Insert))
        {
            FindObjectOfType<PlayerStat>().hp++;
        }
    }
#endif

    [SerializeField] HPBar hpBar;

    public void HPChange(int currentHP)
    {
        hpBar.gameObject.SetActive(true);
        Vector3 playerPos = Camera.main.WorldToScreenPoint(new Vector2(FindObjectOfType<PlayerStat>().transform.position.x, FindObjectOfType<PlayerStat>().transform.position.y + 0.75f));
        playerPos.z = 0;
        hpBar.gameObject.transform.position = playerPos;
        hpBar.ChangeHP(currentHP);
    }

    public void CallBossIntro()
    {
        bossPanel.SetActive(true);

        Sequence seq = DOTween.Sequence();


        seq.Append(nameText.GetComponent<RectTransform>().DOAnchorPosX(64, 2).SetEase(Ease.OutQuint));
        seq.Join(titleText.GetComponent<RectTransform>().DOAnchorPosX(32, 2).SetEase(Ease.OutQuint));
        seq.Join(bossProfile.GetComponent<RectTransform>().DOAnchorPosX(550, 2).SetEase(Ease.Linear));
        seq.Join(playerProfile.GetComponent<RectTransform>().DOAnchorPosX(-300, 2).SetEase(Ease.Linear));
        seq.Join(backGroundUp.GetComponent<RectTransform>().DOAnchorPosY(0, 2).SetEase(Ease.OutQuint));
        seq.Join(backGroundDown.GetComponent<RectTransform>().DOAnchorPosY(0, 2).SetEase(Ease.OutQuint));

        seq.OnComplete(() =>
        {
            Sequence seq1 = DOTween.Sequence();
            seq1.Append(nameText.GetComponent<RectTransform>().DOAnchorPosX(-512, 0.5f).SetEase(Ease.OutQuint));
            seq1.Join(titleText.GetComponent<RectTransform>().DOAnchorPosX(-3000, 0.5f).SetEase(Ease.OutQuint));
            seq1.Join(bossProfile.GetComponent<RectTransform>().DOAnchorPosX(3000, 0.5f).SetEase(Ease.Linear));
            seq1.Join(playerProfile.GetComponent<RectTransform>().DOAnchorPosX(-2000, 0.5f).SetEase(Ease.Linear));
            seq1.Join(backGroundUp.GetComponent<RectTransform>().DOAnchorPosY(1000, 0.5f).SetEase(Ease.OutQuint));
            seq1.Join(backGroundDown.GetComponent<RectTransform>().DOAnchorPosY(-1000, 0.5f).SetEase(Ease.OutQuint));

            seq1.OnComplete(() =>
            {
                bossPanel.SetActive(false);

                });
        });
    }
}
