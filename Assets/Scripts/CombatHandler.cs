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

    [Header("Players")]
    [SerializeField] PlayerCombat[] players;
    [SerializeField] int CurrentActionID;
    public int CurrentCharacterID;
    [SerializeField] Material OutlineMaterial;
    [SerializeField] Material DefaultMaterial;

    [Header("Enemys")]
    [SerializeField] EnemyCombat[] Enemies;

    public bool ChoosingTarget;
    public bool PlayerTurn;

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
        ButtonText.text = players[CurrentCharacterID].Actions[0].name;
        ButtonText = action2Button.GetComponentInChildren<TextMeshProUGUI>();
        ButtonText.text = players[CurrentCharacterID].Actions[1].name;
        ButtonText = action3Button.GetComponentInChildren<TextMeshProUGUI>();
        ButtonText.text = players[CurrentCharacterID].Actions[2].name;
        ButtonText = action4Button.GetComponentInChildren<TextMeshProUGUI>();
        ButtonText.text = players[CurrentCharacterID].Actions[3].name;
    }

    public void ChangeDescription(int ID)
    {
        DescriptionText.text = players[CurrentCharacterID].Actions[ID].Description;
        
    }

    public void UseAction(int ID)
    {
        if (!PlayerTurn)
        {
            CurrentActionID = ID;
            ChoosingTarget = true;
            switch (ID)
            {
                case 0:
                    action1Button.GetComponent<Outline>().enabled = true;
                    break;
                case 1:
                    action2Button.GetComponent<Outline>().enabled = true;
                    break;
                case 2:
                    action3Button.GetComponent<Outline>().enabled = true;
                    break;
                case 3:
                    action4Button.GetComponent<Outline>().enabled = true;
                    break;
            }
        }
    }

    public void ActivateAction(EnemyCombat Targget)
    {
        Targget.TakeDamage(players[CurrentCharacterID].Actions[CurrentActionID].Damage);
        ChoosingTarget = false;
        CurrentCharacterID++;
    }

    public void ActivateAction(PlayerCombat Tarrget)
    {
        if (players[CurrentCharacterID].Actions[CurrentActionID].Target == Action.PossibleTarget.self && players[CurrentCharacterID].gameObject == Tarrget)
        {
            Tarrget.HealHP(players[CurrentCharacterID].Actions[CurrentActionID].Heal);
        }
        else if (players[CurrentCharacterID].Actions[CurrentActionID].Target == Action.PossibleTarget.ally && players[CurrentCharacterID].gameObject != Tarrget)
        {
            Tarrget.HealHP(players[CurrentCharacterID].Actions[CurrentActionID].Heal);
        }
        ChoosingTarget = false;
        CurrentCharacterID++;
        action1Button.GetComponent<Outline>().enabled = false;
        action2Button.GetComponent<Outline>().enabled = false;
        action3Button.GetComponent<Outline>().enabled = false;
        action4Button.GetComponent<Outline>().enabled = false;
        if (CurrentCharacterID >= players.Length)
        {
            PlayerTurn = false;
        }
    }

    public void HighLight(EnemyCombat enemy)
    {
        if (ChoosingTarget && players[CurrentCharacterID].Actions[CurrentActionID].Target == Action.PossibleTarget.enemy)
        {
            enemy.GetComponent<Renderer>().material = OutlineMaterial;
        }
    }

    public void HighLight(PlayerCombat player)
    {
        if (ChoosingTarget && players[CurrentCharacterID].Actions[CurrentActionID].Target == Action.PossibleTarget.ally)
        {
            player.GetComponent<Renderer>().material = OutlineMaterial;
        }
    }

    public void RemoveHighLight(EnemyCombat enemy)
    {
        if (ChoosingTarget && players[CurrentCharacterID].Actions[CurrentActionID].Target == Action.PossibleTarget.enemy)
        {
            enemy.GetComponent<Renderer>().material = DefaultMaterial;
        }
    }
    public void RemoveHighLight(PlayerCombat player)
    {
        if (ChoosingTarget && players[CurrentCharacterID].Actions[CurrentActionID].Target == Action.PossibleTarget.ally)
        {
            player.GetComponent<Renderer>().material = DefaultMaterial;
        }
    }   
}

