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

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<PlayerStat>().hp--;
        }
        else if(Input.GetMouseButtonDown(1))
        {
            FindObjectOfType<PlayerStat>().hp++;
        }
    }

    [SerializeField] HPBar hpBar;

    public void HPChange(int currentHP)
    {
        hpBar.gameObject.SetActive(true);
        Vector3 playerPos = Camera.main.WorldToScreenPoint(new Vector2(FindObjectOfType<PlayerStat>().transform.position.x, FindObjectOfType<PlayerStat>().transform.position.y + 0.75f));
        playerPos.z = 0;
        hpBar.gameObject.transform.position = playerPos;
        hpBar.ChangeHP(currentHP);
    }
}
