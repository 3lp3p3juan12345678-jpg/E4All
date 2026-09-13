using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManagerE4All : MonoBehaviour
{
    [Header("Configuración de Escenas y UI")]
    public GameObject menuPausaPanel;
    public string nombreEscenaMenu = "LoadingScene"; 

    private bool juegoPausado = false;

    private void Start()
    {
        if (menuPausaPanel != null)
        {
            menuPausaPanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    private void Update()
    {
        // Abrir/Cerrar pausa con la tecla Intro/Enter (o tecla P opcional)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.P))
        {
            if (juegoPausado)
            {
                ReanudarJuego();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    public void PausarJuego()
    {
        juegoPausado = true;
        if (menuPausaPanel != null) menuPausaPanel.SetActive(true);
        Time.timeScale = 0f; // Congela el juego
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        if (menuPausaPanel != null) menuPausaPanel.SetActive(false);
        Time.timeScale = 1f; // Reanuda el juego
    }

    public void GuardarYSalir()
    {
        // Guardar la posición actual del personaje si tiene el script asignado
        PlayerPositionSave posSaver = FindFirstObjectByType<PlayerPositionSave>();
        if (posSaver != null)
        {
            posSaver.GuardarPosicion();
        }

        PlayerPrefs.SetInt("TienePartida", 1);
        PlayerPrefs.Save(); 

        Time.timeScale = 1f; 
        SceneManager.LoadScene(nombreEscenaMenu);
    }

    public void SalirSinGuardar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}