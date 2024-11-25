using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private Transform[] spawnPoints; // 4 puntos de spawn
    [SerializeField] private GameObject bulletPrefab; // Prefab de proyectiles
    [SerializeField] private GameObject wavePrefab; // Prefab de proyectiles
    [SerializeField] private GameObject enemyPrefab; // Prefab del enemigo para el segundo punto de spawn
    [SerializeField] private GameObject rayPrefab; // Prefab del rayo
    [SerializeField] private float attackInterval = 3f; // Intervalo entre ataques

    [Header("Spawn Point 1 (Circular + Recta)")]
    [SerializeField] private int projectilesPerBurst = 6; // Número de proyectiles por ráfaga
    [SerializeField] private float minBurstAngle = 90f; // Ángulo mínimo de dispersión
    [SerializeField] private float maxBurstAngle = 180f; // Ángulo máximo de dispersión
    [SerializeField] private float burstInterval = 1f; // Tiempo entre ráfagas
    [SerializeField] private float straightLineInterval = 0.2f; // Tiempo entre proyectiles en línea recta

    [Header("Spawn Point 2 (Enemigos)")]
    [SerializeField] private float enemySpawnInterval = 5f; // Tiempo entre spawns de enemigos

    [Header("Spawn Points 3 & 4 (Rayos + Triple Disparo)")]
    [SerializeField] private float tripleShotSpread = 20f; // Separación angular entre los disparos
    [SerializeField] private float randomPatternInterval = 2f; // Intervalo entre ataques aleatorios

    void Start()
    {
        // Asegurar que haya suficientes spawn points
        if (spawnPoints == null || spawnPoints.Length < 4)
        {
            Debug.LogError("You must assign at least 4 spawn points!");
            return;
        }

        // Iniciar el ciclo de cada punto de spawn
        StartCoroutine(SpawnPoint1Cycle());
        StartCoroutine(SpawnPoint2Cycle());
        StartCoroutine(SpawnPoint3And4Cycle(spawnPoints[2])); // Punto 3
        StartCoroutine(SpawnPoint3And4Cycle(spawnPoints[3])); // Punto 4
    }

    // Ciclo del primer spawn point (Ataque circular y línea recta)
    IEnumerator SpawnPoint1Cycle()
    {
        while (true)
        {
            int randomPattern = Random.Range(0, 2); // Alternar entre patrones
            switch (randomPattern)
            {
                case 0:
                    yield return Pattern1_Circular(spawnPoints[0]);
                    break;
                case 1:
                    yield return Pattern1_StraightLine(spawnPoints[0]);
                    break;
            }
            yield return new WaitForSeconds(attackInterval);
        }
    }

    IEnumerator Pattern1_Circular(Transform spawnPoint)
    {
        int bursts = Random.Range(2, 5); // Número de ráfagas aleatorias por ataque
        float burstAngle = Random.Range(minBurstAngle, maxBurstAngle); // Ángulo total aleatorio para cada ráfaga
        float angleStep = burstAngle / (projectilesPerBurst - 1);

        for (int burst = 0; burst < bursts; burst++) // Disparar múltiples ráfagas
        {
            float startAngle = -burstAngle / 2; // Ángulo inicial para esta ráfaga

            for (int i = 0; i < projectilesPerBurst; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                GameObject projectile = PoolManager.SpawnObject(bulletPrefab, spawnPoint.position, Quaternion.identity);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * Vector3.left;
                    rb.velocity = direction * 5f; // Velocidad del proyectil
                }

            }

            yield return new WaitForSeconds(burstInterval); // Pausa entre ráfagas
        }
    }

    IEnumerator Pattern1_StraightLine(Transform spawnPoint)
    {
        for (int i = 0; i < projectilesPerBurst; i++)
        {
            GameObject projectile = PoolManager.SpawnObject(wavePrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = Vector3.left * 10f;
            }

            yield return new WaitForSeconds(straightLineInterval);
        }
    }

    IEnumerator SpawnPoint2Cycle()
    {
        while (true)
        {
            PoolManager.SpawnObject(enemyPrefab, spawnPoints[1].position, Quaternion.identity);
            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }

    IEnumerator SpawnPoint3And4Cycle(Transform spawnPoint)
    {
        while (true)
        {
            int randomPattern = Random.Range(0, 2); // Alternar entre rayo y triple disparo
            switch (randomPattern)
            {
                case 0:
                    yield return Pattern3_TripleShot(spawnPoint);
                    break;
                case 1:
                    yield return Pattern4_RayAttack(spawnPoint);
                    break;
            }
            yield return new WaitForSeconds(randomPatternInterval);
        }
    }

    // Patrón 3: Triple disparo hacia adelante
    IEnumerator Pattern3_TripleShot(Transform spawnPoint)
    {
        float startAngle = -tripleShotSpread; // Ángulo inicial (izquierda)
        float angleStep = tripleShotSpread * 2 / 2; // Paso entre disparos

        for (int i = 0; i < 3; i++) // Tres disparos
        {
            float currentAngle = startAngle + (angleStep * i);
            GameObject projectile = PoolManager.SpawnObject(wavePrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * Vector3.left;
                rb.velocity = direction * 5f;
            }

        }

        yield return null;
    }

    // Patrón 4: Rayo
    IEnumerator Pattern4_RayAttack(Transform spawnPoint)
    {
        Vector3 adjustedPosition = spawnPoint.transform.position + new Vector3(-9.2f, 0, 0);
        GameObject ray = PoolManager.SpawnObject(rayPrefab, adjustedPosition, Quaternion.identity);
        ray.transform.SetParent(transform);

        // Duración del rayo antes de devolverlo al pool
        float duration = Random.Range(3f, 6f);

        // Esperar el tiempo de duración
        yield return new WaitForSeconds(duration);

        // Devolver el rayo al pool
        PoolManager.ReturnObjectToPool(ray);
    }

}