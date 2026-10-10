using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CombatHandler : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] TextMeshProUGUI PlayerNameText;
    [SerializeField] TextMeshProUGUI CombatLogText;


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
    [SerializeField] Effects toutched;

    [Header("Enemys")]
    [SerializeField] public EnemyCombat[] Enemies;

    [Header("Turn")]
    [SerializeField] GameObject Selector;
    [SerializeField] float SelectorHeight;
    int CurrentTargetID;

    public int ExpReward;
    public bool ChoosingTarget;
    public bool PlayerTurn;
    public bool Teamworking;
    public bool StopNav;
    bool HasWon;

    PlayerDataHandler PlayerDataHandler;
    EventSystem eventSystem;
    SceneLoader SceneLoader;

    private void Awake()
    {
        eventSystem = GetComponentInChildren<EventSystem>();
    }


    private void Start()
    {

    }


    public void OnNavigate(InputValue Input)
    {
        if (StopNav) { return; }
        TargetSelecting(Input);
    }

    public void OnSubmit()
    {
        if (StopNav) { return; }
        submitAction();
    }

    public void OnCancel()
    {
        if (StopNav) { return; }
        if (ChoosingTarget && Teamworking)
        {
            FightButton.Select();
            Teamworking = false;
            ChoosingTarget = false;
            Selector.SetActive(false);
        }
        else if (ChoosingTarget)
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

    public void Teamwork()
    {
        Teamworking = true;
        StartChosing();
    }

    public void ShowItemTab()
    {
        FightTab.SetActive(false);
        DescriptionTab.SetActive(false);
        ItemsTab.SetActive(true);
    }

    public void SetupPlayers(PlayerCombat[] players)
    {
        PlayerDataHandler = FindAnyObjectByType<PlayerDataHandler>();
        SceneLoader = FindObjectOfType<SceneLoader>();
        this.players = players;
        SetupPlayerTabs();
        FightButton.Select();
        PlayerTurn = true;
        PlayerNameText.text = players[CurrentCharacterID].PlayerName;
    }

    public void SetupEnemies(EnemyData[] enemies)
    {
        //adds the data to the enemys and removes the vessels without data
        EnemyCombat[] NewEnemies = new EnemyCombat[0];
        if(enemies == null) { return; }
        int i = 0;
        foreach (EnemyData enemy in enemies)
        {
            Enemies[i].LoadData(enemies[i]);
            ArrayUtility.Add(ref NewEnemies, Enemies[i]);
            ExpReward += enemies[i].XPReward;
            i++;
        }
        foreach (EnemyCombat enemy in Enemies)
        {
            if (enemy.Data == null)
            {
                Destroy(enemy.gameObject);
            }
        }
        Enemies = NewEnemies;
        
    }

    void SetupPlayerTabs()
    {
        //sets up the UI or the current character
        TextMeshProUGUI ButtonText = action1Button.GetComponentInChildren<TextMeshProUGUI>();
        ButtonText.text = players[CurrentCharacterID].Actions[0].name;
        ButtonText = action2Button.GetComponentInChildren<TextMeshProUGUI>();
        ButtonText.text = players[CurrentCharacterID].Actions[1].name;
        ButtonText = action3Button.GetComponentInChildren<TextMeshProUGUI>();
        ButtonText.text = players[CurrentCharacterID].Actions[2].name;
        ButtonText = action4Button.GetComponentInChildren<TextMeshProUGUI>();
        ButtonText.text = players[CurrentCharacterID].Actions[3].name;
        PlayerNameText.text = players[CurrentCharacterID].PlayerName;
    }

    public void ChangeDescription(int ID)
    {
        //Take a wild guess
        DescriptionText.text = players[CurrentCharacterID].Actions[ID].Description;
    }

    public void StartChosing()
    {
        //starts the choosing off targets
        if (PlayerTurn && Teamworking)
        {
            ChoosingTarget = true;
            PlaceSelector();
            eventSystem.SetSelectedGameObject(null);
            Selector.SetActive(true);
        }
    }

    public void StartChosing(int ID)
    {
        //starts the choosing off targets
        if (PlayerTurn && players[CurrentCharacterID].FP >= players[CurrentCharacterID].Actions[ID].FPCost)
        {
            CurrentActionID = ID;
            ChoosingTarget = true;
            PlaceSelector();
            eventSystem.SetSelectedGameObject(null);
            Selector.SetActive(true);
            CurrentTargetID = 0;
        }
    }

    public void PlaceSelector()
    {
        //activates the selector and puts it on the first possible target
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
        if (Teamworking) { Selector.transform.position = players[0].transform.position + new Vector3(0, SelectorHeight, 0); }
        Selector.SetActive(false);
    }

    void submitAction()
    {
        //activates the action
        if (ChoosingTarget && !Teamworking)
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
        }
        else if (ChoosingTarget && Teamworking)
        {
            if(CurrentTargetID == CurrentCharacterID) { return; }
            players[CurrentTargetID].Heal(players[CurrentCharacterID].FriendLevel[CurrentTargetID]);
            Effects effect = ScriptableObject.CreateInstance<Effects>();
            effect.copyFrom(toutched);
            ArrayUtility.Add(ref players[CurrentTargetID].CurrentEffects, effect);
            ChoosingTarget = false;
            Teamworking = false;
            combatLog(players[CurrentCharacterID].name + " hugs " + players[CurrentTargetID].name);
            NextPlayerTurn();

        }
    }

    void TargetSelecting(InputValue Input)
    {
        
        //moves the selector and selects the target
        if (ChoosingTarget && !Teamworking)
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
                        if (CurrentTargetID < 0) { return; }
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
        else if(ChoosingTarget && Teamworking)
        {
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
        }
    }

    public void ActivateAction(EnemyCombat Targget)
    {
        //activats the action on the chosen enemy
        Targget.UseAction(players[CurrentCharacterID].Actions[CurrentActionID], players[CurrentCharacterID].CurrentEffects, players[CurrentCharacterID]);
        ChoosingTarget = false;
        players[CurrentCharacterID].UseFP(players[CurrentCharacterID].Actions[CurrentActionID].FPCost);
        combatLog(players[CurrentCharacterID].name + " uses " + players[CurrentCharacterID].Actions[CurrentActionID].name + " on " + Targget.name);
        NextPlayerTurn();

    }

    public void ActivateAction(PlayerCombat Tarrget)
    {
        //activats the action on the chosen ally/self
        if (players[CurrentCharacterID].Actions[CurrentActionID].Target == Action.PossibleTarget.self && players[CurrentCharacterID].gameObject == Tarrget.gameObject)
        {
            Tarrget.UseAction(players[CurrentCharacterID].Actions[CurrentActionID], players[CurrentCharacterID].CurrentEffects, players[CurrentCharacterID]);
        }
        else if (players[CurrentCharacterID].Actions[CurrentActionID].Target == Action.PossibleTarget.ally && players[CurrentCharacterID].gameObject != Tarrget)
        {
            Tarrget.UseAction(players[CurrentCharacterID].Actions[CurrentActionID], players[CurrentCharacterID].CurrentEffects, players[CurrentCharacterID]);
        }
        ChoosingTarget = false;
        players[CurrentCharacterID].UseFP(players[CurrentCharacterID].Actions[CurrentActionID].FPCost);
        combatLog(players[CurrentCharacterID].name + " uses " + players[CurrentCharacterID].Actions[CurrentActionID].name + " on " + players[CurrentCharacterID].name);
        NextPlayerTurn();

    }

    IEnumerator StartEnemyTurn()
    {
        //checks win
        if (Enemies.Length == 0)
        {
            StopCoroutine(StartEnemyTurn());
        }
        //activates the enemys turns
        foreach (EnemyCombat enemy in Enemies)
        {
            foreach (var effect in enemy.CurrentEffects)
            {
                effect.duration--;
                if (effect.duration <= 0) { ArrayUtility.Remove(ref enemy.CurrentEffects, effect); }
            }
            yield return new WaitForSeconds(1f);
            enemy.UseTurn(players, Enemies);
            yield return new WaitForSeconds(1f);
        }
        StartPlayerTurn();
    }

    void EffectActivationPreAction()
    {
        //activates effects that happen before the players action
        if (players[CurrentCharacterID].IsDead) { NextPlayerTurn(); }
        foreach (Effects effect in players[CurrentCharacterID].CurrentEffects)
        {
            if(effect.activation == Effects.ActivationType.preAction)
            {
                combatLog(effect.Log(players[CurrentCharacterID].gameObject));
 
            }
            
        }
        float i = UnityEngine.Random.value;
        if (PlayerDataHandler.playerData[CurrentCharacterID].HasDysphoria && i < PlayerDataHandler.playerData[CurrentCharacterID].SkipChans)
        {
            CombatLogText.text = players[CurrentCharacterID].name + " does nothing due to dysphoria.";
            //NextPlayerTurn();
        }

    }

    void StartPlayerTurn()
    {
        //ticks down effects and restarts round
        foreach(var player in players)
        {
            foreach(var effect in player.CurrentEffects)
            {
                effect.duration--;
                if (effect.duration <= 0) { ArrayUtility.Remove(ref player.CurrentEffects, effect); }
            }
        }
        CurrentCharacterID = 0;
        PlayerTurn = true;
        SetupPlayerTabs();
        FightButton.Select();
        EffectActivationPreAction();
    }

    void NextPlayerTurn()
    {
        Selector.SetActive(false);
        CurrentCharacterID++;
        FightTab.SetActive(false);
        DescriptionTab.SetActive(false);
        Debug.Log("Next Player Turn: " + CurrentCharacterID);
        if (CurrentCharacterID >= players.Length)
        {
            if (Enemies.Length == 0)
            {
                return;
            }
            PlayerTurn = false;
            StartCoroutine(StartEnemyTurn());
        }
        else
        {
            EffectActivationPreAction();
            SetupPlayerTabs();
            FightButton.Select();
        }
    }

    public void combatLog(string text)
    {
        CombatLogText.text += "\n" + text;
    }

    public void EnemyDeath(EnemyCombat enemy)
    {
        //takes the enemy out off the arrray an checks win
        ArrayUtility.Remove(ref Enemies, enemy);
        if (Enemies.Length == 0)
        {

            EndCombat();
        }
    }

    void EndCombat()
    {
        PlayerDataHandler.giveExp(ExpReward);
        HasWon = true;
        //PlayerDataHandler.UpdateData();
        SceneLoader.LoadOverworld();
    }

}

