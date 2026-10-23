
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [Header("Salud")]
    [SerializeField] private int saludMaxima = 100;
    [SerializeField] private Slider barraSalud;
    private int saludActual;

    [Header("Puntuacion")]
    [SerializeField] private int puntuacion = 0;
    [SerializeField] private TextMeshProUGUI textoPuntos;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;
    void Start()
    {
        saludActual = saludMaxima;

        barraSalud.maxValue = saludMaxima;
        barraSalud.value = saludActual;

        ActualizarHUD();

        Debug.Log("Salud inicial: " + saludActual);
        Debug.Log("Puntuacion inicial: " + puntuacion);
    }

    public void RecibirDano(int cantidad)
    {
        // No recibir más daño después de perder
        if (saludActual <= 0)
            return;

        saludActual -= cantidad;

        if (saludActual < 0)
        {
            saludActual = 0;
        }

        ActualizarBarraSalud();

        if (saludActual <= 0 && gameManager != null)
        {
            gameManager.DeclararDerrota();
        }

        Debug.Log("Salud actual: " + saludActual);
    }


    public void SumarPuntos(int cantidad)
    {
        // Evitar seguir sumando después de ganar
        if (puntuacion >= 100)
            return;

        puntuacion += cantidad;

        if (puntuacion >= 100)
        {
            puntuacion = 100;

            if (gameManager != null)
            {
                gameManager.DeclararVictoria();
            }
        }

        ActualizarHUD();

        Debug.Log("Puntuacion actual: " + puntuacion);
    }

    private void ActualizarHUD()
    {
        if (textoPuntos != null)
        {
            textoPuntos.text = "Puntos: " + puntuacion + "/100";
        }
    }
    private void ActualizarBarraSalud()
    {
        if (barraSalud != null)
        {
            barraSalud.value = saludActual;
        }
    }
}
