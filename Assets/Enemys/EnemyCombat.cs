using TMPro;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    [SerializeField] EnemyData Data;

    [SerializeField] Action[] Actions;
    [SerializeField] int HP;
    [SerializeField] int FP;
    [SerializeField] int MaxHP;
    [SerializeField] int MaxFP;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] CombatHandler CombatHandler;

    private void Awake()
    {
        HP = Data.HP; FP = Data.FP; MaxHP = Data.MaxHP; MaxFP = Data.MaxFP;
        text.text = HP.ToString();
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        
        if (HP <= 0)
        {
            HP = 0;
            CombatHandler.EnemyDeath(this);
            Destroy(gameObject);
        }
        text.text = HP.ToString();
    }
}
