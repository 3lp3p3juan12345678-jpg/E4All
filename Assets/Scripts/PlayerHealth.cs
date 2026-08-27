using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración del Núcleo")]
    public int maxEnergyCores = 3;
    private int currentCores;

    [Header("Interfaz Visual de Núcleos")]
    public Image[] energyCoresUI; // Arrastra aquí tus Core_1, Core_2, Core_3

    private PlayerRespawn respawnScript;

    void Start()
    {
        currentCores = maxEnergyCores;
        respawnScript = GetComponent<PlayerRespawn>();
        UpdateCoresUI();
    }

    // Método que se llama cuando el jugador recibe daño (DeadZone, trampas, caída al vacío)
    public void TakeDamage()
    {
        currentCores--;
        UpdateCoresUI();
        
        Debug.Log("Núcleos de energía restantes: " + currentCores);

        if (currentCores > 0)
        {
            // Aún tiene núcleos: reaparece en el último checkpoint activo
            if (respawnScript != null)
            {
                respawnScript.RespawnToCurrent();
            }
        }
        else
        {
            // ¡Se quedó sin núcleos! Lo mandamos al spawn principal y restauramos la energía
            Debug.Log("¡Sin núcleos de energía! Regresando al SpawnPoint principal.");
            if (respawnScript != null)
            {
                respawnScript.RespawnToInitial();
            }
            ResetHealth(); // Recupera sus 3 núcleos automáticamente
        }
    }

    // Método para actualizar visualmente qué núcleos se ven en la interfaz
    void UpdateCoresUI()
    {
        for (int i = 0; i < energyCoresUI.Length; i++)
        {
            if (i < currentCores)
            {
                energyCoresUI[i].gameObject.SetActive(true); // Núcleo encendido
            }
            else
            {
                energyCoresUI[i].gameObject.SetActive(false); // Núcleo apagado (oculto)
            }
        }
    }

    // Método por si más adelante agregas un coleccionable que devuelva energía
    public void AddEnergyCore(int amount)
    {
        currentCores += amount;
        if (currentCores > maxEnergyCores) 
        {
            currentCores = maxEnergyCores;
        }
        UpdateCoresUI();
    }

    // Método para rellenar la vida al máximo
    public void ResetHealth()
    {
        currentCores = maxEnergyCores;
        UpdateCoresUI();
    }
}