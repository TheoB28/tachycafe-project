using UnityEngine;

[CreateAssetMenu(fileName = "Effects", menuName = "Scriptable Objects/Effects")]
public class Effects : ScriptableObject
{
    [SerializeField] public int duration;
    [SerializeField] public float DamageMultiplier;
    [SerializeField] public float DamageTakenMultiplier;
}
