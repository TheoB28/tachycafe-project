using UnityEngine;

[CreateAssetMenu(fileName = "Actions", menuName = "Scriptable Objects/Actions")]
public class Action : ScriptableObject
{
    public enum PossibleTarget  { self, ally, enemy }

    [SerializeField] public int EnemyDamage;
    [SerializeField] public int Heal;
    [SerializeField] public int FPCost;
    [SerializeField] public int Duration;
    [SerializeField] public PossibleTarget Target;
    [SerializeField] public Effects ActionEffect;
    [SerializeField] public string Description;

    [Header("Scaling")]
    [SerializeField] public float VitalityScale = 1;
    [SerializeField] public float MentalityScale = 1;
    [SerializeField] public float FortitudeScale = 1;
    [SerializeField] public float PhysicalStrengthScale = 1;
    [SerializeField] public float NimblenessScale = 1;
    [SerializeField] public float BrillianceScale = 1;
    [SerializeField] public float HopeScale = 1;

    public int GetDamage(PlayerCombat Player)
    {
        int actualDamage = (int) ((VitalityScale * Player.Vitality) + (MentalityScale * Player.Mentality) + (FortitudeScale * Player.Fortitude) + (PhysicalStrengthScale * Player.PhysicalPower) + (NimblenessScale * Player.Nimbleness) + (BrillianceScale * Player.Brilliance) + (HopeScale * Player.Hope));
        return actualDamage;
    }

    public int GetDamage(EnemyCombat Enemy)
    {
        int actualDamage = (int) ((VitalityScale * Enemy.Vitality) + (MentalityScale * Enemy.Mentality) + (FortitudeScale * Enemy.Fortitude) + (PhysicalStrengthScale * Enemy.PhysicalPower) + (NimblenessScale * Enemy.Nimbleness) + (BrillianceScale * Enemy.Brilliance) + (HopeScale * Enemy.Hope));
        return actualDamage;
    }



}
