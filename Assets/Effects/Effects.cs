using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Effects", menuName = "Scriptable Objects/Effects")]
public class Effects : ScriptableObject
{

    public enum ActivationType { none, preAction, postAction };

    [Header("Effects")]
    [SerializeField] public int duration;
    [SerializeField] public float DamageMultiplier;
    [SerializeField] public float DamageResistanceMultiplier;
    [SerializeField] public GameObject Icon;
    [SerializeField] public string effectLog;
    [SerializeField] public ActivationType activation;

    public bool DealsDamage;
    [SerializeField] public int damage;
    [SerializeField] public int framesToTick = 60;
    public bool ActivatesOutOfCombat;

    [Header("Dysphoria")]
    public bool isDysphoria;
    [SerializeField] public float SkipChans = 0.5f;




    public void copyFrom(Effects other)
    {
        duration = other.duration;
        DamageMultiplier = other.DamageMultiplier;
        DamageResistanceMultiplier = other.DamageResistanceMultiplier;
        Icon = other.Icon;
        effectLog = other.effectLog;
        activation = other.activation;
        name = other.name;
        isDysphoria = other.isDysphoria;
        SkipChans = other.SkipChans;
        
    }

    public string Log(GameObject entity)
    {
        return entity.name + " " + effectLog;
    }

}
