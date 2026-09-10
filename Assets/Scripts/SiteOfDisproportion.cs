using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] TextMeshProUGUI SkillPointsText;

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
    int selectedStatIndex, currentPlayer, tempSP;
    int[] orgStats = new int[0];
    int[] tempStats;
    
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
        currentPlayer = playerID;
        MainTab.SetActive(false);
        StatTab.SetActive(true);
        selectedStatIndex = 0;
        tempSP = playerDataHandler.playerData[currentPlayer].SkillPoints;
        Debug.Log(orgStats);
        Debug.Log(playerDataHandler.playerData[currentPlayer].Vitality);
        ArrayUtility.Clear(ref orgStats);
        ArrayUtility.Add(ref orgStats, playerDataHandler.playerData[currentPlayer].Vitality);
        ArrayUtility.Add(ref orgStats, playerDataHandler.playerData[currentPlayer].Mentality);
        ArrayUtility.Add(ref orgStats, playerDataHandler.playerData[currentPlayer].Fortitude);
        ArrayUtility.Add(ref orgStats, playerDataHandler.playerData[currentPlayer].PhysicalPower);
        ArrayUtility.Add(ref orgStats, playerDataHandler.playerData[currentPlayer].Nimbleness);
        ArrayUtility.Add(ref orgStats, playerDataHandler.playerData[currentPlayer].Brilliance);
        ArrayUtility.Add(ref orgStats, playerDataHandler.playerData[currentPlayer].Hope);
        tempStats = orgStats;
        LevelText.text = playerDataHandler.playerData[currentPlayer].Level.ToString();
        ExpText.text = playerDataHandler.playerData[currentPlayer].Exp.ToString();
        ExpToNextLevelText.text = playerDataHandler.playerData[currentPlayer].ExpToNextLevel.ToString();
        SkillPointsText.text = tempSP.ToString();
        VitalityText.text = orgStats[0].ToString();
        MentalityText.text = orgStats[1].ToString();
        FortitudeText.text = orgStats[2].ToString();
        PhysicalPowerText.text = orgStats[3].ToString();
        NimblenessText.text = orgStats[4].ToString();
        BrillianceText.text = orgStats[5].ToString();
        HopeText.text = orgStats[6].ToString();
        levelingUp = true;

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

    public void IncreseStat(int statID)
    {
        if (tempSP > 0 && tempStats[statID] < 99)
        {
            tempSP--;
            tempStats[statID]++;
            StatText[statID].GetComponent<TextMeshProUGUI>().text = tempStats[statID].ToString();
            SkillPointsText.text = tempSP.ToString();
        }

    }

    public void DecreseStat(int statID)
    {
        Debug.Log(tempStats[statID] > 0);
        Debug.Log(tempStats[statID] != orgStats[statID]);
        if (tempStats[statID] > 0 && tempStats[statID] != orgStats[statID])
        {
            tempSP++;
            tempStats[statID]--;
            StatText[statID].GetComponent<TextMeshProUGUI>().text = tempStats[statID].ToString();
            SkillPointsText.text = tempSP.ToString();
        }

    } 
}
