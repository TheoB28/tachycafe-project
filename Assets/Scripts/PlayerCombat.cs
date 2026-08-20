using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public Action[] Actions;
    [SerializeField] int HP;
    [SerializeField] int FP;

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
