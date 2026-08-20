using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    [SerializeField] EnemyData Data;

    [SerializeField] Action[] Actions;
    [SerializeField] int HP;
    [SerializeField] int FP;
    [SerializeField] int MaxHP;
    [SerializeField] int MaxFP;

    private void Awake()
    {
        HP = Data.HP; FP = Data.FP; MaxHP = Data.MaxHP; MaxFP = Data.MaxFP;
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            Debug.Log("Dead");
        }
    }
}
