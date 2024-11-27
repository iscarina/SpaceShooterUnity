using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemie : EnemyBase
{
    [SerializeField] private GameObject spawnPoint;
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
        while (true)
        {
            AudioManager.Instance.PlaySFX(AudioManager.SOUNDS[AudioManager.SOUNDS_ENUM.ShotEnemy]);
            PoolManager.SpawnObject(disparoPrefab, spawnPoint.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(shotTime);
        }
    }

}
