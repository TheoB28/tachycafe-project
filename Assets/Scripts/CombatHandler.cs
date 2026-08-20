using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatHandler : MonoBehaviour
{

    [Header("Fight Tab")]
    [SerializeField] GameObject FightTab;
    [SerializeField] Button action1Button;
    [SerializeField] Button action2Button;
    [SerializeField] Button action3Button;
    [SerializeField] Button action4Button;
    
    [Header("Items Tab")]
    [SerializeField] GameObject ItemsTab;

    [Header("Description Tab")]
    [SerializeField] GameObject DescriptionTab;
    [SerializeField] TextMeshProUGUI DescriptionText;

    [SerializeField] PlayerCombat player;

    private void Start()
    {
        SetupPlayerTabs();
    }

    public void ShowFightTab()
    {
        FightTab.SetActive(true);  
        DescriptionTab.SetActive(true);
        ItemsTab.SetActive(false);
    }
    public void ShowItemTab()
    {
        FightTab.SetActive(false);
        DescriptionTab.SetActive(false);
        ItemsTab.SetActive(true);
    }

    void SetupPlayerTabs()
    {
        TextMeshProUGUI ButtonText = action1Button.GetComponentInChildren<TextMeshProUGUI>();
        ButtonText.text = player.Actions[0].name;
        ButtonText = action2Button.GetComponentInChildren<TextMeshProUGUI>();
        ButtonText.text = player.Actions[1].name;
        ButtonText = action3Button.GetComponentInChildren<TextMeshProUGUI>();
        ButtonText.text = player.Actions[2].name;
        ButtonText = action4Button.GetComponentInChildren<TextMeshProUGUI>();
        ButtonText.text = player.Actions[3].name;
    }

    public void ChangeDescription(int ID)
    {
        DescriptionText.text = player.Actions[ID].Description;
        
    }
}

