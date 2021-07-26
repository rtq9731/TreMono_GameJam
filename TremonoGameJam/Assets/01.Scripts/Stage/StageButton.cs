using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageButton : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float targetPositionY;
    [SerializeField] private float duration;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "BOX")
        {
            gameObject.SetActive(false);

            door.transform.DOMoveY(targetPositionY, duration).OnComplete(() =>
            {
                door.SetActive(false);
            });
        }
    }
}
