using System.Linq;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    [SerializeField] Quest quest;
    public bool IsActiveQuest = false;


    PlayerDataHandler playerDataHandler;
    private void Start()
    {
        playerDataHandler = FindAnyObjectByType<PlayerDataHandler>();
        foreach(Quest quests in playerDataHandler.Quests)
        {
            if (quests == quest)
            {
                IsActiveQuest = true;
            }
        }
        if (!IsActiveQuest) { gameObject.SetActive(false); }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            quest.HasItem = true;
            Destroy(gameObject);
        }
    }
}
