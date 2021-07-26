using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    [SerializeField] PlayerStat playerStat;
    [SerializeField] Image[] hpIcons;

    int hp4to3Hash = Animator.StringToHash("4to3");
    int hp3to2Hash = Animator.StringToHash("3to2");
    int hp2to1Hash = Animator.StringToHash("2to1");

    int hp = 4;

    public void ChangeHP(int currentHP)
    {
        if( currentHP < hp) // hp가 내려갔을 때
        {
            switch (currentHP)
            {
                case 0:
                    // 죽었을 때 취할 행동
                    break;
                case 1:
                    hpIcons[hpIcons.Length - 3].GetComponent<Animator>().SetTrigger(hp2to1Hash);
                    break;

                case 2:
                    hpIcons[hpIcons.Length - 2].GetComponent<Animator>().SetTrigger(hp3to2Hash);
                    break;

                case 3:
                    hpIcons[hpIcons.Length - 1].GetComponent<Animator>().SetTrigger(hp4to3Hash);
                    break;

                default:
                    break;
            }
        }
        else
        {
            switch (currentHP)
            {
                case 0:
                    // 죽었을 때 취할 행동
                    break;

                case 1:
                    hpIcons[hpIcons.Length - 3].GetComponent<Animator>().SetTrigger(hp2to1Hash);
                    break;

                case 2:
                    hpIcons[hpIcons.Length - 2].GetComponent<Animator>().SetTrigger(hp3to2Hash);
                    break;

                case 3:
                    hpIcons[hpIcons.Length - 1].GetComponent<Animator>().SetTrigger(hp4to3Hash);
                    hpIcons[hpIcons.Length - 2].sprite = null;
                    break;

                default:
                    break;
            }
        }

        hp = currentHP;
    }

    private void ActiveIcons()
    {

    }

}
