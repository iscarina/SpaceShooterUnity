using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance; 
    [SerializeField] private GameObject[] powerUps; 

    private void Awake()
    {
        // Configurar el Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public GameObject GetRandomPowerUp()
    {
        float perc = Random.value;

        if (perc < 0.01f)
        {
            return powerUps[0];
        }
        else if (perc < 0.02f)
        {
            return powerUps[1];
        }
        else if (perc <  0.10f)
        {
            return powerUps[2];
        }
        else if (perc < 0.18f)
        {
            return powerUps[3];
        }
        else
        {
            return null;
        }
    }
}
