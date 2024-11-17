using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private GameObject disparoPrefab;

    [SerializeField] private GameObject spawnPoint1;
    [SerializeField] private GameObject spawnPoint2;
    [SerializeField] private GameObject spawnPoint3;
    [SerializeField] private GameObject spawnPoint4;
    [SerializeField] private GameObject spawnPoint5;
    [SerializeField] private GameObject spawnPoint6;
    [SerializeField] private GameObject spawnPoint7;

    [SerializeField] private float ratioDisparo;

    [SerializeField] private GameObject escudo;

    [SerializeField] private TextMeshProUGUI vidaTexto;

    private bool tieneEscudo = false;

    private float vida = 10;
    private float vidaMaxima = 10;

    private float temporizador = 0.5f;

    private int pickUpRockets = 0;

    void Start()
    {
        ActulizaVida();
    }

    void Update()
    {
        Movimiento();
        DelimitaMovimiento();
        Disparar();
    }

    void Movimiento()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector2(inputH, inputV).normalized * speed * Time.deltaTime);
    }

    void DelimitaMovimiento()
    {
        float xClamped = Mathf.Clamp(transform.position.x, -8.3f, 8.3f);
        float yClamped = Mathf.Clamp(transform.position.y, -4.43f, 4.43f);
        transform.position = new Vector3(xClamped, yClamped, 0);
    }

    void Disparar()
    {

        temporizador += 1 * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && temporizador > ratioDisparo)
        {
            Instantiate(disparoPrefab, spawnPoint1.transform.position, Quaternion.identity);
            Instantiate(disparoPrefab, spawnPoint2.transform.position, Quaternion.identity);

            if (pickUpRockets >= 1)
            {
                Instantiate(disparoPrefab, spawnPoint3.transform.position, Quaternion.identity);
                Instantiate(disparoPrefab, spawnPoint4.transform.position, Quaternion.identity);
                if (pickUpRockets > 1)
                {
                    Instantiate(disparoPrefab, spawnPoint5.transform.position, Quaternion.identity);
                    Instantiate(disparoPrefab, spawnPoint6.transform.position, Quaternion.identity);
                    if (pickUpRockets > 2)
                    {
                        Instantiate(disparoPrefab, spawnPoint7.transform.position, Quaternion.identity);
                    }
                }
                
            }

            temporizador = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DisparoEnemigo") || collision.gameObject.CompareTag("FighterEnemy"))
        {
            if (!tieneEscudo)
            {
                vida -= 1;
                ActulizaVida();
                Destroy(collision.gameObject);

                if (vida <= 0)
                {
                    ActulizaVida();
                    Destroy(this.gameObject);
                }

                Spawner.deadEnemies++;
            }
            else
            {
                escudo.SetActive(false);
                tieneEscudo = false;
            }
        }

        PickUps(collision);

    }

    void PickUps(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PickupEscudo"))
        {
            escudo.SetActive(true);
            tieneEscudo = true;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("PickupVida"))
        {
            if (vida < vidaMaxima)
            {
                vida++;
                ActulizaVida();
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("PickupVelocidad"))
        {
            ratioDisparo = ratioDisparo - 0.1f;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("PickupRockets"))
        {
            pickUpRockets++;
            Destroy(collision.gameObject);
        }

    }

    void ActulizaVida()
    {
        vidaTexto.text = "Vida: " + vida;
    }

}
