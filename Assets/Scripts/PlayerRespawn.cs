using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Configuración de Spawns")]
    public Transform initialSpawnPoint;      // El SpawnPoint principal (al que vuelve si muere sin núcleos o cruza el portal)
    public Transform currentSpawnPoint;    // Dónde va a reaparecer actualmente (se actualiza con checkpoints)
    public float minYLimit = -20f;         // Límite de altura por si cae al vacío

    private Rigidbody rb;
    private PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerHealth = GetComponent<PlayerHealth>();

        // Si no hay un SpawnPoint principal asignado, busca el primero disponible en la escena
        if (initialSpawnPoint == null)
        {
            GameObject firstSpawn = GameObject.FindWithTag("SpawnPoint");
            if (firstSpawn != null)
            {
                initialSpawnPoint = firstSpawn.transform;
            }
        }

        // Al iniciar, el spawn actual es el principal
        if (currentSpawnPoint == null)
        {
            currentSpawnPoint = initialSpawnPoint;
        }
    }

    void Update()
    {
        // Muerte por caer al vacío global del nivel
        if (transform.position.y < minYLimit)
        {
            LoseLifeAndRespawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Si toca una zona de muerte (DeadZone)
        if (other.CompareTag("DeadZone"))
        {
            LoseLifeAndRespawn();
        }

        // 2. Si pasa por un nuevo Checkpoint para actualizar la reaparición
        else if (other.CompareTag("Checkpoint"))
        {
            currentSpawnPoint = other.transform;
            Debug.Log("¡Checkpoint actualizado!");
        }

        // 3. Si toca un Portal: Te cura al máximo Y te manda al SpawnPoint origen
        else if (other.CompareTag("Portal"))
        {
            if (playerHealth != null)
            {
                playerHealth.ResetHealth(); // ¡Te devuelve los 3 núcleos al máximo!
                Debug.Log("¡Portal cruzado! Salud restaurada al máximo.");
            }

            // Te teletransporta al SpawnPoint principal de origen
            RespawnToInitial();
        }
    }

    // Método centralizado para quitar vida y decidir a dónde reaparecer (solo para zonas de muerte/vacío)
    void LoseLifeAndRespawn()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage();
        }
        else
        {
            RespawnToCurrent();
        }
    }

    // Respawn normal (al último checkpoint activo)
    public void RespawnToCurrent()
    {
        if (currentSpawnPoint != null)
        {
            ResetPhysics();
            transform.position = currentSpawnPoint.position;
            transform.rotation = currentSpawnPoint.rotation;
        }
        else
        {
            ResetPhysics();
            transform.position = Vector3.zero;
        }
    }

    // Respawn al inicio absoluto (cuando se queda a 0 núcleos o cruza el portal)
    public void RespawnToInitial()
    {
        if (initialSpawnPoint != null)
        {
            ResetPhysics();
            transform.position = initialSpawnPoint.position;
            transform.rotation = initialSpawnPoint.rotation;
            currentSpawnPoint = initialSpawnPoint; // Reinicia también el checkpoint actual al origen
            Debug.Log("¡Teletransportado al SpawnPoint principal!");
        }
        else
        {
            ResetPhysics();
            transform.position = Vector3.zero;
        }
    }

    void ResetPhysics()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // ---- Para el sistema de guardado ----

    // Devuelve el nombre del checkpoint actual (para poder guardarlo)
    public string GetCurrentCheckpointName()
    {
        return currentSpawnPoint != null ? currentSpawnPoint.name : "";
    }

    // Busca en la escena un checkpoint por su nombre y lo asigna como actual (para restaurarlo al cargar)
    public void SetCurrentCheckpointByName(string checkpointName)
    {
        if (string.IsNullOrEmpty(checkpointName)) return;

        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (GameObject cp in checkpoints)
        {
            if (cp.name == checkpointName)
            {
                currentSpawnPoint = cp.transform;
                return;
            }
        }
    }
}