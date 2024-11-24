using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] protected float health = 1f;
    [SerializeField] protected float speed;

    [SerializeField] protected float shotTime = 1f;

    [SerializeField] protected int score;

    [Header("Move in Y")]
    protected float nextMoveTime = 0f;
    protected float moveDuration = 2f;
    protected bool isMovingY = false;
    protected float moveDirection = 1f;

    [SerializeField] protected float minTimeBetweenMove;
    [SerializeField] protected float maxTimeBetweenMove;

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
        PlayerController.score += score;
        DropPowerUp();
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DisparoPlayer"))
        {
            TakeDamage(collision.gameObject.GetComponent<Disparo>().damage);
            Destroy(collision.gameObject);
        }
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
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }

    }

}
