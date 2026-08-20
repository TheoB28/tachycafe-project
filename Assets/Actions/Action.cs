using UnityEngine;

[CreateAssetMenu(fileName = "Actions", menuName = "Scriptable Objects/Actions")]
public class Action : ScriptableObject
{
    public enum PossibleTarget  { self, ally, enemy }

    [SerializeField] public int Damage;
    [SerializeField] public int Heal;
    [SerializeField] public PossibleTarget Target;
    [SerializeField] public string Description;
}
