using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoBombEnemie : Disparo
{

    [SerializeField] private float oscilacionFrecuencia; // Oscilaciones seg
    [SerializeField] private float oscilacionAmplitud; // Amplitud oscilaciones

    private float tiempoInicial;

    void Start()
    {
        tiempoInicial = Time.time;
    }

    void Update()
    {
        // Movimiento hacia la izquierda
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Movimiento en Y basado en una onda seno
        float tiempoTranscurrido = Time.time - tiempoInicial;
        float oscilacion = Mathf.Sin(tiempoTranscurrido * oscilacionFrecuencia) * oscilacionAmplitud;

        // Mantener dentro de los límites sin afectar la naturaleza del movimiento
        float yOriginal = Mathf.Clamp(transform.position.y, -4.43f + oscilacionAmplitud, 4.43f - oscilacionAmplitud);
        transform.position = new Vector3(transform.position.x, yOriginal + oscilacion, transform.position.z);

    }

}
