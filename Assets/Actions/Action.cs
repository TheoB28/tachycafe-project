using UnityEngine;

[CreateAssetMenu(fileName = "Actions", menuName = "Scriptable Objects/Actions")]
public class Action : ScriptableObject
{
    public enum PossibleTarget  { self, ally, enemy }

    [SerializeField] public int Damage;
    [SerializeField] public int Heal;
    [SerializeField] public int FPCost;
    [SerializeField] public int Duration;
    [SerializeField] public PossibleTarget Target;
    [SerializeField] public Effects ActionEffect;
    [SerializeField] public string Description;
}
