using UnityEngine;

public class ParallaxEfecto : MonoBehaviour
{
    public Transform camara; // Referencia a la cámara o el jugador
    public Vector2 velocidadParallax; // Velocidad de parallax en el eje X e Y

    private Vector3 posicionAnteriorCamara;

    void Start()
    {
        // Guarda la posición inicial de la cámara
        posicionAnteriorCamara = camara.position;
    }

    void Update()
    {
        // Calcula el movimiento de la cámara
        Vector3 deltaMovimiento = camara.position - posicionAnteriorCamara;

        // Aplica el movimiento de parallax
        transform.position += new Vector3(deltaMovimiento.x * velocidadParallax.x, deltaMovimiento.y * velocidadParallax.y, 0);

        // Actualiza la posición anterior de la cámara
        posicionAnteriorCamara = camara.position;
    }
}

