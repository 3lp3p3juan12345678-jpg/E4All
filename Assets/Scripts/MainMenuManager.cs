using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Configuración de Escenas")]
    [Tooltip("Nombre exacto de la escena del juego registrada en Build Settings")]
    public string nombreEscenaJuego = "Nivel1";

    private void Awake()
    {
        RestaurarCursor();
    }

    private void Start()
    {
        RestaurarCursor();
    }

    private void Update()
    {
        // Fuerza el cursor visible si algún otro script en la escena intenta ocultarlo
        if (!Cursor.visible || Cursor.lockState != CursorLockMode.None)
        {
            RestaurarCursor();
        }

        // Atajo por si el clic del ratón falla: presionar Enter o Espacio para jugar
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            BotonJugar();
        }

        // Presionar Escape para salir
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BotonSalir();
        }
    }

    public void RestaurarCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void BotonJugar()
    {
        Debug.Log("Intentando cargar la escena: " + nombreEscenaJuego);

        // Revisa si la escena está en File -> Build Settings
        if (Application.CanStreamedLevelBeLoaded(nombreEscenaJuego))
        {
            SceneManager.LoadScene(nombreEscenaJuego);
        }
        else
        {
            Debug.LogError($"[ERROR] La escena '{nombreEscenaJuego}' no está agregada en File -> Build Settings o tiene un error de nombre.");
            
            // Intento de respaldo: carga la escena #1 en la lista si la búsqueda por nombre falla
            if (SceneManager.sceneCountInBuildSettings > 1)
            {
                Debug.LogWarning("Cargando escena según el índice 1 como respaldo...");
                SceneManager.LoadScene(1);
            }
        }
    }

    public void BotonSalir()
    {
        Debug.Log("Cerrando aplicación...");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}