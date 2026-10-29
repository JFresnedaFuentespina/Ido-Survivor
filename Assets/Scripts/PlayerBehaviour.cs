
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;

    [Header("Movimiento")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    private Vector2 input;
    private bool running;

    void Awake()
    {
        // if (anim == null)
        //     anim = GetComponentInChildren<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (anim == null)
            Debug.LogError("No se encuentra el Animator.");

        if (rb == null)
            Debug.LogError("No se encuentra el Rigidbody.");
    }

    void Update()
    {
        if (anim == null)
            return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        input = Vector2.ClampMagnitude(new Vector2(x, y), 1f);
        running = Input.GetKey(KeyCode.LeftShift);

        float speedX = input.x;
        float speedY = input.y;

        // Caminar hacia delante
        if (speedY > 0f && !running)
            speedY *= 0.5f;

        // Asignar directamente, sin suavizado
        anim.SetFloat("speedX", speedX);
        anim.SetFloat("speedY", speedY);
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        float speed = running ? runSpeed : walkSpeed;

        Vector3 movement =
            transform.right * input.x +
            transform.forward * input.y;

        Vector3 velocity = movement * speed;

        // Conservar la gravedad
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }
}
