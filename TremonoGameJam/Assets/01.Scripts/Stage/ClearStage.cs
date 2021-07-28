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
            int num = (int)name[name.Length - 1] - (int)'0';
            Debug.Log(num);

            if(num == 0)
            {
                GameManager.Instance.LoadScene(5);
                return;
            }
            GameManager.Instance.LoadScene(num + 1);
        }
    }
}
