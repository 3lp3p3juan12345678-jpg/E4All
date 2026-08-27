using UnityEngine;

public class GoalTrophy : MonoBehaviour
{
    [Header("Configuración de Trofeos")]
    public GameObject trophyInLevel;      // Arrastra aquí el trofeo que está en la meta
    public GameObject trophyOnShelf;      // Arrastra aquí el trofeo "fantasma" que está en el estante

    private void Start()
    {
        // Al iniciar, aseguramos que el del estante esté invisible/apagado
        if (trophyOnShelf != null)
        {
            trophyOnShelf.SetActive(false);
        }
        
        // Aseguramos que el del nivel esté visible/encendido
        if (trophyInLevel != null)
        {
            trophyInLevel.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador toca el trofeo del nivel
        if (other.CompareTag("Player"))
        {
            CollectTrophy();
        }
    }

    private void CollectTrophy()
    {
        Debug.Log("¡Trofeo recogido! Meta alcanzada.");

        // 1. Desactivamos el trofeo físico en el nivel
        if (trophyInLevel != null)
        {
            trophyInLevel.SetActive(false);
        }

        // 2. Activamos el trofeo en el estante
        if (trophyOnShelf != null)
        {
            trophyOnShelf.SetActive(true);
        }
        
        // Opcional: Puedes agregar un sonido de victoria aquí
        // O llamar a tu script de UI para mostrar un mensaje de "Nivel Completado"
    }
}