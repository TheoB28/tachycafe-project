using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOverworld : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    public bool InMenu;

    public SiteOfDisproportion currentSite;
    Rigidbody2D Rigidbody;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value)
    {
        if (InMenu) 
        {
            if (currentSite != null && currentSite.levelingUp)
            {
                currentSite.OnPlayerMove(value);
            }
            Rigidbody.linearVelocity = Vector2.zero;

            return; 
        }
        Vector2 movementInput = value.Get<Vector2>();
        Rigidbody.linearVelocity = movementInput * moveSpeed;
    }

    void OnCancel()
    {
        if (InMenu && currentSite != null && InMenu)
        {
            currentSite.OnPlayerCancel();
        }
    }
}
