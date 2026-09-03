using UnityEngine;

public class Mirror : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<PlayerDataHandler>().playerData[0].ChangeGenderLevel(-10);
        }
    }
}
