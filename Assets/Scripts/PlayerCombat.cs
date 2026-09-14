using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public Action[] Actions;
    [SerializeField] public int HP;
    [SerializeField] public int FP;
    [SerializeField] public int MaxHP;
    [SerializeField] public int MaxFP;
    [SerializeField] public Effects[] CurrentEffects;
    [SerializeField] public TextMeshProUGUI HPText;
    [SerializeField] public TextMeshProUGUI FPText;

    [Header("Level Stats")]
    [SerializeField] public int Vitality;
    [SerializeField] public int Mentality;
    [SerializeField] public int Fortitude;
    [SerializeField] public int PhysicalPower;
    [SerializeField] public int Nimbleness;
    [SerializeField] public int Brilliance;
    [SerializeField] public int Hope;

    [Header("Misc")]
    [SerializeField] public string PlayerName;
    [SerializeField] GameObject EffectHolder;
    [SerializeField] Vector2 EffectOffset;
    [SerializeField] int ListSize = 5;

    public bool IsDead = false;


    Canvas Canvas;
    PlayerDataHandler PlayerDataHandler;

    private void Awake()
    {
        Canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        UpdateEffects();
    }

    private void Start()
    {
        PlayerDataHandler = GetComponentInParent<PlayerDataHandler>();
        HPText.text = HP.ToString();
        FPText.text = FP.ToString();
    }

    public void UseAction(Action action, Effects[] UsersEffects, EnemyCombat enemy)
    {  
        //is called by the user to effect the player
        if(action.ActionEffect != null)
        {
            Effects effect = ScriptableObject.CreateInstance<Effects>();
            effect.copyFrom(action.ActionEffect);
            ArrayUtility.Add(ref CurrentEffects, effect);
        }

        UpdateEffects();

        float ActualDamage = action.GetDamage(enemy);
        if (CurrentEffects.Length != 0)
        {
            foreach (var effect in CurrentEffects)
            {
                
                ActualDamage = ActualDamage * effect.DamageResistanceMultiplier * effect.DamageMultiplier;

            }
        }

        if (UsersEffects.Length != 0)
        {
            foreach (var effect in UsersEffects)
            {
                ActualDamage = ActualDamage * effect.DamageMultiplier;
            }
        }

        //hurts the player and clamps the health
        HP -= (int) ActualDamage;

        if (HP <= 0)
        {
            HP = 0;
            IsDead = true;
            PlayerDataHandler.UpdateData();
        }
        HPText.text = HP.ToString();
    }

    public void UseAction(Action action, Effects[] UsersEffects, PlayerCombat Player)
    {
        //is called by the user to effect the player
        if (action.ActionEffect != null)
        {
            Effects effect = ScriptableObject.CreateInstance<Effects>();
            effect.copyFrom(action.ActionEffect);
            ArrayUtility.Add(ref CurrentEffects, effect);
        }

        UpdateEffects();

        float ActualDamage = action.GetDamage(Player);
        if (CurrentEffects.Length != 0)
        {
            foreach (var effect in CurrentEffects)
            {

                ActualDamage = ActualDamage * effect.DamageResistanceMultiplier * effect.DamageMultiplier;

            }
        }

        if (UsersEffects.Length != 0)
        {
            foreach (var effect in UsersEffects)
            {
                ActualDamage = ActualDamage * effect.DamageMultiplier;
            }
        }

        //hurts the player and clamps the health
        HP -= (int)ActualDamage;

        if (HP <= 0)
        {
            HP = 0;
            IsDead = true;
            PlayerDataHandler.UpdateData();
        }
        HPText.text = HP.ToString();
    }

    public void UseFP(int amount)
    {
        FP -= amount;
        FPText.text = FP.ToString();
    }   

    void UpdateEffects()
    {
        //places the effects by the charachter
        int rows = 1;
        Vector2 offset = Vector2.zero;
        foreach (var effect in CurrentEffects)
        {
            if((int)(offset.x/ListSize) >= rows) 
            { 
                offset.y += EffectOffset.y;
                offset.x = 0;
                rows++;
            }
            Instantiate(effect.Icon, EffectHolder.transform.position + new Vector3(offset.x, offset.y, 0), Quaternion.identity, EffectHolder.transform);
            offset.x += EffectOffset.x;
        }
    }

}
