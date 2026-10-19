using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] Button backButton;
    [SerializeField] RectTransform[] children;

    public void SetMenu(bool active)
    {
        menu.SetActive(!active);
        PlayerOverworld playerOverworld = FindAnyObjectByType<PlayerOverworld>();
        if (playerOverworld != null)
        {
            playerOverworld.CloseQuestMenu();
        }
        foreach (var child in children)
        {
            child.gameObject.SetActive(active);
        }
        EventSystem eventSystem = EventSystem.current;
        switch (active)
        {
            case true:
                eventSystem.SetSelectedGameObject(backButton.gameObject);
                break;
            case false:
                eventSystem.SetSelectedGameObject(FindAnyObjectByType<MainMenu>()?.GetComponent<MainMenu>().continueButton.gameObject);
                if(eventSystem.currentSelectedGameObject == null)
                {
                    eventSystem.SetSelectedGameObject(FindAnyObjectByType<PlayerOverworld>()?.GetComponent<PlayerOverworld>().MainMenuButton.gameObject);
                }
                break;

        }
    }
}
