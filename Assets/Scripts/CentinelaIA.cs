
using UnityEngine;
using UnityEngine.AI;

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
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        destinoActual = puntoA;
        agente.SetDestination(destinoActual.position);
    }

    void Update()
    {
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
}
