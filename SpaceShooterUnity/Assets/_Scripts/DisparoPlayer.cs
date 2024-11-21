using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoPlayer : Disparo
{
    void Update()
    {
        transform.Translate(direccion * speed * Time.deltaTime);
    }
}
