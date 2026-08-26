using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOverworld : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    Rigidbody2D Rigidbody;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value)
    {
        Vector2 movementInput = value.Get<Vector2>();
        Rigidbody.linearVelocity = movementInput * moveSpeed;
    }
}
