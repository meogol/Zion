using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    public void Move(Vector3 _vel)
    {
        velocity = _vel;
    }

    private void FixedUpdate()
    {
        PerformMove();

    }

    void PerformMove()
    {
        if (velocity != Vector3.zero)
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
