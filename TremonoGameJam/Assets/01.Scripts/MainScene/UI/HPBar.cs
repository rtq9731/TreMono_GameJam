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

    public void ChangeHP(int currentHP)
    {
        if (currentHP < hp) // hp가 내려갔을 때
        {
            switch (currentHP)
            {
                case 0:
                    // 죽었을 때 취할 행동
                    break;

                case 1:
                    ChangeIcons(currentHP);
                    hpIcons[hpIcons.Length - 4].GetComponent<Animator>().SetTrigger(hp2to1Hash);
                    break;

                case 2:
                    ChangeIcons(currentHP);
                    hpIcons[hpIcons.Length - 3].GetComponent<Animator>().SetTrigger(hp3to2Hash);
                    break;

                case 3:
                    ChangeIcons(currentHP);
                    hpIcons[hpIcons.Length - 2].GetComponent<Animator>().SetTrigger(hp4to3Hash);
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
                    ChangeIcons(currentHP);
                    hpIcons[hpIcons.Length - 3].GetComponent<Animator>().SetTrigger(hp1to2Hash);
                    hpIcons[hpIcons.Length - 2].sprite = hpIconSprites[currentHP];
                    break;

                case 3:
                    ChangeIcons(currentHP);
                    hpIcons[hpIcons.Length - 2].GetComponent<Animator>().SetTrigger(hp2to3Hash);
                    hpIcons[hpIcons.Length - 1].sprite = hpIconSprites[currentHP];
                    break;

                case 4:
                    ChangeIcons(currentHP);
                    hpIcons[hpIcons.Length - 2].GetComponent<Animator>().SetTrigger(hp3to4Hash);
                    hpIcons[hpIcons.Length - 1].sprite = hpIconSprites[currentHP];
                    break;

                default:
                    break;
            }
        }

        hp = currentHP;
    }

    private void ChangeIcons(int currentHP)
    {
        for (int i = 0; i < currentHP; i++)
        {
            hpIcons[i].sprite = hpIconSprites[currentHP];
        }
    }
}
