using System.Collections;
using UnityEngine;

public class InterruptorTormenta : MonoBehaviour
{
    private bool jugadorCerca = false;
    private bool lucesEncendidas = true; // empieza encendida

    [Header("Luces del cuarto")]
    public Light[] lucesHabitacion; // arrastra aquí todas tus lámparas/Point Lights

    [Header("Lluvia")]
    public GameObject lluviaParticulas;
    public AudioSource sonidoLluvia;

    [Header("Truenos")]
    public AudioSource sonidoTrueno;
    public AudioClip[] cliposTrueno;
    public Light luzFlash;
    private Coroutine rutinaTruenos;

    [Header("Puerta - Golpes")]
    public Transform puerta;              // arrastra el Transform de la puerta (el que rota/bisagra)
    public AudioSource sonidoGolpes;      // el AudioSource de golpes que creaste
    public AudioClip[] cliposGolpes;      // uno o varios clips de golpes
    private Coroutine rutinaGolpes;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = false;
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            CambiarEstado();
        }
    }

    void CambiarEstado()
    {
        lucesEncendidas = !lucesEncendidas;

        foreach (Light luz in lucesHabitacion)
        {
            luz.enabled = lucesEncendidas;
        }

        RenderSettings.ambientIntensity = lucesEncendidas ? 1f : 0.05f;

        if (!lucesEncendidas)
        {
            // se apagó la luz -> empieza la tormenta
            lluviaParticulas.SetActive(true);
            sonidoLluvia.Play();
            rutinaTruenos = StartCoroutine(CicloTruenos());
            rutinaGolpes = StartCoroutine(CicloGolpesPuerta());
        }
        else
        {
            // se volvió a encender -> para la tormenta
            lluviaParticulas.SetActive(false);
            sonidoLluvia.Stop();
            if (rutinaTruenos != null) StopCoroutine(rutinaTruenos);
            if (rutinaGolpes != null) StopCoroutine(rutinaGolpes);
            if (luzFlash != null) luzFlash.enabled = false;
        }
    }

    IEnumerator CicloTruenos()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4f, 12f));

            if (luzFlash != null)
            {
                luzFlash.enabled = true;
                luzFlash.intensity = Random.Range(3f, 6f);
            }
            if (cliposTrueno.Length > 0)
            {
                sonidoTrueno.clip = cliposTrueno[Random.Range(0, cliposTrueno.Length)];
                sonidoTrueno.pitch = Random.Range(0.9f, 1.1f);
                sonidoTrueno.Play();
            }

            yield return new WaitForSeconds(0.15f);
            if (luzFlash != null) luzFlash.enabled = false;
        }
    }

    IEnumerator CicloGolpesPuerta()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(6f, 15f));

            if (cliposGolpes.Length > 0)
            {
                sonidoGolpes.clip = cliposGolpes[Random.Range(0, cliposGolpes.Length)];
                sonidoGolpes.pitch = Random.Range(0.95f, 1.05f);
                sonidoGolpes.Play();
            }

            if (puerta != null)
            {
                yield return StartCoroutine(SacudirPuerta());
            }
        }
    }

    IEnumerator SacudirPuerta()
    {
        Vector3 rotacionOriginal = puerta.localEulerAngles;
        int golpes = Random.Range(3, 6); // cantidad de golpes en esta tanda

        for (int i = 0; i < golpes; i++)
        {
            puerta.localEulerAngles = rotacionOriginal + new Vector3(0, Random.Range(2f, 5f), 0);
            yield return new WaitForSeconds(0.08f);
            puerta.localEulerAngles = rotacionOriginal;
            yield return new WaitForSeconds(0.12f);
        }
    }
}