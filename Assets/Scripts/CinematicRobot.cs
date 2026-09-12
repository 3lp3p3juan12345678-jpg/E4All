using UnityEngine;

public class CinematicRobot : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 2f;
    public Transform targetDestination; // Arrastra un objeto vacío aquí para que camine hacia él
    public bool moveForwardOnStart = false; // Activa esto si solo quieres que camine recto

    private Animator animator;
    private bool isMoving = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (targetDestination != null || moveForwardOnStart)
        {
            StartWalking();
        }
    }

    void Update()
    {
        if (!isMoving) return;

        if (moveForwardOnStart)
        {
            // Moverse hacia adelante en línea recta
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else if (targetDestination != null)
        {
            // Moverse hacia el punto de destino
            Vector3 targetPosition = new Vector3(targetDestination.position.x, transform.position.y, targetDestination.position.z);
            
            // Rotar hacia el destino
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            // Desplazarse hacia el punto
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Si llega al punto, se detiene
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                StopWalking();
            }
        }
    }

    // Método para iniciar la caminata y activar la animación
    public void StartWalking()
    {
        isMoving = true;

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
            animator.SetFloat("Speed", moveSpeed);
            animator.SetBool("isGrounded", true);
        }
    }

    // Método para detener al robot
    public void StopWalking()
    {
        isMoving = false;

        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetFloat("Speed", 0f);
        }
    }
}