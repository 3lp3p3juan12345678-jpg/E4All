using UnityEngine;

public class TrofeoPuerta : MonoBehaviour
{
    [Header("Luces de la Puerta (Componente Light)")]
    [Tooltip("Arrastra aquí la primera luz roja")]
    public Light luz1;
    
    [Tooltip("Arrastra aquí la segunda luz roja")]
    public Light luz2;

    [Header("Modelos 3D de los Focos (Opcional)")]
    [Tooltip("Si el modelo 3D del foco cambia de color de material, arrástralo aquí")]
    public Renderer focoMesh1;
    public Renderer focoMesh2;

    [Header("Configuración")]
    public Color colorVerde = Color.green;
    public string tagJugador = "Player";
    
    [Tooltip("¿El trofeo debe desaparecer al tocarlo?")]
    public bool desaparecerAlTocar = true;

    private bool yaActivado = false;

    // Se ejecuta si el trofeo tiene marcado 'Is Trigger'
    private void OnTriggerEnter(Collider other)
    {
        if (!yaActivado && other.CompareTag(tagJugador))
        {
            CambiarAVerde();
        }
    }

    // Se ejecuta si el trofeo es un objeto sólido (sin 'Is Trigger')
    private void OnCollisionEnter(Collision collision)
    {
        if (!yaActivado && collision.gameObject.CompareTag(tagJugador))
        {
            CambiarAVerde();
        }
    }

    private void CambiarAVerde()
    {
        yaActivado = true;

        // 1. Cambiar el color de las luces de Unity
        if (luz1 != null) luz1.color = colorVerde;
        if (luz2 != null) luz2.color = colorVerde;

        // 2. Cambiar el color del material 3D del foco (si los asignaron)
        if (focoMesh1 != null) focoMesh1.material.color = colorVerde;
        if (focoMesh2 != null) focoMesh2.material.color = colorVerde;

        Debug.Log("¡Trofeo tocado! Luces cambiadas a verde.");

        // 3. Desaparecer el trofeo si la opción está activa
        if (desaparecerAlTocar)
        {
            gameObject.SetActive(false);
        }
    }
}