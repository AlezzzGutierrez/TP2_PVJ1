
using UnityEngine;

public class JugadorEstado : MonoBehaviour
{
    [Header("Salud")]
    [SerializeField] private int saludMaxima = 100;
    private int saludActual;

    [Header("Puntuacion")]
    private int puntuacion = 0;

    void Start()
    {
        saludActual = saludMaxima;

        Debug.Log("Salud inicial: " + saludActual);
        Debug.Log("Puntuacion inicial: " + puntuacion);
    }

    public void RecibirDano(int cantidad)
    {
        saludActual -= cantidad;

        if (saludActual < 0)
        {
            saludActual = 0;
        }

        Debug.Log("Salud actual: " + saludActual);

        if (saludActual == 0)
        {
            Debug.Log("¡Game Over! El jugador se quedó sin salud.");
        }
    }

    public void SumarPuntos(int cantidad)
    {
        puntuacion += cantidad;

        Debug.Log("Puntuacion actual: " + puntuacion);
    }
}
