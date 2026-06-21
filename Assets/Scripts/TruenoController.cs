using System.Collections;
using UnityEngine;

public class TruenoController : MonoBehaviour
{
    public Light luzFlash;            // luz tipo Point/Directional para el flash
    public AudioSource sonidoTrueno;
    public float tiempoMin = 4f;
    public float tiempoMax = 12f;

    private Coroutine rutina;

    public void IniciarTormenta()
    {
        rutina = StartCoroutine(CicloTruenos());
    }

    public void DetenerTormenta()
    {
        if (rutina != null) StopCoroutine(rutina);
        luzFlash.enabled = false;
    }

    IEnumerator CicloTruenos()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(tiempoMin, tiempoMax));
            yield return StartCoroutine(Flash());
        }
    }

    IEnumerator Flash()
    {
        luzFlash.enabled = true;
        luzFlash.intensity = Random.Range(3f, 6f);
        sonidoTrueno.pitch = Random.Range(0.9f, 1.1f);
        sonidoTrueno.Play();
        yield return new WaitForSeconds(0.1f);
        luzFlash.enabled = false;

        // Opcional: doble flash para más realismo
        yield return new WaitForSeconds(0.15f);
        luzFlash.enabled = true;
        yield return new WaitForSeconds(0.05f);
        luzFlash.enabled = false;
    }
}