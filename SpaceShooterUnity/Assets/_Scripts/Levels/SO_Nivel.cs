using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nivel_", menuName = "Crear nivel")]
public class SO_Nivel : ScriptableObject
{
    public GameObject[] enemiesPrefabs;

    public int waves;
    public int[] enemiesNumber;
    public float[] enemiesDelay;
}
