using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 6f;
    public float sprintMultiplier = 1.5f;

    [Header("Protección de Paredes (SOLO PAREDES)")]
    public float playerRadius = 0.45f; 
    [Tooltip("Selecciona AQUÍ únicamente la capa donde están las paredes")]
    public LayerMask wallLayer; 

    [Header("Cámaras (Asignar en el Inspector)")]
    public GameObject firstPersonCamera;
    public GameObject thirdPersonCamera;
    private bool isFirstPerson = true;

    [Header("Cámara y Vista (Ratón)")]
    public Transform cameraHolder;
    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;

    [Header("Límites Primera Persona")]
    public float fpLookUpLimit = 85f;
    public float fpLookDownLimit = 2f;

    [Header("Límites Tercera Persona")]
    public float tpsMinVerticalAngle = -10f;
    public float tpsMaxVerticalAngle = 85f;

    [Header("Headbob (Balanceo)")]
    public bool enableHeadBob = true;
    public float walkBobSpeed = 14f;
    public float walkBobAmount = 0.05f;
    public float runBobSpeed = 18f;
    public float runBobAmount = 0.1f;
    private float defaultCameraY;
    private float bobTimer = 0f;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private Vector2 moveInput;
    private bool isRunning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        UpdateCameraViews();

        if (cameraHolder != null)
        {
            defaultCameraY = cameraHolder.localPosition.y;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        moveInput = new Vector2(x, z);

        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 0.9f, 0), 0.3f, ~LayerMask.GetMask("Player"));

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isFirstPerson = !isFirstPerson;
            UpdateCameraViews();
            
            verticalRotation = 0f;
            horizontalRotation = transform.eulerAngles.y;
        }

        HandleMouseLook();

        if (isFirstPerson && enableHeadBob)
        {
            HandleHeadBob();
        }

        // --- ACTUALIZACIÓN DE ANIMACIÓN ---
        if (animator != null)
        {
            float targetSpeed = moveInput.magnitude;
            if (isRunning && targetSpeed > 0.1f) targetSpeed *= 2f;
            animator.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = speed;
        if (isRunning && moveInput.y > 0)
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 moveDirection;

        if (isFirstPerson)
        {
            moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        }
        else
        {
            Quaternion camRotation = Quaternion.Euler(0, horizontalRotation, 0);
            Vector3 camForward = camRotation * Vector3.forward;
            Vector3 camRight = camRotation * Vector3.right;

            moveDirection = camRight * moveInput.x + camForward * moveInput.y;

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, 720f * Time.fixedDeltaTime));
            }
        }

        // --- PROTECCIÓN DE PAREDES ---
        if (moveDirection.magnitude > 0.01f)
        {
            float moveDistance = currentSpeed * Time.fixedDeltaTime;

            if (!Physics.SphereCast(rb.position + Vector3.up * 0.5f, playerRadius, moveDirection.normalized, out RaycastHit hit, moveDistance + 0.05f, wallLayer))
            {
                rb.MovePosition(rb.position + moveDirection * moveDistance);
            }
            else
            {
                Vector3 slideDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal).normalized;
                
                if (!Physics.SphereCast(rb.position + Vector3.up * 0.5f, playerRadius, slideDirection, out _, moveDistance + 0.05f, wallLayer))
                {
                    rb.MovePosition(rb.position + slideDirection * moveDistance);
                }
            }
        }
    }

    void HandleMouseLook()
    {
        if (cameraHolder == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (isFirstPerson)
        {
            transform.Rotate(Vector3.up * mouseX);
            
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -fpLookUpLimit, fpLookDownLimit);
            
            cameraHolder.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            cameraHolder.localPosition = new Vector3(0f, defaultCameraY, 0f);
        }
        else
        {
            horizontalRotation += mouseX;
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, tpsMinVerticalAngle, tpsMaxVerticalAngle);

            cameraHolder.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
            cameraHolder.position = transform.position + Vector3.up * 1.2f;
        }
    }

    void HandleHeadBob()
    {
        if (cameraHolder == null) return;

        bool isMoving = moveInput.magnitude > 0.1f && isGrounded;

        if (isMoving)
        {
            float currentBobSpeed = isRunning ? runBobSpeed : walkBobSpeed;
            float currentBobAmount = isRunning ? runBobAmount : walkBobAmount;

            bobTimer += Time.deltaTime * currentBobSpeed;
            float targetY = defaultCameraY + Mathf.Sin(bobTimer) * currentBobAmount;

            Vector3 localPos = cameraHolder.localPosition;
            cameraHolder.localPosition = new Vector3(localPos.x, Mathf.Lerp(localPos.y, targetY, Time.deltaTime * 10f), localPos.z);
        }
        else
        {
            bobTimer = 0;
            Vector3 localPos = cameraHolder.localPosition;
            cameraHolder.localPosition = new Vector3(localPos.x, Mathf.Lerp(localPos.y, defaultCameraY, Time.deltaTime * 10f), localPos.z);
        }
    }

    void UpdateCameraViews()
    {
        if (firstPersonCamera != null) firstPersonCamera.SetActive(isFirstPerson);
        if (thirdPersonCamera != null) thirdPersonCamera.SetActive(!isFirstPerson);
    }
}