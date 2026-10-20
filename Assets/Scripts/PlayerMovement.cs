
using UnityEngine;
using System.Collections;

public class PlayerMovements : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float velocidad = 5f;
    private float velocidadOriginal;
    [SerializeField] private float fuerzaSalto = 5f;

    [Header("Ground Raycast")]
    [SerializeField] private Transform puntoChequeoSuelo;
    [SerializeField] private float distanciaSuelo = 0.2f;
    [SerializeField] private LayerMask capaSuelo;

    [Header("Interaction Raycast")]
    [SerializeField] private Transform puntoInteraccion;
    [SerializeField] private float distanciaInteraccion = 3f;
    [SerializeField] private LayerMask capaInteractiva;
    private Rigidbody rb;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocidadOriginal = velocidad;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direccion = new Vector3(horizontal, 0f, vertical);

        transform.Translate(
            direccion * velocidad * Time.deltaTime,
            Space.World
        );

        // Comprobar si hay suelo debajo del jugador
        enSuelo = Physics.Raycast(
            puntoChequeoSuelo.position,
            Vector3.down,
            distanciaSuelo,
            capaSuelo
        );

        // Saltar solamente cuando está en el suelo
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.AddForce(
                Vector3.up * fuerzaSalto,
                ForceMode.Impulse
            );
        }

        // Mostrar el Raycast en la ventana Scene
        Debug.DrawRay(
            puntoChequeoSuelo.position,
            Vector3.down * distanciaSuelo,
            enSuelo ? Color.green : Color.red
        );
        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(
                Physics.gravity * 0.5f,
                ForceMode.Acceleration
            );
        }
        RaycastHit hit;

        if (Physics.Raycast(
            puntoInteraccion.position,
            puntoInteraccion.forward,
            out hit,
            distanciaInteraccion,
            capaInteractiva))
        {
            Debug.Log("Objeto interactivo detectado: " + hit.collider.name);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("¡Interactuaste con: " + hit.collider.name + "!");
            }
        }

        Debug.DrawRay(
            puntoInteraccion.position,
            puntoInteraccion.forward * distanciaInteraccion,
            Color.cyan
        );
    }

    public void CambiarVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }

    public void RestaurarVelocidad()
    {
        velocidad = velocidadOriginal;
        Debug.Log("Velocidad restaurada a: " + velocidad);
    }

    private void OnDrawGizmos()
    {
        if (puntoChequeoSuelo == null)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(
            puntoChequeoSuelo.position,
            puntoChequeoSuelo.position + Vector3.down * distanciaSuelo
        );

        Gizmos.DrawSphere(puntoChequeoSuelo.position, 0.04f);
    }

}
