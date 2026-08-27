using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuenciaLuces : MonoBehaviour
{
    // Creamos una estructura para agrupar la luz y su sonido juntos
    [System.Serializable]
    public struct ElementoDeLuz
    {
        public Light luzDelTecho;
        public AudioSource audioLuz;
    }

    public List<ElementoDeLuz> secuenciaDeLuces; 
    
    // Tiempo que esperas ANTES de empezar la secuencia
    public float tiempoDeEsperaInicial = 3.0f; 
    
    // Tiempo entre cada luz individual
    public float tiempoEntreLuces = 0.5f; 

    void Start()
    {
        // Apagamos todas las luces al iniciar y detenemos sus audios
        foreach (ElementoDeLuz elemento in secuenciaDeLuces)
        {
            if (elemento.luzDelTecho != null)
                elemento.luzDelTecho.enabled = false;

            if (elemento.audioLuz != null)
                elemento.audioLuz.Stop();
        }

        // Iniciamos la secuencia
        StartCoroutine(EncenderYReproducirEnSecuencia());
    }

    IEnumerator EncenderYReproducirEnSecuencia()
    {
        // Espera inicial antes de empezar
        yield return new WaitForSeconds(tiempoDeEsperaInicial);

        foreach (ElementoDeLuz elemento in secuenciaDeLuces)
        {
            // Encendemos la luz
            if (elemento.luzDelTecho != null)
            {
                elemento.luzDelTecho.enabled = true; 
            }

            // Reproducimos su sonido correspondiente
            if (elemento.audioLuz != null)
            {
                elemento.audioLuz.Play();
            }

            // Espera antes de pasar a la siguiente luz
            yield return new WaitForSeconds(tiempoEntreLuces); 
        }
    }
}