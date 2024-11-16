using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private Vector3 direccion;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(direccion * speed * Time.deltaTime);
    }
}
