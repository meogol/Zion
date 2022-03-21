using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float lookspeed;

    private PlayerMotor motor;

    private Rigidbody rb;

    private void ChangeSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) != false)
        {
            speed = 20f;
        }
        else
        {
            speed = 10f;
        }
    }

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        ChangeSpeed();


        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 movHor = transform.right * xMov;
        Vector3 movVer = transform.forward * zMov;

        Vector3 velocity = (movHor + movVer).normalized * speed;
        if (rb.velocity.magnitude >= speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }

        rb.AddForce(velocity);

        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookspeed;

        motor.Rotate(rotation);

        float xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 camRotation = new Vector3(xRot, 0f, 0f) * lookspeed;

        motor.RotateCam(camRotation);
    }
}
