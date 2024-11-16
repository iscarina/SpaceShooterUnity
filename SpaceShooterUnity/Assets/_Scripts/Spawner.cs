using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TextMeshProUGUI textoOleadas;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < 5; i++)//Niveles
        {
            for (int j = 0; j < 3; j++)//Oleadas
            {
                textoOleadas.text = "Nivel " + (i + 1) + " - " + "Oleada " + (j + 1);
                yield return new WaitForSeconds(2f);
                textoOleadas.text = "";
                for (int k = 0; k < 10; k++) //Enemigos
                {
                    Vector3 puntoAleatorio = new Vector3(transform.position.x, Random.Range(-4.43f, 4.43f), 0);
                    Instantiate(enemyPrefab, puntoAleatorio, Quaternion.identity);
                    yield return new WaitForSeconds(0.5f);
                }
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(5f);
        }
    }

}
