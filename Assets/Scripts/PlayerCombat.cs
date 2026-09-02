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

    [Header("Misc")]
    [SerializeField] public string PlayerName;
    [SerializeField] GameObject EffectHolder;
    [SerializeField] Vector2 EffectOffset;
    [SerializeField] int ListSize = 5;

    public bool IsDead = false;


    Canvas Canvas;

    private void Awake()
    {
        Canvas = GetComponent<Canvas>();
        gameObject.name = PlayerName;
    }

    private void Update()
    {
        UpdateEffects();
    }

    public void UseAction(Action action, Effects[] UsersEffects)
    {
        

        if(action.ActionEffect != null)
        {
            Effects effect = ScriptableObject.CreateInstance<Effects>();
            effect.copyFrom(action.ActionEffect);
            ArrayUtility.Add(ref CurrentEffects, effect);
        }

        UpdateEffects();

        float ActualDamage = action.Damage;
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

        HP -= (int) ActualDamage;

        if (HP <= 0)
        {
            HP = 0;
            IsDead = true;
            Debug.Log("Player is dead");
        }

        HP += action.Heal;
        if (HP > MaxHP)
        {
            HP = MaxHP;
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
