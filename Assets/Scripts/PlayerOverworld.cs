using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOverworld : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    public bool InMenu, InDialoge;

    Vector2 movementInput;
    Vector2 lookvector;

    public SiteOfDisproportion currentSite;
    public NPC currentNPC;
    Rigidbody2D Rigidbody;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value)
    {
        if (InMenu) 
        {
            if (currentSite != null)
            {
                currentSite.OnPlayerMove(value);
            }
            Rigidbody.linearVelocity = Vector2.zero;

            return; 
        }
        movementInput = value.Get<Vector2>();
        if (movementInput != Vector2.zero)
        {
            lookvector = movementInput;
        }
        Rigidbody.linearVelocity = movementInput * moveSpeed;
    }

    void OnCancel()
    {
        if (InMenu && currentSite != null)
        {
            currentSite.OnPlayerCancel();
        }
        else if (InMenu && currentSite.InMainTab)
        {
            currentSite = null;
        }
    }

    void OnSubmit()
    {
        if (InMenu && currentSite != null)
        {
            currentSite.OnPlayerSubmit();
        }
        else if (InMenu && InDialoge)
        {
            currentNPC.NextText();
        }
        else if (!InMenu)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(lookvector.x, lookvector.y, 0), lookvector, 1);
            if (hit.collider != null && hit.transform.CompareTag("NPC"))
            {
                hit.transform.GetComponent<NPC>().Interact(this);
                currentNPC = hit.transform.GetComponent<NPC>();
                InMenu = true;
                InDialoge = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(lookvector.x, lookvector.y, 0), (Vector2)transform.position + lookvector);
    }
}
