
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CentinelaIA : MonoBehaviour
{
    [Header("Puntos de patrulla")]
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;

    [Header("Deteccion")]
    [SerializeField] private Transform jugador;

    private NavMeshAgent agente;
    private Transform destinoActual;
    private bool persiguiendo = false;
    [Header("Ataque")]
    [SerializeField] private int dano = 20;

    [Header("Aturdimiento")]
    [SerializeField] private bool aturdido = false;
    [SerializeField] private float duracionAturdimiento = 3f;

    [Header("Efecto visual")]
    [SerializeField] private Renderer renderizador;
    [SerializeField] private Color colorAturdido = Color.cyan;

    private Color colorOriginal;
    private Material materialEnemigo;
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        if (renderizador != null)
        {
            materialEnemigo = renderizador.material;
            colorOriginal = materialEnemigo.color;
        }
        destinoActual = puntoA;
        agente.SetDestination(destinoActual.position);
    }

    void Update()
    {
        if (aturdido)
            return;

        if (persiguiendo)
        {
            agente.SetDestination(jugador.position);
        }
        else
        {
            Patrullar();
        }
    }
    void Patrullar()
    {
        if (!agente.pathPending &&
            agente.remainingDistance <= agente.stoppingDistance)
        {
            destinoActual = destinoActual == puntoA
                ? puntoB
                : puntoA;

            agente.SetDestination(destinoActual.position);
        }
    }

    public void DetectarJugador()
    {
        persiguiendo = true;
        Debug.Log("¡Jugador detectado! El centinela persigue al jugador.");
    }

    public void PerderJugador()
    {
        persiguiendo = false;
        Debug.Log("Jugador fuera de la zona. Volviendo a patrullar.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerState estadoJugador =
                collision.gameObject.GetComponent<PlayerState>();

            if (estadoJugador != null)
            {
                estadoJugador.RecibirDano(dano);
            }
        }
    }

    public void Aturdir()
    {
        if (aturdido)
            return;

        StartCoroutine(Aturdimiento());
    }


    private IEnumerator Aturdimiento()
    {
        aturdido = true;

        // Detener al centinela y cancelar su ruta actual
        agente.isStopped = true;
        agente.ResetPath();
        if (materialEnemigo != null)
        {
            materialEnemigo.color = colorAturdido;
        }
        Debug.Log("¡Centinela aturdido!");

        // Esperar el tiempo de aturdimiento
        yield return new WaitForSeconds(duracionAturdimiento);

        // Comprobar que el agente siga disponible
        if (agente != null && agente.isActiveAndEnabled)
        {
            aturdido = false;
            agente.isStopped = false;

            // Reanudar la persecución o la patrulla
            if (persiguiendo && jugador != null)
            {
                agente.SetDestination(jugador.position);
            }
            else if (destinoActual != null)
            {
                agente.SetDestination(destinoActual.position);
            }
        }
        if (materialEnemigo != null)
        {
            materialEnemigo.color = colorOriginal;
        }
        Debug.Log("El centinela se recuperó del aturdimiento.");
    }


}
