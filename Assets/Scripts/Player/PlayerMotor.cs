using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Rigidbody rb;
    private float Speed = 1.2f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 rotationCamera = Vector3.zero;
    private Vector3 euler;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        euler = transform.localEulerAngles;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Move(Vector3 _vel)
    {
        velocity = _vel;
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCam(Vector3 _rotationCam)
    {
        rotationCamera = _rotationCam;
    }
    private void FixedUpdate()
    {
        PerformMove();
        PerformRotation();

    }

    void PerformMove()
    {
        if (velocity != Vector3.zero)
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void PerformRotation()
    {
        euler.x -= Input.GetAxis("Mouse Y") * Speed;
        euler.x = Mathf.Clamp(euler.x, -80.0f, 70.0f);
        euler.y += Input.GetAxis("Mouse X") * Speed;
        transform.localEulerAngles = euler;

        /*rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null)
        {
            cam.transform.Rotate(-rotationCamera);
        }*/
    }
}
