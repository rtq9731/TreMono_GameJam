using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAfterImage : MonoBehaviour
{
    [SerializeField]
    private GameObject afterImageObj = null;
    [SerializeField]
    private Transform afterImageSpawnPosition = null;
    [SerializeField]
    private float _spawnAfterImageDelayMinimum = 0.1f;
    public float spawnAfterImageDelayMinimum
    {
        get { return _spawnAfterImageDelayMinimum; }
    }
    [SerializeField]
    private float _spawnAfterImageDelayMaximum = 0.1f;
    public float spawnAfterImageDelayMaximum
    {
        get { return _spawnAfterImageDelayMaximum; }
    }

    private PlayerMove myMoveScript = null;
    SpriteRenderer spriteRenderer;
    public List<AfterImage> afterImageList { get; private set; }

    private void Start()
    {
        myMoveScript = GetComponent<PlayerMove>();
        spriteRenderer = myMoveScript.spriteRenderer;

        afterImageList = new List<AfterImage>();
    }

    public void SetAfterImage()
    {
        if (afterImageList.Count <= 0)
        {
            AfterImage afterImage = Instantiate(afterImageObj, afterImageSpawnPosition).GetComponent<AfterImage>();

            afterImage.SetSprite(spriteRenderer.sprite, spriteRenderer.flipX, myMoveScript.currentPosition, this);
        }
        else
        {
            AfterImage _afterImage;

            _afterImage = afterImageList[0];

            afterImageList.Remove(_afterImage);

            _afterImage.SetSprite(spriteRenderer.sprite, spriteRenderer.flipX, myMoveScript.currentPosition, this);

            _afterImage.gameObject.SetActive(true);
        }
    }
}
