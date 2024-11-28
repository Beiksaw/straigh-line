using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocoEnJugador : MonoBehaviour
{
    // Asigna el objeto jugador desde el Inspector
    public Transform jugador;

    void Update()
    {
        if (jugador != null)
        {
            // Calcula la dirección desde la luz hacia el jugador
            Vector3 direccionHaciaJugador = jugador.position - transform.position;
            
            // Ajusta la rotación de la luz para mirar hacia el jugador
            transform.rotation = Quaternion.LookRotation(direccionHaciaJugador);
        }
    }
}

