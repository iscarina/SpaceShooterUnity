using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoEnemy : EnemyBase
{

    [SerializeField] private float aimDuration = 0.5f;    // Duración de tiempo que el enemigo apunta al jugador

    private Transform player;  // Referencia al jugador

    //void Start()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player").transform;
    //    StartCoroutine(MoveAndCharge());
    //}

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(MoveAndCharge());
    }

    IEnumerator MoveAndCharge()
    {
        // Mover hacia adelante durante 1 segundo
        float moveEndTime = Time.time + 1f;
        while (Time.time < moveEndTime)
        {
            transform.Translate(Vector3.left * Random.Range(2, 5) * Time.deltaTime);

            yield return null;
        }

        // Apuntar al jugador durante aimDuration segundos
        float aimEndTime = Time.time + aimDuration;
        while (Time.time < aimEndTime)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.z = 0;
            directionToPlayer.Normalize();
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));

            yield return null;
        }

        // Cargar hacia el jugador tomando como referencia el morro de la nave
        while (this.gameObject.activeInHierarchy)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);  // Usar Vector3.right para moverse hacia adelante según la rotación actual
            yield return null;
        }
    }

}
