using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator == null) return;

        // Detecta si hay entrada en los ejes WASD o Flechas
        bool isKeyPressed = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        // IsWalking es true al presionar movimiento, y false al soltar las teclas
        bool isWalking = isKeyPressed;

        animator.SetBool("IsWalking", isWalking);
    }
}