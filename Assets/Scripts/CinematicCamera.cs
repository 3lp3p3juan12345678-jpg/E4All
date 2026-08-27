using UnityEngine;

// 1. Creamos un "paquete" de opciones para cada toma
[System.Serializable]
public class TomaCamara
{
    [Tooltip("El objeto vacío que indica desde dónde graba la cámara.")]
    public Transform posicionToma; 
    
    [Tooltip("Activa esto si quieres que en ESTA toma la cámara siga a un objetivo.")]
    public bool forzarMirarObjetivo = false;
    
    [Tooltip("Arrastra aquí a quién debe mirar en esta toma (Robot, Trofeo, etc.)")]
    public Transform objetivoEspecifico; 
}

public class CinematicCamera : MonoBehaviour
{
    [Header("Director de Cine")]
    // 2. Ahora usamos nuestro paquete para la lista de tomas
    public TomaCamara[] tomas; 
    public float tiempoPorToma = 4f;
    public float velocidadPaneo = 0.5f;

    private int tomaActual = 0;
    private float temporizador = 0f;

    void Start()
    {
        if (tomas.Length > 0) CambiarToma(0);
    }

    void Update()
    {
        if (tomas.Length == 0) return;

        // Temporizador de cortes
        temporizador += Time.deltaTime;
        if (temporizador >= tiempoPorToma)
        {
            temporizador = 0f;
            tomaActual++;
            if (tomaActual >= tomas.Length) tomaActual = 0;
            CambiarToma(tomaActual);
        }

        // 3. Comprobamos las opciones de la toma actual
        TomaCamara tomaActiva = tomas[tomaActual];

        if (tomaActiva.forzarMirarObjetivo && tomaActiva.objetivoEspecifico != null)
        {
            // Si la toma tiene un objetivo, lo mira fijamente
            transform.LookAt(tomaActiva.objetivoEspecifico);
        }

        // Movimiento de paneo lento
        transform.Translate(Vector3.right * velocidadPaneo * Time.deltaTime);
    }

    void CambiarToma(int indice)
    {
        // Nos aseguramos de que asignaste el punto de la toma
        if (tomas[indice].posicionToma != null)
        {
            transform.position = tomas[indice].posicionToma.position;
            transform.rotation = tomas[indice].posicionToma.rotation; 
        }
        else
        {
            Debug.LogWarning("¡Cuidado! Te falta asignar la Posición Toma en la toma número: " + indice);
        }
    }
}