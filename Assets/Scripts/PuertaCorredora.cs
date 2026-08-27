using UnityEngine;

public class PuertaCorredora : MonoBehaviour
{
    [Header("Referencias de Objetos")]
    public GameObject puertaIzquierda;
    public GameObject puertaDerecha;
    public AudioSource audioPuerta;

    [Header("Configuración")]
    public float distanciaApertura = 0.8f;
    public float velocidad = 2.0f;
    
    [Header("Detección")]
    public Transform jugador; 
    public float distanciaMaxima = 3.0f; 

    private bool estaAbierta = false;
    private Vector3 posInicialIzq, posInicialDer;

    void Start()
    {
        posInicialIzq = puertaIzquierda.transform.localPosition;
        posInicialDer = puertaDerecha.transform.localPosition;
    }

    void Update()
    {
        // Calculamos la distancia constantemente
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);
        
        // LÓGICA DE DETECCIÓN:
        // Si el jugador está cerca, forzamos 'estaAbierta' a true.
        // Si el jugador se aleja, forzamos 'estaAbierta' a false.
        if (distanciaAlJugador <= distanciaMaxima)
        {
            if (!estaAbierta) // Solo suena si estaba cerrada y ahora se abre
            {
                estaAbierta = true;
                if (audioPuerta != null) audioPuerta.Play();
            }
        }
        else
        {
            if (estaAbierta) // Solo suena si estaba abierta y ahora se cierra
            {
                estaAbierta = false;
                if (audioPuerta != null) audioPuerta.Play();
            }
        }

        // Movimiento suave hacia la posición objetivo
        Vector3 targetIzq = estaAbierta ? posInicialIzq + new Vector3(-distanciaApertura, 0, 0) : posInicialIzq;
        Vector3 targetDer = estaAbierta ? posInicialDer + new Vector3(distanciaApertura, 0, 0) : posInicialDer;

        puertaIzquierda.transform.localPosition = Vector3.Lerp(puertaIzquierda.transform.localPosition, targetIzq, Time.deltaTime * velocidad);
        puertaDerecha.transform.localPosition = Vector3.Lerp(puertaDerecha.transform.localPosition, targetDer, Time.deltaTime * velocidad);
    }
}