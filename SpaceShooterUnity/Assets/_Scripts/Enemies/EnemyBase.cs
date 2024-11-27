using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] protected float health = 1f;
    [SerializeField] protected float maxHealth = 1f;
    [SerializeField] protected float speed;

    [SerializeField] protected float shotTime = 1f;

    [SerializeField] protected int score;

    [SerializeField] GameObject SpriteVisual;

    [Header("Move in Y")]
    protected float nextMoveTime = 0f;
    protected float moveDuration = 2f;
    protected bool isMovingY = false;
    protected float moveDirection = 1f;

    [SerializeField] protected float minTimeBetweenMove;
    [SerializeField] protected float maxTimeBetweenMove;

    private Color originalColor;
    protected bool isDead = false;

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;

        PlayerController.Score += score;

        StartCoroutine(DestroyAfterAnimation());

    }

    private IEnumerator DestroyAfterAnimation()
    {
        AudioManager.Instance.PlaySFX(AudioManager.SOUNDS[AudioManager.SOUNDS_ENUM.Die]);
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

        DropPowerUp();
        PoolManager.ReturnObjectToPool(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DisparoPlayer"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.SOUNDS[AudioManager.SOUNDS_ENUM.Hit]);
            TakeDamage(collision.gameObject.GetComponent<Disparo>().damage);
            PoolManager.ReturnObjectToPool(collision.gameObject);
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {

        Color customColor = new Color(255f / 255f, 83f / 255f, 83f / 255f);

        SpriteVisual.GetComponent<SpriteRenderer>().color = customColor;

        yield return new WaitForSeconds(0.2f);

        SpriteVisual.GetComponent<SpriteRenderer>().color = Color.white;
    }

    //Para que se mueva en Y de vez en cuando (se ha de llamar desde cada enemigo en caso de querer moverse en Y)
    protected void SetNextMoveTime()
    {
        nextMoveTime = Time.time + Random.Range(minTimeBetweenMove, maxTimeBetweenMove);
    }

    protected IEnumerator MoveInY()
    {
        isMovingY = true;

        moveDirection = Random.Range(0f, 1f) > 0.5f ? 1f : -1f;

        float moveEndTime = Time.time + moveDuration;

        while (Time.time < moveEndTime)
        {
            transform.Translate(Vector3.up * moveDirection * speed * Time.deltaTime);
            float yClamped = Mathf.Clamp(transform.position.y, -4.43f, 4.43f);
            transform.position = new Vector3(transform.position.x, yClamped, 0);
            yield return null;
        }

        isMovingY = false;

        SetNextMoveTime();
    }

    void DropPowerUp()
    {
        GameObject powerUpPrefab = PowerUpManager.Instance.GetRandomPowerUp();


        if (powerUpPrefab != null)
        {
            PoolManager.SpawnObject(powerUpPrefab, transform.position, Quaternion.identity);
        }

    }

    protected void EnableEnemy()
    {
        isDead = false;
        SpriteVisual.GetComponent<SpriteRenderer>().color = Color.white;
        health = maxHealth;

        transform.GetComponent<BoxCollider2D>().enabled = true;
        foreach (Transform child in transform)
        {
            if (child.name != "Die")
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

}
