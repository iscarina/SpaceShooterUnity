using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEnemy : EnemyBase
{
    [SerializeField] private GameObject spawnPoint1;
    [SerializeField] private GameObject spawnPoint2;
    [SerializeField] private GameObject disparoPrefab;

    //void Start()
    //{
    //    SetNextMoveTime();
    //    StartCoroutine(Disparar());
    //}

    private void OnEnable()
    {
        EnableEnemy();
        SetNextMoveTime();
        StartCoroutine(Disparar());
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime);
        if (Time.time >= nextMoveTime && !isMovingY)
        {
            StartCoroutine(MoveInY());
        }
    }

    IEnumerator Disparar()
    {
        while(true)
        {
            int spawnPointIndex = Random.Range(0, 2);

            if (spawnPointIndex == 0)
            {
                PoolManager.SpawnObject(disparoPrefab, spawnPoint1.transform.position, Quaternion.identity);
            }
            else
            {
                PoolManager.SpawnObject(disparoPrefab, spawnPoint2.transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(shotTime);
        }
    }

}
