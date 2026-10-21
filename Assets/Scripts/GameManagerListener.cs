
using UnityEngine;
using TMPro;

public class GameManagerListener : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI textoResultado;

    private void OnEnable()
    {
        if (gameManager == null)
            return;

        gameManager.OnVictoria += MostrarVictoria;
        gameManager.OnDerrota += MostrarDerrota;
    }

    private void OnDisable()
    {
        if (gameManager == null)
            return;

        gameManager.OnVictoria -= MostrarVictoria;
        gameManager.OnDerrota -= MostrarDerrota;
    }

    private void MostrarVictoria()
    {
        MostrarResultado("¡VICTORIA!");
    }

    private void MostrarDerrota()
    {
        MostrarResultado("DERROTA");
    }

    private void MostrarResultado(string mensaje)
    {
        if (textoResultado == null)
            return;

        textoResultado.text = mensaje;
        textoResultado.gameObject.SetActive(true);
    }
}
