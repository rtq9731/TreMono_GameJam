using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] PlayerStat playerStat;
    [SerializeField] Image[] hpIcons;
    [SerializeField] Sprite[] hpIconSprites;

    int hp4to3Hash = Animator.StringToHash("4to3");
    int hp3to2Hash = Animator.StringToHash("3to2");
    int hp2to1Hash = Animator.StringToHash("2to1");

    int hp1to2Hash = Animator.StringToHash("1to2");
    int hp2to3Hash = Animator.StringToHash("2to3");
    int hp3to4Hash = Animator.StringToHash("3to4");

    int hp = 4;

    private void Start()
    {
        hpIcons[0].sprite = null;
    }

    private void Update()
    {
        if(gameObject.activeSelf)
        {
            Vector3 playerPos = Camera.main.WorldToScreenPoint(new Vector2(FindObjectOfType<PlayerStat>().transform.position.x, FindObjectOfType<PlayerStat>().transform.position.y + 0.75f));
            playerPos.z = 0;
            this.transform.position = playerPos;
        }
    }

    public void ChangeHP(int currentHP)
    {
        if (currentHP < hp) // hp가 내려갔을 때
        {
            Debug.Log("HP 감소");
            switch (currentHP)
            {
                case 0:
                    // 죽었을 때 취할 행동
                    break;

                case 1:
                    GetComponent<Animator>().SetTrigger(hp2to1Hash);
                    break;

                case 2:
                    GetComponent<Animator>().SetTrigger(hp3to2Hash);
                    break;

                case 3:
                    GetComponent<Animator>().SetTrigger(hp4to3Hash);
                    break;

                default:
                    break;
            }
        }
        else
        {
            switch (currentHP)
            {
                case 2:
                    GetComponent<Animator>().SetTrigger(hp1to2Hash);
                    break;

                case 3:
                    GetComponent<Animator>().SetTrigger(hp2to3Hash);
                    break;

                case 4:
                    GetComponent<Animator>().SetTrigger(hp3to4Hash);
                    break;

                default:
                    break;
            }
        }
        CancelInvoke();
        Invoke("ActiveFalse", 1);
        hp = currentHP;
    }

    private void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
