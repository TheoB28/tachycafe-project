using UnityEngine;

[CreateAssetMenu(fileName = "Actions", menuName = "Scriptable Objects/Actions")]
public class Action : ScriptableObject
{
    enum TargetsEnum  { self, ally, enemy }

    [SerializeField] int Damage;
    [SerializeField] int Heal;
    [SerializeField] TargetsEnum Targets;
    [SerializeField] string Description;
}
