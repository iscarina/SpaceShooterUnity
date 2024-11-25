using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrigateEnemy : EnemyBase
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject disparoPrefab;

    //void Start()
    //{
    //    StartCoroutine(Disparar());
    //    SetNextMoveTime();    
    //}

    private void OnEnable()
    {
        StartCoroutine(Disparar());
        SetNextMoveTime();
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
            Vector3 adjustedPosition = spawnPoint.transform.position + new Vector3(-9.2f, 0, 0);
            GameObject ray = PoolManager.SpawnObject(disparoPrefab, adjustedPosition, Quaternion.identity);
            ray.transform.SetParent(transform);

            StartCoroutine(ReturnRayToPoolAfterTime(ray, Random.Range(5f, 8f)));

            while (ray != null)
            {
                yield return null;
            }

            yield return new WaitForSeconds(shotTime);
        }
    }

    IEnumerator ReturnRayToPoolAfterTime(GameObject ray, float time)
    {
        yield return new WaitForSeconds(time);

        PoolManager.ReturnObjectToPool(ray);
    }
}
