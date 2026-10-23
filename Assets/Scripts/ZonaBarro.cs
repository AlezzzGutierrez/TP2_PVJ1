using UnityEngine;

public class ZonaBarro : MonoBehaviour
{
    [Header("Configuracion del barro")]
    [SerializeField] private float velocidadReducida = 2f;

    private PlayerMovements jugadorDentro;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovements jugador = other.GetComponentInParent<PlayerMovements>();

        if (jugador != null)
        {
            jugadorDentro = jugador;
            jugadorDentro.CambiarVelocidad(velocidadReducida);

            Debug.Log("El jugador entro en la zona de barro.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovements jugador = other.GetComponentInParent<PlayerMovements>();

        if (jugador != null && jugador == jugadorDentro)
        {
            jugadorDentro.RestaurarVelocidad();
            jugadorDentro = null;

            Debug.Log("El jugador salio de la zona de barro.");
        }
    }
}