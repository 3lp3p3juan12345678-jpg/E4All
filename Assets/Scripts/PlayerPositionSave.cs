using UnityEngine;

public class PlayerPositionSave : MonoBehaviour
{
    private void Start()
    {
        CargarDatosJugador();
    }

    public void GuardarPosicion()
    {
        GuardarDatosJugador();
    }

    public void GuardarDatosJugador()
    {
        // 1. Guardar Posición
        Vector3 posActual = transform.position;
        PlayerPrefs.SetFloat("PosicionX", posActual.x);
        PlayerPrefs.SetFloat("PosicionY", posActual.y);
        PlayerPrefs.SetFloat("PosicionZ", posActual.z);
        PlayerPrefs.SetInt("PosicionGuardada", 1);

        // 2. Guardar Salud si existe el componente
        PlayerHealth healthScript = GetComponent<PlayerHealth>();
        if (healthScript != null)
        {
            PlayerPrefs.SetInt("VidaGuardada", healthScript.GetCurrentCores());
            PlayerPrefs.SetInt("VidaExiste", 1);
        }

        // 3. Guardar el checkpoint actual
        PlayerRespawn respawnScript = GetComponent<PlayerRespawn>();
        if (respawnScript != null)
        {
            string checkpointActual = respawnScript.GetCurrentCheckpointName();
            PlayerPrefs.SetString("CheckpointActual", checkpointActual);
        }

        // 4. Guardar el estado (abierta/cerrada) de todas las puertas de la escena
        // IMPORTANTE: incluye objetos INACTIVOS, para no perder ninguna puerta que esté oculta/desactivada.
        DoorOpen[] puertas = FindObjectsByType<DoorOpen>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (DoorOpen puerta in puertas)
        {
            PlayerPrefs.SetInt("Puerta_" + puerta.GetDoorId(), puerta.IsOpen ? 1 : 0);
        }

        // 5. Guardar qué trofeos ya fueron conseguidos
        // IMPORTANTE: incluye objetos INACTIVOS, porque el trofeo se desactiva a sí mismo al conseguirlo,
        // y sin esto, se "pierde" de la búsqueda justo cuando más importa guardarlo.
        GoalTrophy[] trofeos = FindObjectsByType<GoalTrophy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (GoalTrophy trofeo in trofeos)
        {
            string key = "Trofeo_" + trofeo.GetTrophyId();
            PlayerPrefs.SetInt(key, trofeo.FueConseguido() ? 1 : 0);
        }

        // 6. Marcar que ya existe una partida guardada
        PlayerPrefs.SetInt("TienePartida", 1);

        PlayerPrefs.Save();
        Debug.Log($"[E4All] Datos guardados - Posición: {posActual}");
    }

    public void CargarDatosJugador()
    {
        if (PlayerPrefs.GetInt("TienePartida", 0) == 1)
        {
            // Cargar Posición
            if (PlayerPrefs.GetInt("PosicionGuardada", 0) == 1)
            {
                float x = PlayerPrefs.GetFloat("PosicionX", transform.position.x);
                float y = PlayerPrefs.GetFloat("PosicionY", transform.position.y);
                float z = PlayerPrefs.GetFloat("PosicionZ", transform.position.z);

                Vector3 posGuardada = new Vector3(x, y, z);

                CharacterController controller = GetComponent<CharacterController>();
                if (controller != null) controller.enabled = false;

                transform.position = posGuardada;

                if (controller != null) controller.enabled = true;
            }

            // Cargar Salud
            if (PlayerPrefs.GetInt("VidaExiste", 0) == 1)
            {
                int vidaGuardada = PlayerPrefs.GetInt("VidaGuardada", 3);
                PlayerHealth healthScript = GetComponent<PlayerHealth>();
                if (healthScript != null)
                {
                    healthScript.SetCores(vidaGuardada);
                }
            }

            // Cargar el checkpoint actual
            PlayerRespawn respawnScript = GetComponent<PlayerRespawn>();
            if (respawnScript != null)
            {
                string checkpointGuardado = PlayerPrefs.GetString("CheckpointActual", "");
                respawnScript.SetCurrentCheckpointByName(checkpointGuardado);
            }

            // Cargar el estado de todas las puertas (incluye inactivas)
            DoorOpen[] puertas = FindObjectsByType<DoorOpen>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (DoorOpen puerta in puertas)
            {
                string key = "Puerta_" + puerta.GetDoorId();
                if (PlayerPrefs.HasKey(key))
                {
                    bool estabaAbierta = PlayerPrefs.GetInt(key, 0) == 1;
                    puerta.SetOpenState(estabaAbierta);
                }
            }

            // Cargar qué trofeos ya fueron conseguidos (incluye inactivos)
            GoalTrophy[] trofeos = FindObjectsByType<GoalTrophy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (GoalTrophy trofeo in trofeos)
            {
                string key = "Trofeo_" + trofeo.GetTrophyId();
                if (PlayerPrefs.GetInt(key, 0) == 1)
                {
                    trofeo.RestaurarComoConseguido();
                }
            }
        }
    }
}