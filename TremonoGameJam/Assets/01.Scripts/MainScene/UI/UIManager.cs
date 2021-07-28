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

    [SerializeField] GameObject ClearPanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] Button gameOverExitBtn;
    [SerializeField] Button gameOverRetryBtn;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
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
        hpBar.ChangeHP(currentHP);
    }

    public void CallBossIntro(GameObject boss)
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
                boss.SetActive(true);
                });
             });
        }

    public void Clear()
    {
        ClearPanel.SetActive(true);
        ClearPanel.GetComponent<RectTransform>().DOAnchorPosY(0, 1f);
        ClearPanel.GetComponentInChildren<Button>().onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene"));
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        GameOverPanel.GetComponent<RectTransform>().DOAnchorPosY(0, 1f);
        gameOverExitBtn.onClick.AddListener(() => Application.Quit());
        string name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        int num = (int)name[name.Length - 1] - (int)'0';
        gameOverRetryBtn.onClick.AddListener(() => GameManager.Instance.LoadScene(num));
    }
}
