using UnityEngine;

public class RobotCinematico : MonoBehaviour
{
    [Header("Configuración de Caminata")]
    public float velocidad = 2f;
    public float tiempoDeCaminata = 4f;

    [Header("Configuración de Animación")]
    public Animator animator;
    [Tooltip("Escribe aquí el nombre exacto de la animación o del estado de caminata en tu Animator")]
    public string nombreEstadoAnimacion = "Walk"; 

    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;
    private float temporizador = 0f;

    void Start()
    {
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;

        // Si no arrastraste el Animator, busca el componente automáticamente
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        temporizador += Time.deltaTime;

        if (temporizador < tiempoDeCaminata)
        {
            // 1. Mueve el robot hacia adelante
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

            // 2. Fuerzale al Animator a reproducir la animación de caminata
            if (animator != null && !string.IsNullOrEmpty(nombreEstadoAnimacion))
            {
                animator.Play(nombreEstadoAnimacion);
            }
        }
        else
        {
            // Reset de posición
            transform.position = posicionInicial;
            transform.rotation = rotacionInicial;
            temporizador = 0f;
        }
    }
}