using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputAction walk;
    Rigidbody rb;

    [SerializeField] private float Speed;

    Animator animator;
    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
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

        if (dir.magnitude > 0.3f)
        {
            animator.SetBool("IsWalking", true);
            rb.linearVelocity = dir.normalized * Speed;
            transform.forward = dir;
        }
        else
        {
            animator.SetBool("IsWalking", false);
            rb.linearVelocity = new Vector3(0, 0, 0);
        }
    }
}
