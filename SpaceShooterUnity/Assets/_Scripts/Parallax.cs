using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{

    [SerializeField] private float velocidad;
    [SerializeField] private Vector3 direccion;
    [SerializeField] private float anchoImagen;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Resto: cuanto queda de recorrido para alcanzar el nuevo ciclo
        float resto = (velocidad * Time.time) % anchoImagen;

        //Mi posición se va refrescando desde la inicial sumando tanto como resto me quede en la dirección deseada.
        transform.position = posicionInicial + resto * direccion;

    }
}
