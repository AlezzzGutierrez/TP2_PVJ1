
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
        saludActual -= cantidad;

        if (saludActual < 0)
        {
            saludActual = 0;
        }

        ActualizarBarraSalud();

        if (saludActual <= 0)
        {
            saludActual = 0;

            Debug.Log("¡DERROTA! El jugador se quedó sin vida.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dato"))
        {
            SumarPuntos(10);
            Destroy(other.gameObject);
        }
    }

    public void SumarPuntos(int cantidad)
    {
        puntuacion += cantidad;

        if (puntuacion >= 100)
        {
            puntuacion = 100;
            Debug.Log("¡VICTORIA! ¡Conseguiste 100 puntos!");
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
