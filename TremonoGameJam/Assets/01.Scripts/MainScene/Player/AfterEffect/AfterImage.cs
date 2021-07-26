using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AfterImage : MonoBehaviour
{
    [SerializeField]
    private float fadeOutTime = 0.7f;
    private SpriteRenderer spriteRenderer = null;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite, bool flip, Vector3 position, SpawnAfterImage spawnAfterImage)
    {
        transform.position = position;
        spriteRenderer.flipX = flip;
        spriteRenderer.color = new Color(0f, 1f, 1f, 1f);
        spriteRenderer.sprite = sprite;

        spriteRenderer.DOFade(0, fadeOutTime).OnComplete(() =>
        {
            spawnAfterImage.afterImageList.Add(this);
            gameObject.SetActive(false);
        });
    }
}
