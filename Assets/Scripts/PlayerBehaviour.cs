
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Animator anim;

    [Header("Movimiento")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    [Header("Animaciones")]
    public float animationSmooth = 0.1f;

    void Update()
    {
        // Obtener input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(x, y);

        // Evitar que diagonal sea más rápida
        input = Vector2.ClampMagnitude(input, 1f);

        // Comprobar si está corriendo
        bool running = Input.GetKey(KeyCode.LeftShift);

        // Valores para el Blend Tree
        float speedX = input.x;
        float speedY = input.y;

        // Caminar hacia delante utiliza 0.5
        if (speedY > 0 && !running)
        {
            speedY *= 0.5f;
        }

        // Actualizar Animator
        anim.SetFloat("speedX", speedX, animationSmooth, Time.deltaTime);
        anim.SetFloat("speedY", speedY, animationSmooth, Time.deltaTime);

        // Movimiento real del personaje
        float speed = running ? runSpeed : walkSpeed;

        Vector3 movement =
            transform.right * input.x +
            transform.forward * input.y;

        transform.position += movement * speed * Time.deltaTime;
    }
}
