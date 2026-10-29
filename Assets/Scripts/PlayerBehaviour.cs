
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;

    [Header("Movimiento")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    [Header("Animaciones")]
    public float animationSmooth = 0.1f;

    private Vector2 input;
    private bool running;

    void Awake()
    {
        if (anim == null)
            anim = GetComponent<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Recoger input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        input = Vector2.ClampMagnitude(new Vector2(x, y), 1f);

        running = Input.GetKey(KeyCode.LeftShift);

        // Valores del Blend Tree
        float speedX = input.x;
        float speedY = input.y;

        // Caminar hacia delante
        if (speedY > 0 && !running)
        {
            speedY *= 0.5f;
        }

        // Actualizar animaciones
        anim.SetFloat("speedX", speedX, animationSmooth, Time.deltaTime);
        anim.SetFloat("speedY", speedY, animationSmooth, Time.deltaTime);
    }

    void FixedUpdate()
    {
        float speed = running ? runSpeed : walkSpeed;

        // Movimiento relativo al personaje
        Vector3 movement =
            transform.right * input.x +
            transform.forward * input.y;

        Vector3 velocity = movement * speed;

        // Conservar la velocidad vertical (gravedad)
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }
}
