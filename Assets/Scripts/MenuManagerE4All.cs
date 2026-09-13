using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManagerE4All : MonoBehaviour
{
    [Header("Configuración de Escena")]
    public string escenaJuego = "SampleScene"; 

    [Header("UI - Botones Principales")]
    public Button botonJugar;           
    public Button botonNuevaPartida;
    public Button botonSalir;

    [Header("UI - Panel de Confirmación")]
    public GameObject panelConfirmacion; 
    public Button botonSi;
    public Button botonNo;

    private void Start()
    {
        if (panelConfirmacion != null)
        {
            panelConfirmacion.SetActive(false);
        }

        VerificarPartidaGuardada();
    }

    public void VerificarPartidaGuardada()
    {
        bool tieneGuardado = PlayerPrefs.GetInt("TienePartida", 0) == 1;

        if (botonJugar != null)
        {
            botonJugar.interactable = tieneGuardado;
        }
    }

    public void ClickJugar()
    {
        SceneManager.LoadScene(escenaJuego);
    }

    public void ClickNuevaPartida()
    {
        if (PlayerPrefs.GetInt("TienePartida", 0) == 1)
        {
            if (panelConfirmacion != null)
            {
                panelConfirmacion.SetActive(true);
            }
        }
        else
        {
            CrearNuevaPartida();
        }
    }

    public void ConfirmarNuevaPartida()
    {
        PlayerPrefs.DeleteAll();
        CrearNuevaPartida();
    }

    public void CancelarNuevaPartida()
    {
        if (panelConfirmacion != null)
        {
            panelConfirmacion.SetActive(false);
        }
    }

    private void CrearNuevaPartida()
    {
        PlayerPrefs.SetInt("TienePartida", 1);
        PlayerPrefs.SetInt("TrofeosGuardados", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(escenaJuego);
    }

    public void ClickSalir()
    {
        Application.Quit();
    }
}