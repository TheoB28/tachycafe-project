using UnityEngine;

public class PlayerDataHolder : MonoBehaviour
{

    [SerializeField] PlayerData[] playerData;
    [SerializeField] PlayerCombat[] playerCombat;

    private void Start()
    {
        int playerDataHolderCount = FindObjectsOfType<PlayerDataHolder>().Length;
        if (playerDataHolderCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        playerCombat = FindObjectsOfType<PlayerCombat>();
        for (int i = 0; i < playerCombat.Length; i++)
        {
            playerCombat[i].Actions = playerData[i].Actions;
            playerCombat[i].HP = playerData[i].HP;
            playerCombat[i].FP = playerData[i].FP;
            playerCombat[i].MaxHP = playerData[i].MaxHP;
            playerCombat[i].MaxFP = playerData[i].MaxFP;
            playerCombat[i].HPText.text = playerData[i].HP.ToString();
            playerCombat[i].FPText.text = playerData[i].FP.ToString();
        }
    }
}
