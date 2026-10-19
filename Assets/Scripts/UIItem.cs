using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, ISelectHandler
{
    public enum Type {Action, Quest, Item}
    public Type type;
    public Action action;
    public Quest quest;

    PlayerOverworld playerOverworld;
    SiteOfDisproportion SiteOfDisproportion;

    private void Start()
    {
        switch (type)
        {
            case Type.Action:
                SiteOfDisproportion = GetComponentInParent<SiteOfDisproportion>();
                break;
            case Type.Quest:
                playerOverworld = FindAnyObjectByType<PlayerOverworld>();
                break;
        }

    }

    public void OnSelect(BaseEventData eventData)
    {
        switch (type)
        {
            case Type.Action:
                SiteOfDisproportion.MoveScroll(transform);
                SiteOfDisproportion.AddDiscription(action);
                break;
            case Type.Quest:
                playerOverworld.MoveScroll(transform);
                playerOverworld.addQuestDescription(quest);
                break;
        }

    }



}
