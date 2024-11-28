using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPersonaje : MonoBehaviour
{
    public float velocidadMovimiento = 5.0f;
    
    private Vector3 moveDirectional;
    public GameObject playerModel;

    public float fuerzaSalto = 7.0f;

    public float alturaNormal = 2.0f;
    public float alturaAgachado = 1.0f;
    public bool estaAgachado = false;
    private CapsuleCollider col;

    private bool estaEnSuelo;
    public Transform detectorSuelo;
    public float radioDeteccionSuelo = 0.2f;
    public LayerMask capaSuelo;

    public float fuerzaEmpuje = 10.0f;
    private bool estaCercaDeObjeto = false;
    private GameObject objetoEmpujable;

    private bool estaCercaDeBoton = false;
    private GameObject botonInteractuable;

    private Rigidbody rb;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        if (animator == null)
        {
            Debug.LogError("Animator no asignado en el Inspector.");
        }
    }

    void Update()
    {
        moveDirectional = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f);
        transform.position += moveDirectional * Time.deltaTime * velocidadMovimiento;

        if (moveDirectional != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.Euler(0, Mathf.Atan2(moveDirectional.x, moveDirectional.z) * Mathf.Rad2Deg, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5f);
        }

        if (animator != null)
        {
            animator.SetFloat("Velx", moveDirectional.magnitude);
        }

        estaEnSuelo = Physics.CheckSphere(detectorSuelo.position, radioDeteccionSuelo, capaSuelo);

        if (Input.GetButtonDown("Jump") && estaEnSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse); 
            animator.SetBool("Jump", true);
        }

        if (!estaEnSuelo && rb.velocity.y < 0)
        {
            if (animator != null)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("falling", true);
            }
        }

        if (estaEnSuelo)
        {
            if (animator != null)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("falling", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Agacharse();
            if (animator != null) animator.SetBool("Kneeling", true);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            Levantarse();
            if (animator != null)
            {
                animator.SetBool("Kneeling", false);
                animator.SetBool("Standing", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && estaCercaDeBoton)
        {
            PulsarBoton();
        }

        if (estaCercaDeObjeto && Input.GetKey(KeyCode.E))
        {
            EmpujarObjeto();
        }

        if (Input.GetKeyDown(KeyCode.R)) // Reinicia el nivel al presionar la tecla "R"
        {
        ReiniciarEscena();
        }
    }

    void Agacharse()
    {
        if (!estaAgachado)
        {
            col.height = alturaAgachado;
            estaAgachado = true;
        }
    }

    void Levantarse()
    {
        if (estaAgachado)
        {
            col.height = alturaNormal;
            estaAgachado = false;
        }
    }

    void PulsarBoton()
    {
        Debug.Log("Botón presionado");
    }

    void EmpujarObjeto()
    {
        if (objetoEmpujable != null)
        {
            Rigidbody rbObjeto = objetoEmpujable.GetComponent<Rigidbody>();
            if (rbObjeto != null)
            {
                rbObjeto.AddForce(transform.forward * fuerzaEmpuje, ForceMode.Force);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Empujable"))
        {
            estaCercaDeObjeto = true;
            objetoEmpujable = other.gameObject;
        }

        if (other.CompareTag("Boton"))
        {
            estaCercaDeBoton = true;
            botonInteractuable = other.gameObject;
        }

        if (other.CompareTag("Peligro")) // Si el objeto tiene la etiqueta "Peligro"
        {
            ReiniciarEscena(); // Reinicia el nivel
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Empujable"))
        {
            estaCercaDeObjeto = false;
            objetoEmpujable = null;
        }

        if (other.CompareTag("Boton"))
        {
            estaCercaDeBoton = false;
            botonInteractuable = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            estaEnSuelo = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            estaEnSuelo = false;
        }
    }

    // Llama a esta función para reiniciar el nivel
    public void ReiniciarEscena()
    {
        // Obtiene el índice o el nombre de la escena actual y la recarga
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}



