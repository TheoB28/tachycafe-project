using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Effects", menuName = "Scriptable Objects/Effects")]
public class Effects : ScriptableObject
{
    [SerializeField] public int duration;
    [SerializeField] public float DamageMultiplier;
    [SerializeField] public float DamageResistanceMultiplier;
    [SerializeField] public GameObject Icon;

    public void copyFrom(Effects other)
    {
        duration = other.duration;
        DamageMultiplier = other.DamageMultiplier;
        DamageResistanceMultiplier = other.DamageResistanceMultiplier;
        Icon = other.Icon;
    }
}
