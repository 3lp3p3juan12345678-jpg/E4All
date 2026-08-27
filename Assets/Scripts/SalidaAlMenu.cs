using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaAlMenu : MonoBehaviour
{
    [Header("Configuración de la Escena")]
    [Tooltip("Nombre exacto de tu escena de menú o carga registrada en Build Settings")]
    public string nombreEscenaDestino = "LoadingScene";

    [Header("Configuración del Jugador")]
    [Tooltip("Tag que debe tener el objeto del jugador para activar la salida")]
    public string tagJugador = "Player";

    // Se ejecuta cuando el objeto es de tipo "Trigger" (Zona atravesable)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            Debug.Log("¡Jugador entró a la zona de salida!");
            RegresarAlMenu();
        }
    }

    // Se ejecuta si el objeto es un sólido físico (sin 'Is Trigger')
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagJugador))
        {
            Debug.Log("¡Jugador chocó con el objeto de salida!");
            RegresarAlMenu();
        }
    }

    private void RegresarAlMenu()
    {
        // 1. Muestra y desbloquea el cursor por si venía bloqueado del gameplay
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 2. Carga la escena del Menú o Carga
        SceneManager.LoadScene(nombreEscenaDestino);
    }
}