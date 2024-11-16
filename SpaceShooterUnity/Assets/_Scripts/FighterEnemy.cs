using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEnemy : MonoBehaviour
{

    [SerializeField] private float speed;

    [SerializeField] private GameObject spawnPoint1;
    [SerializeField] private GameObject spawnPoint2;
    [SerializeField] private GameObject disparoPrefab;

    void Start()
    {
        StartCoroutine(Disparar());
    }

    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime);
    }

    IEnumerator Disparar()
    {
        while(true)
        {
            int spawnPointIndex = Random.Range(0, 2);

            if (spawnPointIndex == 0)
            {
                Instantiate(disparoPrefab, spawnPoint1.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(disparoPrefab, spawnPoint2.transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DisparoPlayer"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

}
