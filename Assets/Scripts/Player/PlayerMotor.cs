using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;


    [SerializeField]
    private GameObject playerHead;


    [SerializeField]
    private GameObject playerBody;


    private Rigidbody rb;
    private float Speed = 1.2f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 rotationCamera = Vector3.zero;
    private Vector3 euler;
    private Vector3 eulerBody;
    private Vector3 eulerHead;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        euler = transform.localEulerAngles;
        eulerBody = transform.localEulerAngles;
        eulerHead = transform.localEulerAngles;
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
        PerformRotation();

    }

    void PerformRotation()
    {
        euler.x -= Input.GetAxis("Mouse Y") * Speed;
        euler.x =  Mathf.Clamp(euler.x, -80.0f, 30.0f);
        cam.transform.localEulerAngles = euler;


        eulerHead.z -= Input.GetAxis("Mouse Y") * Speed;
        eulerHead.z = Mathf.Clamp(euler.x, -80.0f, 70.0f);
        playerHead.transform.localEulerAngles = eulerHead;


        eulerBody.y += Input.GetAxis("Mouse X") * Speed;
        playerBody.transform.localEulerAngles = eulerBody;
    }
}
