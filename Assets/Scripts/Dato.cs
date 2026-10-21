
using UnityEngine;

public class Dato : MonoBehaviour
{
    [Header("Puntuación")]
    [SerializeField] private int puntosOtorgados = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerState jugador = other.GetComponent<PlayerState>();

            if (jugador != null)
            {
                jugador.SumarPuntos(puntosOtorgados);
                Destroy(gameObject);
            }
        }
    }
}
