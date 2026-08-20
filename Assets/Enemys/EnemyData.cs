using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class EnemyData : ScriptableObject
{
    [SerializeField] public Action[] Actions;
    [SerializeField] public int HP;
    [SerializeField] public int FP;
    [SerializeField] public int MaxHP;
    [SerializeField] public int MaxFP;

    CombatHandler CombatHandler;

    void Start()
    {
        CombatHandler = FindAnyObjectByType<CombatHandler>();
    }
    
    void chosen()
    {
        if (CombatHandler.ChoosingTargetEnemy)
        {

        }
    }


}
