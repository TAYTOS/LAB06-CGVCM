using UnityEngine;

public class InterruptorDiaNoche : MonoBehaviour
{
    [Header("Detección del jugador")]
    public Transform camaraJugador;
    public float distanciaInteraccion = 2.5f;

    [Header("Iluminación")]
    public Light luzSol;              // Directional Light
    public Color colorDia = Color.white;
    public Color colorNoche = new Color(0.1f, 0.1f, 0.2f);
    public float intensidadDia = 1f;
    public float intensidadNoche = 0.05f;
    public Light luzNocturna;         // Lámpara/veladora, apagada de día

    [Header("Lluvia y truenos")]
    public ParticleSystem lluvia;
    public AudioSource sonidoLluvia;
    public TruenoController truenos;  // script aparte para el rayo/flash + sonido

    private bool esDeNoche = false;

    void Update()
    {
        if (EstaJugadorCerca() && Input.GetKeyDown(KeyCode.E))
        {
            CambiarEstado();
        }
    }

    bool EstaJugadorCerca()
    {
        return Vector3.Distance(camaraJugador.position, transform.position) <= distanciaInteraccion;
    }

    void CambiarEstado()
    {
        esDeNoche = !esDeNoche;

        luzSol.color = esDeNoche ? colorNoche : colorDia;
        luzSol.intensity = esDeNoche ? intensidadNoche : intensidadDia;
        luzNocturna.enabled = esDeNoche;

        if (esDeNoche)
        {
            lluvia.Play();
            sonidoLluvia.Play();
            truenos.IniciarTormenta();
        }
        else
        {
            lluvia.Stop();
            sonidoLluvia.Stop();
            truenos.DetenerTormenta();
        }
    }
}