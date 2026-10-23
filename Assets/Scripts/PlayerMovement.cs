
using UnityEngine;
using System.Collections;
using TMPro;
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

    [Header("Dash")]
    [SerializeField] private float velocidadDash = 20f;
    [SerializeField] private float duracionDash = 0.2f;
    [SerializeField] private float cooldownDash = 3f;
    [SerializeField] private TextMeshProUGUI textoCooldownDash;

    [Header("Arma de aturdimiento")]
    [SerializeField] private LayerMask capaEnemigo;
    [SerializeField] private float distanciaAturdimiento = 5f;
    private bool haciendoDash = false;
    private bool puedeUsarDash = true;
    private Vector3 direccionDash;
    private Rigidbody rb;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocidadOriginal = velocidad;
    }

    void Update()
    {
        Debug.Log("Update se está ejecutando");
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direccion = new Vector3(horizontal, 0f, vertical);
        direccion = Vector3.ClampMagnitude(direccion, 1f);

        // Activar el dash con Shift izquierdo
        if (Input.GetKeyDown(KeyCode.LeftShift) && puedeUsarDash)
        {
            direccionDash = direccion.sqrMagnitude > 0.01f
                ? direccion.normalized
                : transform.forward;

            StartCoroutine(RealizarDash());
        }

        // Movimiento normal o dash
        if (haciendoDash)
        {
            transform.Translate(
                direccionDash * velocidadDash * Time.deltaTime,
                Space.World
            );
        }
        else
        {
            transform.Translate(
                direccion * velocidad * Time.deltaTime,
                Space.World
            );
        }

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
        puntoInteraccion.forward * distanciaAturdimiento,
         Color.magenta
           );
        Debug.DrawRay(
            puntoInteraccion.position,
            puntoInteraccion.forward * distanciaInteraccion,
            Color.cyan
        );

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.DrawRay(
                puntoInteraccion.position,
                puntoInteraccion.forward * distanciaAturdimiento,
                Color.red,
                5f
            );

            RaycastHit hitAturdimiento;

            
            if (Physics.Raycast(
                puntoInteraccion.position,
                puntoInteraccion.forward,
                out hitAturdimiento,
                distanciaAturdimiento))
            {
                CentinelaIA centinela =
                    hitAturdimiento.collider.GetComponentInParent<CentinelaIA>();

                if (centinela != null)
                {
                    centinela.Aturdir();
                    Debug.Log("¡Centinela aturdido!");
                }
            }
            else
            {
                Debug.Log("No hay ningún collider delante del jugador.");
            }
        }

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
    private IEnumerator RealizarDash()
    {
        puedeUsarDash = false;
        haciendoDash = true;

        if (textoCooldownDash != null)
            textoCooldownDash.text = "DASH!";

        yield return new WaitForSeconds(duracionDash);

        haciendoDash = false;

        float tiempoRestante = cooldownDash;

        while (tiempoRestante > 0)
        {
            if (textoCooldownDash != null)
                textoCooldownDash.text = "Recarga: " +
                    tiempoRestante.ToString("F1") + " s";

            yield return new WaitForSeconds(0.1f);
            tiempoRestante -= 0.1f;
        }

        puedeUsarDash = true;

        if (textoCooldownDash != null)
            textoCooldownDash.text = "Dash Listo";
    }
    private void OnDrawGizmos()
    {
        // Raycast del suelo
        if (puntoChequeoSuelo != null)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(
                puntoChequeoSuelo.position,
                puntoChequeoSuelo.position + Vector3.down * distanciaSuelo
            );

            Gizmos.DrawSphere(puntoChequeoSuelo.position, 0.04f);
        }

        // Raycast del arma de aturdimiento
        if (puntoInteraccion != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawRay(
                puntoInteraccion.position,
                puntoInteraccion.forward * distanciaAturdimiento
            );
        }
    }

}
