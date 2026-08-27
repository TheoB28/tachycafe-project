using System.Linq;
using TMPro;
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

    public bool IsDead = false;

    public void UseAction(Action action)
    {

        CurrentEffects.ut
        int ActualDamage = action.Damage;
        foreach (var effect in CurrentEffects)
        {
            ActualDamage = Mathf.RoundToInt(ActualDamage * effect.DamageTakenMultiplier);
        }
        HP -= ActualDamage;

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
}
