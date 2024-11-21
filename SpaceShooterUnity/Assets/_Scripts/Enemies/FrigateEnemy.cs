using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrigateEnemy : EnemyBase
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject disparoPrefab;

    void Start()
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
            GameObject ray = Instantiate(disparoPrefab, adjustedPosition, Quaternion.identity);
            ray.transform.SetParent(transform);

            Destroy(ray, Random.Range(5, 8));

            while(ray != null)
            {
                yield return null;
            }

            yield return new WaitForSeconds(shotTime);
        }
    }
}
