using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Stats")]
    [SerializeField] public string PlayerName;
    [SerializeField] public Action[] Actions;
    [SerializeField] public int HP;
    [SerializeField] public int FP;
    [SerializeField] public int MaxHP;
    [SerializeField] public int MaxFP;
    [SerializeField] public Effects[] CurrentEffects;

    public bool IsDead;
    [Header("Dysphoria")]
    [SerializeField] public int Gender;
    public bool HasDysphoria;
    public float SkipChans;

    public void ChangeGenderLevel(int change)
    {
        Gender += change;
        if (Gender <= 0)
        {
            HasDysphoria = true;
            SkipChans = 1f -( 1f / (float) Mathf.Abs(Gender));
        }
        else
        {
            HasDysphoria = false;
        }
    } 
}
