using UnityEditor;
using UnityEngine;

public class PlayerDataHandler : MonoBehaviour
{

    [SerializeField] PlayerData[] playerData;
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
    }

    public void UpdateData()
    {
        for (int i = 0; i < playerCombat.Length; i++)
        {
            playerData[i].HP = playerCombat[i].HP;
            playerData[i].FP = playerCombat[i].FP;
            playerData[i].MaxHP = playerCombat[i].MaxHP;
            playerData[i].MaxFP = playerCombat[i].MaxFP;
            foreach(Effects effect in playerCombat[i].CurrentEffects)
            {
                Effects NewEffect = ScriptableObject.CreateInstance<Effects>();
                NewEffect.copyFrom(effect);
                ArrayUtility.Add(ref playerData[i].CurrentEffects, NewEffect);
                Debug.Log(NewEffect.ToString() + effect);
            }
        }
    }

    private void FixedUpdate()
    {
        TickEffect();
        UpdateData();
    }

    void TickEffect()
    {
        tick++;
        if (sceneLoader.Incombat) {  return; }
        foreach(var player in playerData)
        {
            foreach(var effect in player.CurrentEffects)
            {
                //Deals Damage if tick is divisable by frametotick 
                if (effect.ActivatesOutOfCombat && effect != null && tick % effect.framesToTick == 0)
                {
                    
                    player.HP -= effect.damage;
                    if (player.HP <= 0)
                    {
                        player.HP = 0;
                        player.IsDead = true;
                    }
                    effect.duration--;
                    if (effect.duration <= 0) { ArrayUtility.Remove(ref player.CurrentEffects, effect); }
                    Debug.Log(effect.duration + effect.name);
                }
            }
        }
    }
}
