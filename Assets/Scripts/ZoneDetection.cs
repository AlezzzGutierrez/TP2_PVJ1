
using UnityEngine;


public class ZonaDeteccion : MonoBehaviour
{
    private SphereCollider zona;
    private CentinelaIA centinela;

    private void Awake()
    {
        zona = GetComponent<SphereCollider>();
        centinela = GetComponentInParent<CentinelaIA>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            centinela.DetectarJugador();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            centinela.PerderJugador();
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


    