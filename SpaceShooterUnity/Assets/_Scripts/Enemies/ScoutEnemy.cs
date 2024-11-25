using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutEnemy : EnemyBase
{

    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab; // Prefab del proyectil
    [SerializeField] private int projectilesPerBurst = 5; // N�mero de proyectiles por r�faga
    [SerializeField] private float burstAngle = 180f; // �ngulo total de la r�faga en grados
    [SerializeField] private float intervaloRafaga = 0.1f; // Tiempo entre cada proyectil de la r�faga

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
        transform.Translate(Vector3.left * speed * Time.deltaTime); // Mover hacia la izquierda (ajustar seg�n el dise�o del juego)
    }

    IEnumerator ShootBurstRoutine()
    {
        while (isShooting)
        {
            yield return StartCoroutine(ShootBurst());
            yield return new WaitForSeconds(shotTime); // Esperar entre r�fagas
        }
    }

    IEnumerator ShootBurst()
    {
        // Dispara en forma de arco semicircular
        float angleStep = burstAngle / (projectilesPerBurst - 1); // �ngulo entre proyectiles
        float startAngle = -burstAngle / 2; // �ngulo inicial

        for (int i = 0; i < projectilesPerBurst; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            ShootProjectile(currentAngle);

            yield return new WaitForSeconds(intervaloRafaga); // Intervalo entre cada disparo en la r�faga
        }
    }

    void ShootProjectile(float angle)
    {
        // Crear el proyectil
        GameObject projectile = PoolManager.SpawnObject(projectilePrefab, spawnPoint.transform.position, Quaternion.identity);

        // Calcular la direcci�n
        Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.left;

        // Asignar direcci�n y velocidad al proyectil
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 5;
        }
        else
        {
            Debug.LogWarning("El proyectil no tiene un Rigidbody2D.");
        }

        // Opcional: Destruir el proyectil despu�s de un tiempo para evitar acumulaci�n
        Destroy(projectile, 5f);
    }

}
