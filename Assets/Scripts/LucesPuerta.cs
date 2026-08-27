using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucesPuerta : MonoBehaviour
{
    // Arrastra aquí únicamente las luces que están arriba de la puerta
    public List<Light> lucesDePuerta; 

    // Colores específicos para la puerta
    public Color colorRojoAlInicio = Color.red;
    public Color colorVerdeAlCompletar = Color.green;

    // Tiempo entre cada luz de la puerta (si quieres efecto en cadena también aquí)
    public float tiempoEntreLuces = 0.2f;

    void Start()
    {
        // Al iniciar el juego, configuramos las luces de la puerta en rojo y encendidas (o como prefieras)
        foreach (Light luz in lucesDePuerta)
        {
            if (luz != null)
            {
                luz.color = colorRojoAlInicio;
                luz.enabled = true; // O ponlo en false si quieres que inicien apagadas
            }
        }
    }

    // Esta función la llamas desde tu portal cuando el jugador regresa
    public void ActivarLucesVerdesPuerta()
    {
        StartCoroutine(CambiarVerdeEnCadena());
    }

    IEnumerator CambiarVerdeEnCadena()
    {
        foreach (Light luz in lucesDePuerta)
        {
            if (luz != null)
            {
                luz.color = colorVerdeAlCompletar; // Cambia a verde
                luz.enabled = true;
            }

            yield return new WaitForSeconds(tiempoEntreLuces);
        }
    }
}