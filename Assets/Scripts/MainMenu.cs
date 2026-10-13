using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public Button continueButton;

    [SerializeField] GameObject SettingsMenu;


   EventSystem eventSystem;

    void Start()
    {
        eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(continueButton.gameObject);
        
    }

    public void Setup(SceneLoader sceneLoader)
    {
        continueButton.onClick.AddListener(sceneLoader.LoadOverworld);
        continueButton.onClick.AddListener(delegate { sceneLoader.LoadOverworld(); });
        continueButton.onClick.AddListener (() => {sceneLoader.LoadOverworld();});
    }

}
