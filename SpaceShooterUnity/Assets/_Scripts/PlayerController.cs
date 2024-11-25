using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public delegate void LifeChangeHandler(float newLife);
    public event LifeChangeHandler OnLifeChange;

    private float _life;
    private float vidaMaxima = 20;
    public float life
    {
        get => _life;
        set
        {
            _life = value;
            OnLifeChange?.Invoke(_life); // Disparar evento al cambiar la vida
        }
    }

    [Header("Basics")]
    [SerializeField] private float speed;
    [SerializeField] private GameObject disparoPrefab;
    [SerializeField] private float ratioDisparo;
    [SerializeField] private GameObject escudo;

    [Header("SpawnPoints")]
    [SerializeField] private GameObject spawnPoint1;
    [SerializeField] private GameObject spawnPoint2;
    [SerializeField] private GameObject spawnPoint3;
    [SerializeField] private GameObject spawnPoint4;
    [SerializeField] private GameObject spawnPoint5;
    [SerializeField] private GameObject spawnPoint6;
    [SerializeField] private GameObject spawnPoint7;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI vidaTexto;
    [SerializeField] private TextMeshProUGUI pointsUI;
    [SerializeField] private GameObject gameOver;

    [Header("RED")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;
    private bool isInvulnerable = false;

    private Color originalColor;

    private bool tieneEscudo = false;

    private float temporizador = 0.5f;

    private float crashShip = 2f;

    private int pickUpRockets = 0;

    public static int score = 0;

    //Rayo
    private bool isOnRay = false;
    private float timeSinceLastDamage = 0f; 
    private float damageInterval = 0.3f; 

    void Start()
    {
        OnLifeChange += UpdateLife;
        life = 20;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        Movimiento();
        DelimitaMovimiento();
        Disparar();
        if (isOnRay)
        {
            RayDamage();
        }
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
            PoolManager.SpawnObject(disparoPrefab, spawnPoint1.transform.position, Quaternion.identity);
            PoolManager.SpawnObject(disparoPrefab, spawnPoint2.transform.position, Quaternion.identity);

            if (pickUpRockets >= 1)
            {
                PoolManager.SpawnObject(disparoPrefab, spawnPoint3.transform.position, Quaternion.identity);
                PoolManager.SpawnObject(disparoPrefab, spawnPoint4.transform.position, Quaternion.identity);
                if (pickUpRockets > 1)
                {
                    PoolManager.SpawnObject(disparoPrefab, spawnPoint5.transform.position, Quaternion.identity);
                    PoolManager.SpawnObject(disparoPrefab, spawnPoint6.transform.position, Quaternion.identity);
                    if (pickUpRockets > 2)
                    {
                        PoolManager.SpawnObject(disparoPrefab, spawnPoint7.transform.position, Quaternion.identity);
                    }
                }
                
            }

            temporizador = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DisparoEnemigo") || collision.gameObject.CompareTag("Enemy"))
        {
            if (!tieneEscudo)
            {
                if (!isInvulnerable) //Tiempo cortesia para que sea mas justo si le hacen da�o en 0.1s no le pueden volver a hacer.
                {
                    isInvulnerable = true;
                
                    if (collision.gameObject.CompareTag("DisparoEnemigo"))
                    {
                        life -= collision.gameObject.GetComponent<Disparo>().damage;
                    }
                    else
                    {
                        life -= crashShip;
                    }
                    StartCoroutine(FlashRed());
                }
                PoolManager.ReturnObjectToPool(collision.gameObject);

                Spawner.deadEnemies++;
            }
            else
            {
                escudo.SetActive(false);
                tieneEscudo = false;
            }
        }
        else if (collision.gameObject.CompareTag("Ray"))
        {
            isOnRay = true;
        }

        PickUps(collision);

    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(0.1f); //Tiempo cortesia para que sea mas justo.

        isInvulnerable = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ray"))
        {
            isOnRay = false;
        }
    }

    void RayDamage()
    {
        timeSinceLastDamage += Time.deltaTime;

        if (timeSinceLastDamage >= damageInterval)
        {
            // Aplicar da�o
            life -= 1f;

            // Resetear el temporizador
            timeSinceLastDamage = 0f;
        }
    }

    void PickUps(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PickupEscudo"))
        {
            escudo.SetActive(true);
            tieneEscudo = true;
            PoolManager.ReturnObjectToPool(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("PickupVida"))
        {
            if (life < vidaMaxima)
            {
                life++;
                PoolManager.ReturnObjectToPool(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("PickupVelocidad"))
        {
            ratioDisparo = ratioDisparo - 0.1f;
            PoolManager.ReturnObjectToPool(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("PickupRockets"))
        {
            pickUpRockets++;
            PoolManager.ReturnObjectToPool(collision.gameObject);
        }

    }

    void UpdateLife(float life)
    {
        vidaTexto.text = "Vida: " + life;
        if (life <= 0)
        {
            gameOver.SetActive(true);
            Destroy(this.gameObject);
        }

    }

    void UpdateScore()
    {
        pointsUI.text = "Score: " + score;
    }

}
