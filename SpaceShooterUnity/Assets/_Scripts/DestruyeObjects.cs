using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruyeObjects : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Ray"))
        {
            Destroy(collision.gameObject);
        }
        
    }
}
