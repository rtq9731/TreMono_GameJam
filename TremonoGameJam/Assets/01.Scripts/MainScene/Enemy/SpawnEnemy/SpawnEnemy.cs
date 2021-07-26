using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private GameManager gameManager = null;

    [Header("이 오브젝트와 플레이어의 거리가 이 변수의 값보다 작으면 Enemy가 스폰됨")]
    [SerializeField]
    private float enemySpawnRange = 5f;

    [Header("이 오브젝트 자신 혹은, 임의의 오브젝트")]
    [SerializeField]
    private Transform enemySpawnPosition = null;
    private bool enemySpawned = false;

    [Header("Spawn될 Enemy는 이 오브젝트의 자식오브젝트로, gameObject.active == false인 상태로 있고, 이 변수는 그 오브젝트를 담고있어야함.")]
    [SerializeField]
    private GameObject spawnThis = null;
    private Vector2 currentPosition = Vector2.zero;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void FixedUpdate()
    {
        currentPosition = transform.position;

        Spawn();

        transform.position = currentPosition;
    }
    private void Spawn()
    {
        float distance = Vector2.Distance(gameManager.playerTrm.transform.position, currentPosition);

        if(distance <= enemySpawnRange && !enemySpawned)
        {
            enemySpawned = true;
            spawnThis.SetActive(true);
            spawnThis.transform.position = enemySpawnPosition.position;

            spawnThis.transform.SetParent(gameManager.enemys);
            gameObject.SetActive(false);
        }
    }
}
