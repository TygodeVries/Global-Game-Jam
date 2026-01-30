using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputAction walk;
    Rigidbody rb;

    [SerializeField] private float Speed;

    public void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        walk = input.actions["Walk"];

        rb = GetComponent<Rigidbody>();

        walk.Enable();
    }

    public void Update()
    {
        WalkCharacter();
    }

    public void WalkCharacter()
    {
        Vector2 move = walk.ReadValue<Vector2>();

        Vector3 dir = new Vector3(move.x, 0, move.y);

        rb.linearVelocity = dir.normalized * Speed;

        if (dir.magnitude > 0.3f)
            transform.forward = dir;
    }
}
