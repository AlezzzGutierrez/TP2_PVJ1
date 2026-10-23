
using UnityEngine;
using System.Collections;

public class PowerUpInvisibilidad : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float duracionInvisibilidad = 5f;

    private Renderer[] renderizadores;

    public bool EstaInvisible { get; private set; } = false;

    private void Awake()
    {
        renderizadores = GetComponentsInChildren<Renderer>(true);
    }

    public void ActivarInvisibilidad()
    {
        StopAllCoroutines();
        StartCoroutine(InvisibilidadTemporal());
    }

    private IEnumerator InvisibilidadTemporal()
    {
        EstaInvisible = true;
        CambiarVisibilidad(false);

        Debug.Log("¡Jugador invisible!");

        yield return new WaitForSeconds(duracionInvisibilidad);

        EstaInvisible = false;
        CambiarVisibilidad(true);

        Debug.Log("La invisibilidad terminó.");
    }

    private void CambiarVisibilidad(bool visible)
    {
        foreach (Renderer renderizador in renderizadores)
        {
            renderizador.enabled = visible;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PowerUpInvisibilidad"))
            return;

        ActivarInvisibilidad();

        Debug.Log("Power-up de invisibilidad recogido.");

        Destroy(other.gameObject);
    }
}
