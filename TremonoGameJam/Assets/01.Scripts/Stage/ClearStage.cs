using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearStage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        int num = name[name.Length - 1] - '0';

        GameManager.Instance.stageTopScore = num + 1;
        SceneManager.LoadScene("StartScene");
    }
}
