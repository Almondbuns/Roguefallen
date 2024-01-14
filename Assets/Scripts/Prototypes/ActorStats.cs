using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public enum DamageType
{
    UNKNOWN,
    SLASH,
    CRUSH,
    PIERCE,
    FIRE,
    ICE,
    LIGHTNING,
    MAGIC,
    DIVINE,
    DARK,
    DURABILITY,
    DISEASE,
    POISON,
    INSANITY
}

public enum ArmorType
{
    NONE,
    PHYSICAL,
    ELEMENTAL,
    MAGICAL
}

public enum DamageTypeResistances
{
    EXTREMELY_WEAK,
    VERY_WEAK,
    WEAK,
    NORMAL,
    RESISTANT,
    VERY_RESISTANT,
    EXTREMELY_RESISTANT
}

public class MeterResistances
{
    public Dictionary<DamageType, int> resistances;

    public MeterResistances()
    {
        resistances = new()
        {
            { DamageType.DISEASE, 0 },
            { DamageType.POISON, 0 },
            { DamageType.INSANITY, 0 },
        };
    }

    public int? GetResistance(DamageType type)
    {
        if (resistances.ContainsKey(type))
            return resistances[type];

        else return null;
    }

    public void SetResistance(DamageType type, int resistance)
    {
        if (resistances.ContainsKey(type))
            resistances[type] = resistance;

        else Debug.Log("Error: No meter resistance " + type.ToString());
    }

}

public class ProbabilityResistances
{
    public List<DamageTypeResistances> resistances;

    public ProbabilityResistances()
    {
        Array array = Enum.GetValues(typeof(DamageType));
        resistances = new List<DamageTypeResistances>(array.Length);
        foreach (DamageType type in array)
            resistances.Add(DamageTypeResistances.NORMAL);
    }

    public DamageTypeResistances GetResistance(DamageType type)
    {
        return resistances[(int) type];
    }

    public void SetResistance(DamageType type, DamageTypeResistances resistance)
    {
        resistances[(int)type] = resistance;
    }

    public static float GetDamageMultiplyer(DamageTypeResistances resistance_level)
    {
        if (resistance_level <= DamageTypeResistances.EXTREMELY_WEAK)
            return 2;
        if (resistance_level <= DamageTypeResistances.VERY_WEAK)
            return 1.50f;
        if (resistance_level <= DamageTypeResistances.WEAK)
            return 1.20f;
        if (resistance_level <= DamageTypeResistances.NORMAL)
            return 1;
        if (resistance_level <= DamageTypeResistances.RESISTANT)
            return 0.8f;
        if (resistance_level <= DamageTypeResistances.VERY_RESISTANT)
            return 0.6f;
        else
            return 0.4f;
    }

    public static float GetEffectProbability(DamageTypeResistances resistance_level)
    {
        if (resistance_level <= DamageTypeResistances.EXTREMELY_WEAK)
            return .95f;
        if (resistance_level <= DamageTypeResistances.VERY_WEAK)
            return .9f;
        if (resistance_level <= DamageTypeResistances.WEAK)
            return .8f;
        if (resistance_level <= DamageTypeResistances.NORMAL)
            return .7f;
        if (resistance_level <= DamageTypeResistances.RESISTANT)
            return 0.5f;
        if (resistance_level <= DamageTypeResistances.VERY_RESISTANT)
            return 0.3f;
        else
            return 0.1f;
    }
}

public class ArmorStats
{
    public string body_part;
    public int percentage;
    public (int physical, int elemental, int magical) armor;
    public int durability_max;
}

public class ActorStats 
{
    public int level = 1;
    public int kill_experience = 10;

    public int health_max;
    public int stamina_max;
    public int mana_max;

    public List<ArmorStats> body_armor;
    public int movement_time = 100;
    public int attack_time = 100;
    public int usage_time = 100;

    public int to_hit;
    public int dodge;
    public int stealth;

    public ProbabilityResistances probability_resistances;
    public MeterResistances meter_resistances;

    public ActorStats()
    {
        body_armor = new();
        probability_resistances = new();
        meter_resistances = new();
    }
}
