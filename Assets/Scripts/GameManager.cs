
using UnityEngine;
using System;

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
    // Eventos del juego
    public event Action<EstadoJuego> OnEstadoCambiado;
    public event Action OnVictoria;
    public event Action OnDerrota;
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
        // Evitar notificar si el estado no cambió
        if (estadoActual == nuevoEstado)
            return;

        estadoActual = nuevoEstado;

        // Controlar el tiempo según el estado
        switch (estadoActual)
        {
            case EstadoJuego.Inicio:
                Time.timeScale = 0f;
                Debug.Log("Esperando que comience la partida.");
                break;

            case EstadoJuego.Jugando:
                Time.timeScale = 1f;
                Debug.Log("¡La partida comenzó!");
                break;

            case EstadoJuego.Pausa:
                Time.timeScale = 0f;
                Debug.Log("Partida pausada.");
                break;

            case EstadoJuego.Victoria:
                Time.timeScale = 0f;
                Debug.Log("¡Victoria!");
                break;

            case EstadoJuego.Derrota:
                Time.timeScale = 0f;
                Debug.Log("Derrota.");
                break;
        }

        Debug.Log("Estado del juego: " + estadoActual);

        // Notificar a los suscriptores
        OnEstadoCambiado?.Invoke(estadoActual);
    }
    public void DeclararVictoria()
    {
        if (estadoActual == EstadoJuego.Victoria ||
            estadoActual == EstadoJuego.Derrota)
            return;

        CambiarEstado(EstadoJuego.Victoria);
        OnVictoria?.Invoke();
    }

    public void DeclararDerrota()
    {
        if (estadoActual == EstadoJuego.Victoria ||
            estadoActual == EstadoJuego.Derrota)
            return;

        CambiarEstado(EstadoJuego.Derrota);
        OnDerrota?.Invoke();
    }
}
