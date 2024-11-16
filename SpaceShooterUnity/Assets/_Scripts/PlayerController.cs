using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private GameObject disparoPrefab;

    [SerializeField] private GameObject spawnPoint1;
    [SerializeField] private GameObject spawnPoint2;

    [SerializeField] private float ratioDisparo;

    private float vidas = 10;

    private float temporizador = 0.5f;

    void Start()
    {
        
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

            temporizador = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DisparoEnemigo") || collision.gameObject.CompareTag("FighterEnemy"))
        {
            vidas -= 1;
            Destroy(collision.gameObject);

            if (vidas <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

}
