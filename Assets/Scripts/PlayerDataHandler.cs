using UnityEditor;
using UnityEngine;

public class PlayerDataHandler : MonoBehaviour
{

    [SerializeField] public PlayerData[] playerData;
    [SerializeField] public PlayerCombat[] playerCombat;

    int tick;

    SceneLoader sceneLoader;
    CombatHandler combatHandler;


    private void Awake()
    {
        
        int playerDataHolderCount = FindObjectsOfType<PlayerDataHandler>().Length;
        if (playerDataHolderCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        
    }

    private void Start()
    {
        sceneLoader = FindAnyObjectByType<SceneLoader>();
    }


    public void UpdateData()
    {
        
        int i = 0;
        foreach (PlayerCombat player in playerCombat) 
        {
            playerData[i].HP = player.HP;
            playerData[i].FP = player.FP;
            playerData[i].MaxHP = player.MaxHP;
            playerData[i].MaxFP = player.MaxFP;
            i++;
        }
    }

    public void UpdateCombat()
    {
        //loads each player with its data
        int i = 0;
        foreach (PlayerCombat player in playerCombat)
        {
            player.PlayerName = playerData[i].PlayerName;
            player.gameObject.name = playerData[i].PlayerName;
            player.Actions = playerData[i].Actions;
            player.HP = playerData[i].HP;
            player.FP = playerData[i].FP;
            player.MaxHP = playerData[i].MaxHP;
            player.MaxFP = playerData[i].MaxFP;
            player.HPText.text = playerData[i].HP.ToString();
            player.FPText.text = playerData[i].FP.ToString();
            i++;
        }
    }

    public void DeactivateCombat()
    {
        foreach (PlayerCombat player in playerCombat)
        {
            if (player == null) { return; }
            player.gameObject.SetActive(false);
        }
    }

    public void ActivateCombat()
    {

        foreach (PlayerCombat player in playerCombat)
        {
            if (player == null) { return; }
            player.gameObject.SetActive(true);
            UpdateCombat();
        }
        combatHandler = FindAnyObjectByType<CombatHandler>();
        combatHandler.SetupPlayers(playerCombat);
    }
}
