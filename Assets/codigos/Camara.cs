using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    // Asigna el objeto jugador desde el Inspector
    public Transform jugador;

    // Distancia fija en el eje Z (opcional, por si quieres que la cámara esté a cierta profundidad)
    public float distanciaZ = -10.0f;

    void Update()
    {
        if (jugador != null)
        {
            // Coloca la cámara en la posición del jugador en X y Y, manteniendo distancia en Z
            Vector3 posicionCamara = new Vector3(jugador.position.x, jugador.position.y, distanciaZ);
            transform.position = posicionCamara;
        }
    }
}


