using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public delegate void LifeChangeHandler(float newLife);
    public event LifeChangeHandler OnLifeChange;

    private float _life;
    [SerializeField] private float vidaMaxima = 30f;
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

    [Header("Damage")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;
    private bool isInvulnerable = false;

    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteSlightDamaged;
    [SerializeField] private Sprite spriteDamaged;
    [SerializeField] private Sprite spriteVeryDamaged;

    private Color originalColor;

    private bool tieneEscudo = false;

    private float temporizador = 0.5f;

    private float crashShip = 2f;

    private int pickUpRockets = 0;

    private static int score;

    public static int Score { get => score; set => score = value; }

    //Rayo
    private bool isOnRay = false;
    private float timeSinceLastDamage = 0f; 
    private float damageInterval = 0.3f;


    private bool isDead = false;
    void Start()
    {
        OnLifeChange += UpdateLife;
        life = vidaMaxima;
        score = 0;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (!isDead)
        {
            Movimiento();
            DelimitaMovimiento();
            Disparar();
            if (isOnRay)
            {
                RayDamage();
            }
            UpdateScore();
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

        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && temporizador > ratioDisparo)
        {
            AudioManager.Instance.PlaySFX(AudioManager.SOUNDS[AudioManager.SOUNDS_ENUM.ShotPlayer]);
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
                if (!isInvulnerable) //Tiempo cortesia para que sea mas justo si le hacen daño en 0.1s no le pueden volver a hacer.
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
                PoolManager.ReturnObjectToPool(collision.gameObject);
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
        AudioManager.Instance.PlaySFX(AudioManager.SOUNDS[AudioManager.SOUNDS_ENUM.Hit]);
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(0.15f); //Tiempo cortesia para que sea mas justo.

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
            // Aplicar daño
            life -= 1f;
            StartCoroutine(FlashRed());
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
            AudioManager.Instance.PlaySFX(AudioManager.SOUNDS[AudioManager.SOUNDS_ENUM.PowerUp]);
        }
        else if (collision.gameObject.CompareTag("PickupVida"))
        {
            if (life < vidaMaxima)
            {
                life++;
                PoolManager.ReturnObjectToPool(collision.gameObject);
                AudioManager.Instance.PlaySFX(AudioManager.SOUNDS[AudioManager.SOUNDS_ENUM.PowerUp]);
            }
        }
        else if (collision.gameObject.CompareTag("PickupVelocidad"))
        {
            if (ratioDisparo > 0.2f)
            {
                ratioDisparo = ratioDisparo - 0.1f;
            }
            else
            {
                ratioDisparo = ratioDisparo - 0.01f;
            }
            PoolManager.ReturnObjectToPool(collision.gameObject);
            AudioManager.Instance.PlaySFX(AudioManager.SOUNDS[AudioManager.SOUNDS_ENUM.PowerUp]);
        }
        else if (collision.gameObject.CompareTag("PickupRockets"))
        {
            pickUpRockets++;
            PoolManager.ReturnObjectToPool(collision.gameObject);
            AudioManager.Instance.PlaySFX(AudioManager.SOUNDS[AudioManager.SOUNDS_ENUM.PowerUp]);
        }

    }

    void UpdateLife(float life)
    {
        vidaTexto.text = "Vida: " + life;
        if (life <= 0)
        {
            isDead = true;
            StartCoroutine(DestroyAfterAnimation());
        }
        else if (life <= 22)
        {
            spriteRenderer.sprite = spriteSlightDamaged;
            if (life <= 15)
            {
                spriteRenderer.sprite = spriteDamaged;
                if (life <= 8)
                {
                    spriteRenderer.sprite = spriteVeryDamaged;
                }
            }
        }
        else
        {
            spriteRenderer.sprite = spriteNormal;
        }

    }

    private IEnumerator DestroyAfterAnimation()
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
        foreach (Transform child in transform)
        {
            if (child.name != "Die")
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }

        yield return new WaitForSeconds(1f);
        foreach (Transform child in transform)
        {
                child.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1.5f);
        gameOver.SetActive(true);
        Destroy(this.gameObject);
    }

    void UpdateScore()
    {
        pointsUI.text = "Score: " + score;
    }

}
