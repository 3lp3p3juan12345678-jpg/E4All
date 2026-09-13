using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucesPuerta : MonoBehaviour
{
    // Un grupo de luces por cada sala/puerta (Facil, Intermedio, Dificil, etc.)
    [System.Serializable]
    public class GrupoLucesPuerta
    {
        [Tooltip("Debe coincidir EXACTAMENTE con el Trophy Id de esa sala en GoalTrophy (ej: Facil, Intermedio, Dificil)")]
        public string salaId;
        public List<Light> luces;
    }

    [Header("Grupos de luces, uno por sala")]
    public List<GrupoLucesPuerta> gruposDeLuces;

    [Header("Colores")]
    public Color colorRojoAlInicio = Color.red;
    public Color colorVerdeAlCompletar = Color.green;

    [Header("Efecto en cadena")]
    public float tiempoEntreLuces = 0.2f;

    void Start()
    {
        // Al iniciar, todas las luces de todas las salas quedan en rojo
        foreach (GrupoLucesPuerta grupo in gruposDeLuces)
        {
            foreach (Light luz in grupo.luces)
            {
                if (luz != null)
                {
                    luz.color = colorRojoAlInicio;
                    luz.enabled = true;
                }
            }
        }
    }

    // Llamado por GoalTrophy cuando el jugador consigue el trofeo de una sala específica (con animación en cadena)
    public void ActivarLucesVerdesPuerta(string salaId)
    {
        GrupoLucesPuerta grupo = gruposDeLuces.Find(g => g.salaId == salaId);
        if (grupo != null)
        {
            StartCoroutine(CambiarVerdeEnCadena(grupo.luces));
        }
        else
        {
            Debug.LogWarning($"[LucesPuerta] No se encontró ningún grupo con salaId = \"{salaId}\"");
        }
    }

    IEnumerator CambiarVerdeEnCadena(List<Light> luces)
    {
        foreach (Light luz in luces)
        {
            if (luz != null)
            {
                luz.color = colorVerdeAlCompletar;
                luz.enabled = true;
            }

            yield return new WaitForSeconds(tiempoEntreLuces);
        }
    }

    // Usado por el sistema de guardado: pone en verde SOLO el grupo de esa sala, de una vez, sin animación
    public void PonerVerdeInmediato(string salaId)
    {
        GrupoLucesPuerta grupo = gruposDeLuces.Find(g => g.salaId == salaId);
        if (grupo != null)
        {
            foreach (Light luz in grupo.luces)
            {
                if (luz != null)
                {
                    luz.color = colorVerdeAlCompletar;
                    luz.enabled = true;
                }
            }
        }
    }
}