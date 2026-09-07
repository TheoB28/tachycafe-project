using TMPro;
using UnityEngine;

public class SiteOfDisproportion : MonoBehaviour
{

    [SerializeField] Canvas canvas;
    [Header("Main Tab")]
    [SerializeField] GameObject MainTab;

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

    PlayerDataHandler playerDataHandler;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvas.gameObject.SetActive(true);
            playerDataHandler = FindAnyObjectByType<PlayerDataHandler>();
            Setup();
        }
        
    }

    void Setup()
    {
        MainTab.SetActive(true);
    }

    public void SetupPlayerStats(int playerID)
    {
        MainTab.SetActive(false);
        StatTab.SetActive(true);
        LevelText.text =playerDataHandler.playerData[playerID].Level.ToString();
        ExpText.text = playerDataHandler.playerData[playerID].Exp.ToString();
        ExpToNextLevelText.text = playerDataHandler.playerData[playerID].ExpToNextLevel.ToString();
        VitalityText.text = playerDataHandler.playerData[playerID].Vitality.ToString();
        MentalityText.text = playerDataHandler.playerData[playerID].Mentality.ToString();
        FortitudeText.text = playerDataHandler.playerData[playerID].Fortitude.ToString();
        PhysicalPowerText.text = playerDataHandler.playerData[playerID].PhysicalPower.ToString();
        NimblenessText.text = playerDataHandler.playerData[playerID].Nimbleness.ToString();
        BrillianceText.text = playerDataHandler.playerData[playerID].Brilliance.ToString();
        HopeText.text = playerDataHandler.playerData[playerID].Hope.ToString();
    }
}
