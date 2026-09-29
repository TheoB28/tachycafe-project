using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIActionItem : MonoBehaviour, ISelectHandler
{
    public Action action;
    SiteOfDisproportion SiteOfDisproportion;

    private void Start()
    {
        SiteOfDisproportion = GetComponentInParent<SiteOfDisproportion>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SiteOfDisproportion.MoveScroll(transform);
        SiteOfDisproportion.AddDiscription(action);
    }



}
