
using UnityEngine;

public class ZonaDeteccion : MonoBehaviour
{
    private SphereCollider zona;
    private CentinelaIA centinela;
    private PowerUpInvisibilidad invisibilidadJugador;

    private bool jugadorDentro = false;
    private bool ultimaInvisibilidad = false;

    private void Awake()
    {
        zona = GetComponent<SphereCollider>();
        centinela = GetComponentInParent<CentinelaIA>();
    }

    private void Update()
    {
        if (!jugadorDentro || invisibilidadJugador == null)
            return;

        // Comprobar si cambio el estado de invisibilidad
        if (invisibilidadJugador.EstaInvisible != ultimaInvisibilidad)
        {
            ultimaInvisibilidad = invisibilidadJugador.EstaInvisible;
            ActualizarDeteccion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        invisibilidadJugador =
            other.GetComponentInParent<PowerUpInvisibilidad>();

        if (invisibilidadJugador == null)
            return;

        jugadorDentro = true;
        ultimaInvisibilidad = invisibilidadJugador.EstaInvisible;

        ActualizarDeteccion();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PowerUpInvisibilidad invisibilidad =
            other.GetComponentInParent<PowerUpInvisibilidad>();

        if (invisibilidad == null || invisibilidad != invisibilidadJugador)
            return;

        jugadorDentro = false;

        // Si el jugador sale siendo visible, el centinela lo pierde.
        if (!invisibilidadJugador.EstaInvisible)
        {
            centinela.PerderJugador();
        }

        invisibilidadJugador = null;
    }

    private void ActualizarDeteccion()
    {
        if (centinela == null || invisibilidadJugador == null)
            return;

        if (invisibilidadJugador.EstaInvisible)
        {
            centinela.PerderJugador();
            Debug.Log("El centinela no puede detectar al jugador invisible.");
        }
        else
        {
            centinela.DetectarJugador();
        }
    }

    private void OnDrawGizmos()
    {
        SphereCollider esfera = GetComponent<SphereCollider>();

        if (esfera == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.TransformPoint(esfera.center),
            esfera.radius * transform.lossyScale.x
        );
    }
}
