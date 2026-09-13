using UnityEngine;

public class GoalTrophy : MonoBehaviour
{
    [Header("Identificador único de este trofeo")]
    [Tooltip("Ponle un nombre único por sala, ej: Facil, Intermedio, Dificil. Debe coincidir con el salaId correspondiente en LucesPuerta.")]
    public string trophyId;

    [Header("Configuración de Trofeos")]
    public GameObject trophyInLevel;      // Arrastra aquí el trofeo que está en la meta
    public GameObject trophyOnShelf;      // Arrastra aquí el trofeo "fantasma" que está en el estante

    [Header("Luces de la puerta (script compartido entre salas)")]
    public LucesPuerta lucesDePuerta;     // Arrastra aquí el ÚNICO ControladorLucesPuerta de la escena

    private bool yaConseguido = false;

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
        if (other.CompareTag("Player") && !yaConseguido)
        {
            CollectTrophy();
        }
    }

    private void CollectTrophy()
    {
        yaConseguido = true;
        Debug.Log("¡Trofeo recogido! Meta alcanzada: " + GetTrophyId());

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

        // 3. Activamos SOLO las luces verdes de la puerta de ESTA sala (con animación en cadena)
        if (lucesDePuerta != null)
        {
            lucesDePuerta.ActivarLucesVerdesPuerta(GetTrophyId());
        }
    }

    // ---- Para el sistema de guardado ----

    // Devuelve el identificador único (o el nombre del objeto si no se puso uno)
    public string GetTrophyId()
    {
        return string.IsNullOrEmpty(trophyId) ? gameObject.name : trophyId;
    }

    // Devuelve si este trofeo ya fue conseguido (para poder guardarlo)
    public bool FueConseguido()
    {
        return yaConseguido;
    }

    // Restaura el estado de "ya conseguido" al cargar una partida, sin animaciones
    public void RestaurarComoConseguido()
    {
        yaConseguido = true;

        if (trophyInLevel != null)
        {
            trophyInLevel.SetActive(false);
        }

        if (trophyOnShelf != null)
        {
            trophyOnShelf.SetActive(true);
        }

        if (lucesDePuerta != null)
        {
            lucesDePuerta.PonerVerdeInmediato(GetTrophyId());
        }
    }
}