
using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerMovements : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float fuerzaSalto = 5f;

    private float velocidadOriginal;
    private Rigidbody rb;
    private bool enSuelo;

    [Header("Raycast del suelo")]
    [SerializeField] private Transform puntoChequeoSuelo;
    [SerializeField] private float distanciaSuelo = 0.2f;
    [SerializeField] private LayerMask capaSuelo;

    [Header("Interacción")]
    [SerializeField] private Transform puntoInteraccion;
    [SerializeField] private float distanciaInteraccion = 3f;
    [SerializeField] private LayerMask capaInteractiva;

    [Header("Dash")]
    [SerializeField] private float velocidadDash = 20f;
    [SerializeField] private float duracionDash = 0.2f;
    [SerializeField] private float cooldownDash = 3f;
    [SerializeField] private TextMeshProUGUI textoCooldownDash;

    private bool haciendoDash = false;
    private bool puedeUsarDash = true;
    private Vector3 direccionDash;

    [Header("Arma de aturdimiento")]
    [SerializeField] private float distanciaAturdimiento = 5f;

    [Header("Laser de apuntado")]
    [SerializeField] private LineRenderer laser;
    [SerializeField] private Transform puntoDisparo;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocidadOriginal = velocidad;

        if (laser != null)
        {
            laser.positionCount = 2;
            laser.enabled = true;
        }
    }

    private void Update()
    {
        MoverJugador();
        ComprobarSuelo();
        GestionarSalto();
        GestionarInteraccion();
        GestionarAturdimiento();
        ActualizarLaser();
    }

    private void MoverJugador()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direccion = new Vector3(horizontal, 0f, vertical);
        direccion = Vector3.ClampMagnitude(direccion, 1f);

        if (Input.GetKeyDown(KeyCode.LeftShift) && puedeUsarDash)
        {
            direccionDash = direccion.sqrMagnitude > 0.01f
                ? direccion.normalized
                : transform.forward;

            StartCoroutine(RealizarDash());
        }

        Vector3 movimiento = haciendoDash
            ? direccionDash * velocidadDash
            : direccion * velocidad;

        transform.Translate(
            movimiento * Time.deltaTime,
            Space.World
        );
    }

    private void ComprobarSuelo()
    {
        if (puntoChequeoSuelo == null)
            return;

        enSuelo = Physics.Raycast(
            puntoChequeoSuelo.position,
            Vector3.down,
            distanciaSuelo,
            capaSuelo
        );

        if (rb != null && rb.linearVelocity.y < 0)
        {
            rb.AddForce(
                Physics.gravity * 0.5f,
                ForceMode.Acceleration
            );
        }
    }

    private void GestionarSalto()
    {
        if (Input.GetButtonDown("Jump") && enSuelo && rb != null)
        {
            rb.AddForce(
                Vector3.up * fuerzaSalto,
                ForceMode.Impulse
            );
        }
    }

    private void GestionarInteraccion()
    {
        if (puntoInteraccion == null)
            return;

        if (Physics.Raycast(
            puntoInteraccion.position,
            puntoInteraccion.forward,
            out RaycastHit hit,
            distanciaInteraccion,
            capaInteractiva))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Objeto interactivo detectado: " + hit.collider.name);
            }
        }
    }

    private void GestionarAturdimiento()
    {
        if (!Input.GetKeyDown(KeyCode.F) || puntoInteraccion == null)
            return;

        if (Physics.Raycast(
            puntoInteraccion.position,
            puntoInteraccion.forward,
            out RaycastHit hit,
            distanciaAturdimiento))
        {
            CentinelaIA centinela =
                hit.collider.GetComponentInParent<CentinelaIA>();

            if (centinela != null)
            {
                centinela.Aturdir();
                Debug.Log("¡Centinela aturdido!");
            }
            else
            {
                Debug.Log("El objeto alcanzado no es un centinela.");
            }
        }
        else
        {
            Debug.Log("No hay ningún collider delante del jugador.");
        }
    }

    private void ActualizarLaser()
    {
        if (laser == null || puntoDisparo == null)
            return;

        Vector3 origen = puntoDisparo.position;
        Vector3 direccion = puntoDisparo.forward;

        Vector3 destino = origen + direccion * distanciaAturdimiento;

        if (Physics.Raycast(
            origen,
            direccion,
            out RaycastHit hit,
            distanciaAturdimiento))
        {
            destino = hit.point;
        }

        laser.SetPosition(0, origen);
        laser.SetPosition(1, destino);
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
            textoCooldownDash.text = "¡DASH!";

        yield return new WaitForSeconds(duracionDash);

        haciendoDash = false;

        float tiempoRestante = cooldownDash;

        while (tiempoRestante > 0)
        {
            if (textoCooldownDash != null)
            {
                textoCooldownDash.text =
                    "Recarga: " + tiempoRestante.ToString("F1") + " s";
            }

            yield return new WaitForSeconds(0.1f);
            tiempoRestante -= 0.1f;
        }

        puedeUsarDash = true;

        if (textoCooldownDash != null)
            textoCooldownDash.text = "Dash listo";
    }

    private void OnDrawGizmos()
    {
        if (puntoChequeoSuelo != null)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(
                puntoChequeoSuelo.position,
                puntoChequeoSuelo.position + Vector3.down * distanciaSuelo
            );
        }

        if (puntoInteraccion != null)
        {
            Gizmos.color = Color.cyan;

            Gizmos.DrawRay(
                puntoInteraccion.position,
                puntoInteraccion.forward * distanciaInteraccion
            );

            Gizmos.color = Color.magenta;

            Gizmos.DrawRay(
                puntoInteraccion.position,
                puntoInteraccion.forward * distanciaAturdimiento
            );
        }
    }
}
