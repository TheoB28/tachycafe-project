using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SiteOfDisproportion : MonoBehaviour
{

    [SerializeField] Canvas canvas;
    [Header("Main Tab")]
    [SerializeField] GameObject MainTab;
    [SerializeField] UnityEngine.UI.Button[] PlayerButtons;

    [Header("Stat Tab")]
    [SerializeField] GameObject StatTab;
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] TextMeshProUGUI ExpText;
    [SerializeField] TextMeshProUGUI ExpToNextLevelText;

    [SerializeField] TextMeshProUGUI VitalityText;
    [SerializeField] TextMeshProUGUI MentalityText;
    [SerializeField] TextMeshProUGUI FortitudeText;
    [SerializeField] TextMeshProUGUI PhysicalPowerText;
    [SerializeField] TextMeshProUGUI NimblenessText;
    [SerializeField] TextMeshProUGUI BrillianceText;
    [SerializeField] TextMeshProUGUI HopeText;

    [Header("UI Navigation")]
    GameObject[] StatText = new GameObject[0];
    [SerializeField] GameObject selector;

    bool levelingUp;
    int selectedStatIndex;

    
    PlayerDataHandler playerDataHandler;
    EventSystem eventSystem;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            eventSystem = GetComponentInChildren<EventSystem>();
            canvas.gameObject.SetActive(true);
            playerDataHandler = FindAnyObjectByType<PlayerDataHandler>();
            Setup();
            ArrayUtility.Clear(ref StatText);
            ArrayUtility.Add(ref StatText, VitalityText.gameObject);
            ArrayUtility.Add(ref StatText, MentalityText.gameObject);
            ArrayUtility.Add(ref StatText, FortitudeText.gameObject);
            ArrayUtility.Add(ref StatText, PhysicalPowerText.gameObject);
            ArrayUtility.Add(ref StatText, NimblenessText.gameObject);
            ArrayUtility.Add(ref StatText, BrillianceText.gameObject);
            ArrayUtility.Add(ref StatText, HopeText.gameObject);
            other.gameObject.GetComponent<PlayerOverworld>().InMenu = true;
        }
        
    }

    void Setup()
    {
        MainTab.SetActive(true);
        TextMeshProUGUI text;
        int i = 0;
        foreach(UnityEngine.UI.Button button in PlayerButtons)
        {
            text = PlayerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (i >= playerDataHandler.playerData.Length)
            {
                button.gameObject.SetActive(false);
                continue;
            } else
            {
                text.text = playerDataHandler.playerData[i].PlayerName;
            }
            i++;
        }
        PlayerButtons[0].Select();
    }

    public void SetupPlayerStats(int playerID)
    {
        MainTab.SetActive(false);
        StatTab.SetActive(true);
        selectedStatIndex = 0;
        LevelText.text = playerDataHandler.playerData[playerID].Level.ToString();
        ExpText.text = playerDataHandler.playerData[playerID].Exp.ToString();
        ExpToNextLevelText.text = playerDataHandler.playerData[playerID].ExpToNextLevel.ToString();
        VitalityText.text = playerDataHandler.playerData[playerID].Vitality.ToString();
        MentalityText.text = playerDataHandler.playerData[playerID].Mentality.ToString();
        FortitudeText.text = playerDataHandler.playerData[playerID].Fortitude.ToString();
        PhysicalPowerText.text = playerDataHandler.playerData[playerID].PhysicalPower.ToString();
        NimblenessText.text = playerDataHandler.playerData[playerID].Nimbleness.ToString();
        BrillianceText.text = playerDataHandler.playerData[playerID].Brilliance.ToString();
        HopeText.text = playerDataHandler.playerData[playerID].Hope.ToString();
        levelingUp = true;

        VitalityText.AddComponent<Selectable>();
        MentalityText.AddComponent<Selectable>();
        FortitudeText.AddComponent<Selectable>();
        PhysicalPowerText.AddComponent<Selectable>();
        NimblenessText.AddComponent<Selectable>();
        BrillianceText.AddComponent<Selectable>();
        HopeText.AddComponent<Selectable>();

        VitalityText.GetComponent<Selectable>().Select();
        selector.transform.position = VitalityText.transform.position;
    }

    private void Update()
    {
        if (levelingUp)
        {
            selector.transform.position = eventSystem.currentSelectedGameObject.transform.position;
        }

    }
}
