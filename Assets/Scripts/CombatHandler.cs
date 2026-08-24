using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CombatHandler : MonoBehaviour
{

    [Header("Fight Tab")]
    [SerializeField] Button FightButton;
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

    [Header("Turn")]
    [SerializeField] GameObject Selector;
    [SerializeField] float SelectorHeight;
    int CurrentTargetID;


    public bool ChoosingTarget;
    public bool PlayerTurn;

    EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = GetComponentInChildren<EventSystem>();
    }


    private void Start()
    {
        SetupPlayerTabs();
        FightButton.Select();

    }

    public void OnNavigate(InputValue Input)
    {
        if (ChoosingTarget)
        {
            switch (players[CurrentCharacterID].Actions[CurrentActionID].Target) 
            {
                case Action.PossibleTarget.enemy:
                    if (Input.Get<Vector2>().x > 0)
                    {
                        if (CurrentTargetID >= Enemies.Length - 1) { return; }
                        Selector.transform.position = Enemies[CurrentTargetID + 1].transform.position + new Vector3(0, SelectorHeight, 0);
                        CurrentTargetID++;
                    }
                    else if (Input.Get<Vector2>().x < 0)
                    {
                        if (CurrentTargetID == 0) { return; }
                        Selector.transform.position = Enemies[CurrentTargetID - 1].transform.position + new Vector3(0, SelectorHeight, 0);
                        CurrentTargetID--;
                    }
                    break;
                case Action.PossibleTarget.ally:
                    if (Input.Get<Vector2>().x > 0)
                    {
                        if (CurrentTargetID == 0) { return; }
                        Selector.transform.position = players[CurrentTargetID - 1].transform.position + new Vector3(0, SelectorHeight, 0);
                        CurrentTargetID--;
                    }
                    else if (Input.Get<Vector2>().x < 0)
                    {
                        if (CurrentTargetID >= players.Length - 1) { return; }
                        Selector.transform.position = players[CurrentTargetID + 1].transform.position + new Vector3(0, SelectorHeight, 0);
                        CurrentTargetID++;
                    }
                    break;
            }
        }
    }

    public void OnCancel()
    {
        if (ChoosingTarget)
        {
            action1Button.Select();
            ChoosingTarget = false;
            Selector.SetActive(false);
        }
        else
        {
            FightButton.Select();
            FightTab.SetActive(false);
            DescriptionTab.SetActive(false);
        }
    }

    public void OnSubmit()
    {
        Debug.Log("sub");
    }
     
    public void ShowFightTab()
    {
        FightTab.SetActive(true);  
        DescriptionTab.SetActive(true);
        ItemsTab.SetActive(false);
        action1Button.Select();
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
            SelectTarget();
            eventSystem.SetSelectedGameObject(null);
            Selector.SetActive(true);
        }
    }

    public void SelectTarget()
    {

        switch (players[CurrentCharacterID].Actions[CurrentActionID].Target)
        {
            case Action.PossibleTarget.enemy:
                Selector.transform.position = Enemies[0].transform.position + new Vector3(0, SelectorHeight, 0);
                break;
            case Action.PossibleTarget.ally:
                Selector.transform.position = players[0].transform.position + new Vector3(0, SelectorHeight, 0);
                break;
            case Action.PossibleTarget.self:
                Selector.transform.position = players[CurrentCharacterID].transform.position + new Vector3(0, SelectorHeight, 0);
                break;
        }
        Selector.SetActive(false);
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
        if (CurrentCharacterID >= players.Length)
        {
            PlayerTurn = false;
        }
    }
 
}

