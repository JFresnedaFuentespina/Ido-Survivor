
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform lookpoint;
    public Transform head;

    [Header("Mouse")]
    public float sensitivity = 0.5f;

    [Header("Look Point")]
    public float horizontalRange = 0.8f;
    public float verticalRange = 0.4f;
    public float forwardDistance = 2f;
    public float lookPointSmoothTime = 0.12f;

    [Header("Head rotation")]
    public float maxHeadAngle = 25f;
    public float headSpeed = 5f;
    private Quaternion headStartRotation;

    private float lookX;
    private float lookY;

    private float targetX;
    private float targetY;

    private Vector3 localStartPos;
    private Vector3 lookPointVelocity;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        localStartPos = lookpoint.localPosition;
        headStartRotation = head.localRotation;
    }

    void Update()
    {
        // Movimiento del ratón
        targetX += Input.GetAxis("Mouse X") * sensitivity;
        targetY += Input.GetAxis("Mouse Y") * sensitivity;

        // Limitar movimiento
        targetX = Mathf.Clamp(targetX, -horizontalRange, horizontalRange);
        targetY = Mathf.Clamp(targetY, -verticalRange, verticalRange);
    }

    void LateUpdate()
    {
        MoveLookPoint();
        RotateHead();
    }
    void MoveLookPoint()
    {
        lookX = Mathf.SmoothDamp(
            lookX,
            targetX,
            ref lookXVelocity,
            lookPointSmoothTime
        );

        lookY = Mathf.SmoothDamp(
            lookY,
            targetY,
            ref lookYVelocity,
            lookPointSmoothTime
        );

        Vector3 desiredPosition = localStartPos +
            new Vector3(lookX, lookY, 0);

        lookpoint.localPosition = desiredPosition;
    }


    void RotateHead()
    {
        // Dirección hacia el LookPoint en coordenadas locales del padre
        Vector3 dir = head.parent.InverseTransformDirection(
            lookpoint.position - head.position
        ).normalized;

        // Ángulos respecto al eje -Z
        float yaw = Mathf.Atan2(-dir.x, -dir.z) * Mathf.Rad2Deg;

        float pitch = Mathf.Atan2(
            dir.y,
            Mathf.Sqrt(dir.x * dir.x + dir.z * dir.z)
        ) * Mathf.Rad2Deg;

        yaw = Mathf.Clamp(yaw, -maxHeadAngle, maxHeadAngle);
        pitch = Mathf.Clamp(pitch, -maxHeadAngle, maxHeadAngle);

        // Aplicamos el movimiento sobre la rotación inicial
        Quaternion offset = Quaternion.Euler(-pitch, yaw, 0);

        Quaternion targetRotation = headStartRotation * offset;

        head.localRotation = Quaternion.Slerp(
            head.localRotation,
            targetRotation,
            1f - Mathf.Exp(-headSpeed * Time.deltaTime)
        );
    }

    void RotateHead2()
    {

        Vector3 dir = lookpoint.position - head.position;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        Quaternion localTarget = Quaternion.Inverse(head.parent.rotation) * targetRot;

        Vector3 euler = localTarget.eulerAngles;

        if (euler.x > 180) euler.x -= 360;
        if (euler.y > 180) euler.y -= 360;

        euler.x = Mathf.Clamp(euler.x, -maxHeadAngle, maxHeadAngle);
        euler.y = Mathf.Clamp(euler.y, -maxHeadAngle, maxHeadAngle);

        Quaternion limitedRot = Quaternion.Euler(euler);

        head.localRotation = limitedRot;
    }



    private float lookXVelocity;
    private float lookYVelocity;
}
