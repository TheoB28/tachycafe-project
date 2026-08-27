using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] public EnemyCombat[] Enemies;

    [Header("Turn")]
    [SerializeField] GameObject Selector;
    [SerializeField] float SelectorHeight;
    int CurrentTargetID;


    public bool ChoosingTarget;
    public bool PlayerTurn;
    bool HasWon;

    PlayerDataHolder PlayerDataHolder;
    EventSystem eventSystem;
    SceneLoader SceneLoader;

    private void Awake()
    {
        eventSystem = GetComponentInChildren<EventSystem>();
    }


    private void Start()
    {
        PlayerDataHolder = FindAnyObjectByType<PlayerDataHolder>();
        SceneLoader = FindObjectOfType<SceneLoader>();
        SetupPlayerTabs();
        FightButton.Select();
        PlayerTurn = true;
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

    public void OnSubmit()
    {
        if (ChoosingTarget)
        {
            switch (players[CurrentCharacterID].Actions[CurrentActionID].Target)
            {
                case Action.PossibleTarget.enemy:
                    ActivateAction(Enemies[CurrentTargetID]);
                    break;
                case Action.PossibleTarget.ally:
                    ActivateAction(players[CurrentTargetID]);
                    break;
                case Action.PossibleTarget.self:
                    ActivateAction(players[CurrentCharacterID]);
                    break;
            }
            FightTab.SetActive(false);
            if (PlayerTurn)
            {
                SetupPlayerTabs();
                FightButton.Select();
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

    public void SetupEnemies(EnemyData[] enemies)
    {
        if (!HasWon)
        {
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (i < enemies.Length)
                { Enemies[i].LoadData(enemies[i]); }
                else
                {
                    Destroy(Enemies[i].gameObject);

                }
            }
            EnemyCombat[] NewEnemies = new EnemyCombat[enemies.Length];
            for (int i = 0; i < enemies.Length; i++)
            {
                NewEnemies[i] = Enemies[i];
            }
            Enemies = NewEnemies;
        }
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
        if (PlayerTurn && players[CurrentCharacterID].FP >= players[CurrentCharacterID].Actions[ID].FPCost)
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
        Targget.UseAction(players[CurrentCharacterID].Actions[CurrentActionID], players[CurrentCharacterID].CurrentEffects);
        ChoosingTarget = false;
        players[CurrentCharacterID].UseFP(players[CurrentCharacterID].Actions[CurrentActionID].FPCost);
        CurrentCharacterID++;
        Selector.SetActive(false);
        if (CurrentCharacterID >= players.Length)
        {
            PlayerTurn = false;
            StartCoroutine(StartEnemyTurn());
        }
    }

    public void ActivateAction(PlayerCombat Tarrget)
    {
        if (players[CurrentCharacterID].Actions[CurrentActionID].Target == Action.PossibleTarget.self && players[CurrentCharacterID].gameObject == Tarrget.gameObject)
        {
            Tarrget.UseAction(players[CurrentCharacterID].Actions[CurrentActionID]);
        }
        else if (players[CurrentCharacterID].Actions[CurrentActionID].Target == Action.PossibleTarget.ally && players[CurrentCharacterID].gameObject != Tarrget)
        {
            Tarrget.UseAction(players[CurrentCharacterID].Actions[CurrentActionID]);
        }
        ChoosingTarget = false;
        players[CurrentCharacterID].UseFP(players[CurrentCharacterID].Actions[CurrentActionID].FPCost);
        CurrentCharacterID++;
        Selector.SetActive(false);
        if (CurrentCharacterID >= players.Length)
        {
            PlayerTurn = false;
            StartCoroutine(StartEnemyTurn());
        }
    }

    IEnumerator StartEnemyTurn()
    {
        if (Enemies.Length == 0)
        {
            SceneLoader.LoadOverworld();
        }
        foreach (EnemyCombat enemy in Enemies)
        {
            yield return new WaitForSeconds(1f);
            enemy.UseTurn(players, Enemies);
            yield return new WaitForSeconds(1f);
        }
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        CurrentCharacterID = 0;
        PlayerTurn = true;
        SetupPlayerTabs();
        FightButton.Select();
    }

    public void EnemyDeath(EnemyCombat enemy)
    {
        Enemies = Enemies.Where(e => e != enemy).ToArray();
        if (Enemies.Length == 0)
        {
            HasWon = true;
            PlayerDataHolder.UpdateData();
            SceneLoader.LoadOverworld();
        }
    }
 
}

