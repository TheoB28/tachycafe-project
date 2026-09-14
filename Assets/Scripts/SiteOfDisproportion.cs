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

    public bool levelingUp, InMainTab;
    int selectedStatIndex, currentPlayer, tempSP, currentSelect;
    int[] orgStats = new int[0];
    int[] tempStats;

    PlayerOverworld player;
    PlayerDataHandler playerDataHandler;
    EventSystem eventSystem;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            player = other.GetComponent<PlayerOverworld>();
            player.currentSite = this;
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
            player.InMenu = true;
            InMainTab = true;
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
        InMainTab = false;
        currentSelect = 0;

        VitalityText.GetComponent<Selectable>().Select();
        selector.transform.position = VitalityText.transform.position;
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
        if (tempStats[statID] > 0 && tempStats[statID] == orgStats[statID])
        {
            tempSP++;
            tempStats[statID]--;
            StatText[statID].GetComponent<TextMeshProUGUI>().text = tempStats[statID].ToString();
            SkillPointsText.text = tempSP.ToString();
        }
    }

    public void comfirm()
    {
        playerDataHandler.playerData[currentPlayer].Vitality = tempStats[0];
        playerDataHandler.playerData[currentPlayer].Mentality = tempStats[1];
        playerDataHandler.playerData[currentPlayer].Fortitude = tempStats[2];
        playerDataHandler.playerData[currentPlayer].PhysicalPower = tempStats[3];
        playerDataHandler.playerData[currentPlayer].Nimbleness = tempStats[4];
        playerDataHandler.playerData[currentPlayer].Brilliance = tempStats[5];
        playerDataHandler.playerData[currentPlayer].Hope = tempStats[6];
        playerDataHandler.playerData[currentPlayer].SkillPoints = tempSP;
        ExitLeveling();
    }

    public void OnPlayerCancel()
    {
        if (levelingUp)
        {
            ExitLeveling();
        }
        else if (InMainTab)
        {
            canvas.gameObject.SetActive(false);
            InMainTab = false;
            player.InMenu = false;
            playerDataHandler.UpdateData();
        }
    }

    public void OnPlayerMove(InputValue input)
    {
  
        if (input.Get<Vector2>().x > 0 && currentSelect != StatText.Length)
        {
            IncreseStat(currentSelect);
        }
        else if(input.Get<Vector2>().x < 0 && currentSelect != StatText.Length)
        {
            DecreseStat(currentSelect);
        }
        if (input.Get<Vector2>().y > 0 && currentSelect > 0)
        {
            selector.SetActive(true);
            currentSelect--;
            selector.transform.position = StatText[currentSelect].transform.position;
        }
        else if (input.Get<Vector2>().y < 0 && currentSelect < StatText.Length -1)
        {
            selector.SetActive(true);

            currentSelect++;
            selector.transform.position = StatText[currentSelect].transform.position;
        } 
        else if (input.Get<Vector2>().y < 0 && currentSelect == StatText.Length - 1)
        {
            selector.SetActive(false);
            currentSelect++;
        }     
    }

    void ExitLeveling()
    {
        MainTab.SetActive(true);
        StatTab.SetActive(false);
        InMainTab = true;
        levelingUp = false;
        PlayerButtons[0].Select();
        playerDataHandler.UpdateData();
    }
}
