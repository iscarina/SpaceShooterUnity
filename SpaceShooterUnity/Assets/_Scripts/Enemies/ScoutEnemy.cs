using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutEnemy : EnemyBase
{

    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab; // Prefab del proyectil
    [SerializeField] private int projectilesPerBurst = 5; // Número de proyectiles por ráfaga
    [SerializeField] private float burstAngle = 180f; // Ángulo total de la ráfaga en grados
    [SerializeField] private float intervaloRafaga = 0.1f; // Tiempo entre cada proyectil de la ráfaga

    [SerializeField] private GameObject spawnPoint; // Punto de origen de los proyectiles (opcional)

    private bool isShooting = true;

    void Start()
    {
        StartCoroutine(ShootBurstRoutine());
    }

    private void OnEnable()
    {
        StartCoroutine(ShootBurstRoutine());
    }

    void Update()
    {
        // Movimiento hacia adelante
        transform.Translate(Vector3.left * speed * Time.deltaTime); // Mover hacia la izquierda (ajustar según el diseño del juego)
    }

    IEnumerator ShootBurstRoutine()
    {
        while (isShooting)
        {
            yield return StartCoroutine(ShootBurst());
            yield return new WaitForSeconds(shotTime); // Esperar entre ráfagas
        }
    }

    IEnumerator ShootBurst()
    {
        // Dispara en forma de arco semicircular
        float angleStep = burstAngle / (projectilesPerBurst - 1); // Ángulo entre proyectiles
        float startAngle = -burstAngle / 2; // Ángulo inicial

        for (int i = 0; i < projectilesPerBurst; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            ShootProjectile(currentAngle);

            yield return new WaitForSeconds(intervaloRafaga); // Intervalo entre cada disparo en la ráfaga
        }
    }

    void ShootProjectile(float angle)
    {
        // Crear el proyectil
        GameObject projectile = PoolManager.SpawnObject(projectilePrefab, spawnPoint.transform.position, Quaternion.identity);

        // Calcular la dirección
        Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.left;

        // Asignar dirección y velocidad al proyectil
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 5;
        }
        else
        {
            Debug.LogWarning("El proyectil no tiene un Rigidbody2D.");
        }

        // Opcional: Destruir el proyectil después de un tiempo para evitar acumulación
        Destroy(projectile, 5f);
    }

}
