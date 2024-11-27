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

    [SerializeField] private GameObject finalBoss;
    [SerializeField] private GameObject finalBossEnemy1;
    [SerializeField] private GameObject finalBossEnemy2;
    [SerializeField] private GameObject finalBossEnemy3;

    [SerializeField] private GameObject win;
    [SerializeField] private TextMeshProUGUI scoreText;

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
            textoOleadas.text = "Level " + (levellIndex + 1) + " - Wave " + (j + 1);
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
        textoOleadas.text = "GET REDY !";
        yield return new WaitForSeconds(3f);
        textoOleadas.text = "";

        finalBoss.SetActive(true);

        while (finalBossEnemy1 != null || finalBossEnemy2 != null || finalBossEnemy3 != null)
        {
            yield return null;   
        }

        yield return new WaitForSeconds(3f);

        scoreText.text = "Score: " + PlayerController.Score;
        finalBoss.SetActive(false);
        win.SetActive(true);
    }

}
