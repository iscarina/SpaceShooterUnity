using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEnemy : EnemyBase
{
    [SerializeField] private GameObject spawnPoint1;
    [SerializeField] private GameObject spawnPoint2;
    [SerializeField] private GameObject disparoPrefab;

    void Start()
    {
        SetNextMoveTime();
        StartCoroutine(Disparar());
    }

    void Update()
    {
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
                Instantiate(disparoPrefab, spawnPoint1.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(disparoPrefab, spawnPoint2.transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(shotTime);
        }
    }

}
