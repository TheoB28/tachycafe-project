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

    [Header("Level stats")]
    [SerializeField] public int Level;
    [SerializeField] public int Exp;
    [SerializeField] public int ExpToNextLevel;
    [SerializeField] public int SkillPoints;
    [SerializeField] public int Vitality;
    [SerializeField] public int Mentality;
    [SerializeField] public int Fortitude;
    [SerializeField] public int PhysicalPower;
    [SerializeField] public int Nimbleness;
    [SerializeField] public int Brilliance;
    [SerializeField] public int Hope;

    [Header("Stat scaling")]
    [SerializeField] public int VitalityScale1To20;
    [SerializeField] public int VitalityScale20To40;
    [SerializeField] public int VitalityScale40To60;
    [SerializeField] public int VitalityScale60To80;
    [SerializeField] public int VitalityScale80To100;

    [SerializeField] public int MentalityScale1To20;
    [SerializeField] public int MentalityScale20To40;
    [SerializeField] public int MentalityScale40To60;
    [SerializeField] public int MentalityScale60To80;
    [SerializeField] public int MentalityScale80To100;

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

    public void GiveXP(int amount)
    {
        Exp += amount;
        if (Exp >= ExpToNextLevel)
        {
            while (Exp >= ExpToNextLevel)
            {
                Level++;
                SkillPoints++;
                Exp -= ExpToNextLevel;
                ExpToNextLevel = (int)5 * Level ^ 2;
                Debug.Log("Level Up! " + PlayerName + " is now level " + Level);
            }

        }

    }

    public void UppdateStats()
    {
        if(Vitality <= 20) { MaxHP = Vitality * VitalityScale1To20; }
        else if(Vitality <= 40) { MaxHP = VitalityScale1To20 * 20 + (Vitality -20) * VitalityScale20To40; }
        else if(Vitality <= 60) { MaxHP = VitalityScale1To20 * 20 + VitalityScale20To40 * 20 + (Vitality -40) * VitalityScale40To60; }
        else if(Vitality <= 80) { MaxHP = VitalityScale1To20 * 20 + VitalityScale20To40 * 20 + VitalityScale40To60 * 20 + (Vitality -60) * VitalityScale60To80; }
        else if(Vitality <= 100) { MaxHP = VitalityScale1To20 * 20 + VitalityScale20To40 * 20 + VitalityScale40To60 * 20 + VitalityScale60To80 * 20 + (Vitality -80) * VitalityScale80To100; }

        if(Mentality <= 20) { MaxFP = Mentality * MentalityScale1To20; }
        else if(Mentality <= 40) { MaxFP = MentalityScale1To20 * 20 + (Mentality -20) * MentalityScale20To40; }
        else if(Mentality <= 60) { MaxFP = MentalityScale1To20 * 20 + MentalityScale20To40 * 20 + (Mentality -40) * MentalityScale40To60; }
        else if(Mentality <= 80) { MaxFP = MentalityScale1To20 * 20 + MentalityScale20To40 * 20 + MentalityScale40To60 * 20 + (Mentality -60) * MentalityScale60To80; }
        else if(Mentality <= 100) { MaxFP = MentalityScale1To20 * 20 + MentalityScale20To40 * 20 + MentalityScale40To60 * 20 + MentalityScale60To80 * 20 + (Mentality -80) * MentalityScale80To100; }
        HP = MaxHP;
        FP = MaxFP;
        
    }
}
