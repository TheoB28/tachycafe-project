using UnityEditor;
using UnityEngine;

public class PlayerDataHandler : MonoBehaviour
{

    [SerializeField] public PlayerData[] playerData;
    [SerializeField] PlayerCombat[] playerCombat;

    int tick;

    SceneLoader sceneLoader;

    

    private void Start()
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
        sceneLoader = FindAnyObjectByType<SceneLoader>();

    }

    private void FixedUpdate()
    {
        UpdateCombat();
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
        playerCombat = FindObjectsOfType<PlayerCombat>();
        //loads each player with its data
        for (int i = 0; i < playerCombat.Length; i++)
        {
            playerCombat[i].PlayerName = playerData[i].PlayerName;
            playerCombat[i].gameObject.name = playerData[i].PlayerName;
            playerCombat[i].Actions = playerData[i].Actions;
            playerCombat[i].HP = playerData[i].HP;
            playerCombat[i].FP = playerData[i].FP;
            playerCombat[i].MaxHP = playerData[i].MaxHP;
            playerCombat[i].MaxFP = playerData[i].MaxFP;
            playerCombat[i].HPText.text = playerData[i].HP.ToString();
            playerCombat[i].FPText.text = playerData[i].FP.ToString();
        }
        playerCombat = null;
    }

}
