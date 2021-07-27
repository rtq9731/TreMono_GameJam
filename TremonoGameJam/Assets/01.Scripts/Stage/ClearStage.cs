using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearStage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            string name = SceneManager.GetActiveScene().name;
            int num = name[name.Length - 1] - '0';

            Debug.Log(GameManager.Instance.stageTopScore);
            Debug.Log(num + 1);

            GameManager.Instance.stageTopScore = num + 1;
            SceneManager.LoadScene("StartScene");
        }
    }
}
