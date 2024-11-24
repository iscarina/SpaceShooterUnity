using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemie : EnemyBase
{
    [SerializeField] private GameObject spawnPoint;
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
        while (true)
        {
           Instantiate(disparoPrefab, spawnPoint.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(shotTime);
        }
    }

}
