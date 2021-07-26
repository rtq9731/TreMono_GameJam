using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [SerializeField] GameObject[] objs;
    [SerializeField] float duration;

    public void ChangeMainScene()
    {
        objs[0].transform.DOMoveY(2000, duration);
        objs[1].transform.DOMoveX(2000, duration);
        objs[2].transform.DOMoveX(-2000, duration).OnComplete(() => SceneManager.LoadScene("MainScene"));
    }

}
