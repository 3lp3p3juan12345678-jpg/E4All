using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [Header("Identificador único de esta puerta (opcional)")]
    [Tooltip("Solo si quieres un nombre más legible, ej: PuertaFacil. Si lo dejas vacío, se genera uno automático según su posición en la jerarquía (recomendado si tienes muchas puertas, como las de las preguntas).")]
    public string doorId;

    [Header("Interacción")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactDistance = 3.5f;
    [SerializeField] private GameObject promptUI;

    [Header("Rotación de la puerta")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 3f;

    [Header("Sonidos de la puerta")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private Transform playerTransform;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;

        // Si no se asignó manualmente un AudioSource, busca uno en este mismo GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Busca al jugador por su Tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool inRange = distance <= interactDistance;

        if (promptUI != null)
        {
            promptUI.SetActive(inRange);
        }

        if (inRange && Input.GetKeyDown(interactKey))
        {
            isOpen = !isOpen;
            PlayDoorSound();
        }

        // Animación suave hacia el estado objetivo
        Quaternion target = isOpen ? openRotation : closedRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * openSpeed);
    }

    private void PlayDoorSound()
    {
        if (audioSource == null) return;

        AudioClip clipToPlay = isOpen ? openSound : closeSound;

        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }

    // ---- Para el sistema de guardado ----

    // Devuelve el identificador único: el que escribiste a mano, o si está vacío,
    // uno generado automático según su posición exacta en la jerarquía (nunca se repite).
    public string GetDoorId()
    {
        if (!string.IsNullOrEmpty(doorId))
        {
            return doorId;
        }

        string path = gameObject.name;
        Transform current = transform.parent;
        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }
        return path;
    }

    // Permite que otros scripts (PlayerPositionSave) lean si está abierta
    public bool IsOpen => isOpen;

    // Permite restaurar el estado guardado. instant=true la coloca de una vez, sin animar ni sonar.
    public void SetOpenState(bool open, bool instant = true)
    {
        isOpen = open;
        if (instant)
        {
            transform.localRotation = isOpen ? openRotation : closedRotation;
        }
    }
}