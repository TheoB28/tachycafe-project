using UnityEngine;
using UnityEngine.EventSystems;

public class UIActionItem : MonoBehaviour, ISelectHandler
{

    SiteOfDisproportion SiteOfDisproportion;

    private void Start()
    {
        SiteOfDisproportion = GetComponentInParent<SiteOfDisproportion>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SiteOfDisproportion.MoveScroll(transform);
    }
}
