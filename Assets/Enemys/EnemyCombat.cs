using TMPro;
using UnityEditor;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    public EnemyData Data;

    [SerializeField] Action[] Actions;
    [SerializeField] int HP;
    [SerializeField] int FP;
    [SerializeField] int MaxHP;
    [SerializeField] int MaxFP;
    [SerializeField] EnemyData.EnemyBehavior Behaviour;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] public Effects[] CurrentEffects;

    [SerializeField] CombatHandler CombatHandler;

    public void LoadData(EnemyData data)
    {
        Data = data;
        HP = Data.HP; FP = Data.FP; MaxHP = Data.MaxHP; MaxFP = Data.MaxFP; Behaviour = Data.Behavior; Actions = Data.Actions;
        text.text = HP.ToString();
    }
    public void UseAction(Action action, Effects[] PlayerEffects)
    {
        if (action.ActionEffect != null)
        {
            Effects effect = action.ActionEffect;
            ArrayUtility.Add(ref CurrentEffects, effect);
        }

        float ActualDamage = action.Damage;
        if (CurrentEffects.Length != 0)
        {
            foreach (var effect in CurrentEffects)
            {

                ActualDamage = ActualDamage * effect.DamageResistanceMultiplier;

            }

        }
        if (PlayerEffects.Length != 0)
        {
            foreach (var effect in PlayerEffects)
            {
                ActualDamage = ActualDamage * effect.DamageMultiplier;
            }
        }
        HP -= (int)ActualDamage;

        if (HP <= 0)
        {
            HP = 0;
            CombatHandler.EnemyDeath(this);
            Destroy(gameObject);
        }

        HP += action.Heal;
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
        text.text = HP.ToString();
    }

    public void UseTurn(PlayerCombat[] players , EnemyCombat[] enemies)
    {
        foreach(var effect in CurrentEffects)
        {
            effect.duration--;
            if(effect.duration <= 0)
            {
                CurrentEffects = System.Array.FindAll(CurrentEffects, e => e != effect);
            }
        }
        Action ChosenAction = Actions[0];
        PlayerCombat TargetPlayer = players[0];
        EnemyCombat TargetEnemy = enemies[0];
        switch (Behaviour)
        {
            case EnemyData.EnemyBehavior.Aggressive:

                foreach (var action in Actions)
                {
                    if(action.Damage > ChosenAction.Damage && action.FPCost <= FP)
                    {
                        ChosenAction = action;
                    }
                }
                foreach (var player in players)
                {
                    if(player.HP > TargetPlayer.HP)
                    {
                        TargetPlayer = player;
                    }
                }
                

                break;
            case EnemyData.EnemyBehavior.Defensive:
                TargetPlayer = players[Random.Range(0, players.Length)];
                if (Random.Range(0, 2) == 0)
                {
                    foreach (var action in Actions)
                    {
                        if (action.ActionEffect.DamageResistanceMultiplier <= ChosenAction.ActionEffect.DamageResistanceMultiplier)
                        {
                            ChosenAction = action;
                        }
                    }
                }
                else
                {
                    foreach (var action in Actions)
                    {
                        if(action.Damage > ChosenAction.Damage && action.FPCost <= FP)
                        {
                            ChosenAction = action;
                        }
                    }
                    TargetPlayer.UseAction(ChosenAction, CurrentEffects);
                }
                break;
            case EnemyData.EnemyBehavior.Supportive:
                
                foreach(var enemy in enemies)
                {
                    if(enemy.HP < TargetEnemy.HP && enemy.HP < enemy.HP / 2)
                    {
                        TargetEnemy = enemy;
                    }
                }
                if(TargetEnemy.HP < TargetEnemy.MaxHP / 2)
                {
                    foreach (var action in Actions)
                    {
                        if (action.Heal > ChosenAction.Heal)
                        {
                            ChosenAction = action;
                        }
                    }
                }
                else if (Random.Range(0, 2) == 0)
                {
                    foreach (var action in Actions)
                    {
                        if (action.ActionEffect.DamageResistanceMultiplier <= ChosenAction.ActionEffect.DamageResistanceMultiplier && action.FPCost <= FP)
                        {
                            ChosenAction = action;
                        }
                    }
                }
                else
                {
                    foreach (var action in Actions)
                    {
                        if (action.Damage > ChosenAction.Damage)
                        {
                            ChosenAction = action;
                        }
                    }
                    TargetPlayer.UseAction(ChosenAction, CurrentEffects);
                }
                break;
        }
        if (ChosenAction.FPCost > FP)
        {
            foreach (var action in Actions)
            {
                if (action.FPCost <= FP)
                {
                    ChosenAction = action;
                }
            }
        }



        switch (ChosenAction.Target)
        {
            case Action.PossibleTarget.self:
                UseAction(ChosenAction, CurrentEffects);

                break;
            case Action.PossibleTarget.ally:
                TargetEnemy.UseAction(ChosenAction, CurrentEffects);
                break;
            case Action.PossibleTarget.enemy:
                TargetPlayer.UseAction(ChosenAction, CurrentEffects);
                Debug.Log($"{gameObject.name} uses {ChosenAction.name} on {TargetPlayer.gameObject.name}");
                break;
        }
    }
}
