using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{

    //[SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TextMeshProUGUI textoOleadas;

    [SerializeField] private SO_Nivel[] niveles;

    [SerializeField] private float timeBetweenLevels;
    [SerializeField] private float timeBetweenWaves;

    [SerializeField] GameObject finalBoss;

    public static int deadEnemies;

    void Start()
    {
        StartCoroutine(SpawnLevels());
    }

    IEnumerator SpawnLevels()
    {
        for (int i = 0; i < niveles.Length; i++) // Niveles
        {
            yield return StartCoroutine(SpawnOleadas(i)); // Llamada a las oleadas
            yield return new WaitForSeconds(timeBetweenLevels);
            //Espero a que no haya enemigos en escena para pasar de nivel
            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                yield return new WaitForSeconds(timeBetweenLevels);
            }
        }
        StartCoroutine(FinalBoss());
    }

    IEnumerator SpawnOleadas(int levellIndex)
    {
        for (int j = 0; j < niveles[levellIndex].waves; j++) // Oleadas
        {
            textoOleadas.text = "Nivel " + (levellIndex + 1) + " - Oleada " + (j + 1);
            yield return new WaitForSeconds(2f);
            textoOleadas.text = "";

            // Llamar a los enemigos
            yield return StartCoroutine(SpawnEnemies(levellIndex, j));
            //Espero a que no haya enemigos en escena para pasar de oleada
            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }

    IEnumerator SpawnEnemies(int levellIndex, int waveIndex)
    {
        GameObject[] enemies = niveles[levellIndex].enemiesPrefabs;

        for (int k = 0; k < niveles[levellIndex].enemiesNumber[waveIndex]; k++) // Enemigos
        {
            // Selecciona aleatoriamente un enemigo
            GameObject enemyPrefab = enemies[Random.Range(0, enemies.Length)];

            Vector3 spawnPosition = new Vector3(transform.position.x, Random.Range(-4.43f, 4.43f), 0);
            PoolManager.SpawnObject(enemyPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(niveles[levellIndex].enemiesDelay[waveIndex]);
        }
    }

    IEnumerator FinalBoss()
    {
        textoOleadas.text = "FINAL BOSS !";
        yield return new WaitForSeconds(3f);
        finalBoss.SetActive(true);

        while (finalBoss.activeInHierarchy)
        {
            yield return null;
        }

        Debug.Log("WIN");
    }

}
