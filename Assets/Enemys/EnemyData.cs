using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class EnemyData : ScriptableObject
{
    public enum EnemyBehavior { Aggressive, Defensive, Supportive }

    [SerializeField] public Action[] Actions;
    [SerializeField] public int HP;
    [SerializeField] public int FP;
    [SerializeField] public int MaxHP;
    [SerializeField] public int MaxFP;
    [SerializeField] public EnemyBehavior Behavior;

    CombatHandler CombatHandler;

    void Start()
    {
        CombatHandler = FindAnyObjectByType<CombatHandler>();
    }
}
