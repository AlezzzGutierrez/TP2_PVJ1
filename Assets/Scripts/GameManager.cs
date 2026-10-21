
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum EstadoJuego
    {
        Inicio,
        Jugando,
        Pausa,
        Victoria,
        Derrota
    }

    [Header("Estado del juego")]
    [SerializeField] private EstadoJuego estadoActual = EstadoJuego.Inicio;

    void Start()
    {
        Time.timeScale = 0f;
        CambiarEstado(EstadoJuego.Inicio);
    }

    void Update()
    {
        // Iniciar con Enter
        if (estadoActual == EstadoJuego.Inicio &&
            Input.GetKeyDown(KeyCode.Return))
        {
            CambiarEstado(EstadoJuego.Jugando);
        }

        // Pausar o reanudar con Escape
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (estadoActual == EstadoJuego.Jugando)
            {
                CambiarEstado(EstadoJuego.Pausa);
            }
            else if (estadoActual == EstadoJuego.Pausa)
            {
                CambiarEstado(EstadoJuego.Jugando);
            }
        }
    }

    public void CambiarEstado(EstadoJuego nuevoEstado)
    {
        estadoActual = nuevoEstado;

        // Congelar el tiempo en pausa, inicio y estados finales
        if (estadoActual == EstadoJuego.Jugando)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }

        Debug.Log("Estado del juego: " + estadoActual);
    }

    public void NotificarVidaYPuntaje(int vida, int puntaje)
    {
        Debug.Log(
            "Vida actual: " + vida +
            " | Puntaje: " + puntaje
        );
    }

    public void DeclararVictoria()
    {
        CambiarEstado(EstadoJuego.Victoria);
    }

    public void DeclararDerrota()
    {
        CambiarEstado(EstadoJuego.Derrota);
    }
}
