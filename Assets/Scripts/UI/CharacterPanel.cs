using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    PlayerData player_data;

    // Start is called before the first frame update
    void Start()
    {
        player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        
        Refresh();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {
        transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.prototype.name;
        transform.Find("Level").GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + player_data.player_stats.level;
        int experience_next_level = 0;
        if (player_data.player_stats.level - 1 < player_data.experience_levels.Count)
            experience_next_level = player_data.experience_levels[player_data.player_stats.level - 1];

        transform.Find("ExperienceBar").GetComponent<CurrentMaxBar>().SetValues(player_data.player_stats.experience, experience_next_level);
        transform.Find("HealthBar").GetComponent<CurrentMaxBar>().SetValues(player_data.Health_current, player_data.GetHealthMax());
        transform.Find("StaminaBar").GetComponent<CurrentMaxBar>().SetValues(player_data.Stamina_current, player_data.GetStaminaMax());
        transform.Find("ManaBar").GetComponent<CurrentMaxBar>().SetValues(player_data.mana_current, player_data.GetManaMax());

        transform.Find("BaseStats").Find("Strength").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.strength.ToString();
        transform.Find("BaseStats").Find("Strength").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetStrength().ToString();
        transform.Find("BaseStats").Find("Vitality").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.vitality.ToString();
        transform.Find("BaseStats").Find("Vitality").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetVitality().ToString();
        transform.Find("BaseStats").Find("Dexterity").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.dexterity.ToString();
        transform.Find("BaseStats").Find("Dexterity").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetDexterity().ToString();
        transform.Find("BaseStats").Find("Constitution").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.constitution.ToString();
        transform.Find("BaseStats").Find("Constitution").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetConstitution().ToString();
        transform.Find("BaseStats").Find("Intelligence").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.intelligence.ToString();
        transform.Find("BaseStats").Find("Intelligence").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetIntelligence().ToString();
        transform.Find("BaseStats").Find("Willpower").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.willpower.ToString();
        transform.Find("BaseStats").Find("Willpower").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetWillpower().ToString();
        transform.Find("BaseStats").Find("Points").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.attribute_points.ToString();

        transform.Find("DerivedAttributes").Find("ToHit").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetToHit().ToString();
        transform.Find("DerivedAttributes").Find("Dodge").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetDodge().ToString();
        transform.Find("DerivedAttributes").Find("MaxWeight").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetMaxWeight().ToString();

        transform.Find("DerivedAttributes").Find("MovementTime").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetMovementTime().ToString();
        transform.Find("DerivedAttributes").Find("AttackTime").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetAttackTime().ToString();
        transform.Find("DerivedAttributes").Find("UsageTime").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetUsageTime().ToString();

        int durability;
        transform.Find("Armor").Find("Chest").Find("Physical").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("chest", ArmorType.PHYSICAL).ToString();
        transform.Find("Armor").Find("Chest").Find("Elemental").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("chest", ArmorType.ELEMENTAL).ToString();
        transform.Find("Armor").Find("Chest").Find("Magical").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("chest", ArmorType.MAGICAL).ToString();
        durability = player_data.GetDurability("chest");
        transform.Find("Armor").Find("Chest").Find("DurabilityBar").GetComponent<CurrentMaxBar>().SetValues(durability, player_data.GetMaxDurability("chest"));
        transform.Find("Armor").Find("Head").Find("Physical").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("head", ArmorType.PHYSICAL).ToString();
        transform.Find("Armor").Find("Head").Find("Elemental").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("head", ArmorType.ELEMENTAL).ToString();
        transform.Find("Armor").Find("Head").Find("Magical").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("head", ArmorType.MAGICAL).ToString();
        durability = player_data.GetDurability("head");
        transform.Find("Armor").Find("Head").Find("DurabilityBar").GetComponent<CurrentMaxBar>().SetValues(durability, player_data.GetMaxDurability("head"));
        transform.Find("Armor").Find("Hands").Find("Physical").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("hands", ArmorType.PHYSICAL).ToString();
        transform.Find("Armor").Find("Hands").Find("Elemental").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("hands", ArmorType.ELEMENTAL).ToString();
        transform.Find("Armor").Find("Hands").Find("Magical").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("hands", ArmorType.MAGICAL).ToString();
        durability = player_data.GetDurability("hands");
        transform.Find("Armor").Find("Hands").Find("DurabilityBar").GetComponent<CurrentMaxBar>().SetValues(durability, player_data.GetMaxDurability("hands"));
        transform.Find("Armor").Find("Feet").Find("Physical").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("feet", ArmorType.PHYSICAL).ToString();
        transform.Find("Armor").Find("Feet").Find("Elemental").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("feet", ArmorType.ELEMENTAL).ToString();
        transform.Find("Armor").Find("Feet").Find("Magical").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetArmor("feet", ArmorType.MAGICAL).ToString();
        durability = player_data.GetDurability("feet");
        transform.Find("Armor").Find("Feet").Find("DurabilityBar").GetComponent<CurrentMaxBar>().SetValues(durability, player_data.GetMaxDurability("feet"));

        transform.Find("Meter Resistances").Find("Disease").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.meter_resistances.resistances[DamageType.DISEASE] + "/" + player_data.GetMaxDiseaseResistance().ToString();
        transform.Find("Meter Resistances").Find("Poison").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.meter_resistances.resistances[DamageType.POISON] + "/" + player_data.GetMaxPoisonResistance().ToString();
        transform.Find("Meter Resistances").Find("Insanity").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.meter_resistances.resistances[DamageType.INSANITY] + "/" + player_data.GetMaxInsanityResistance().ToString();

        transform.Find("General Resistances").Find("Slash").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = ((DamageTypeResistances) player_data.GetResistance(DamageType.SLASH)).ToString().Replace("_", " ").ToLower();
        transform.Find("General Resistances").Find("Slash").Find("Damage").GetComponent<TMPro.TextMeshProUGUI>().text = ProbabilityResistances.GetDamageMultiplyer((DamageTypeResistances) player_data.GetResistance(DamageType.SLASH)).ToString().Replace(",",".");
        transform.Find("General Resistances").Find("Slash").Find("Probability").GetComponent<TMPro.TextMeshProUGUI>().text = (100 * ProbabilityResistances.GetEffectProbability((DamageTypeResistances) player_data.GetResistance(DamageType.SLASH))).ToString() + "%";

        transform.Find("General Resistances").Find("Pierce").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = ((DamageTypeResistances) player_data.GetResistance(DamageType.PIERCE)).ToString().Replace("_", " ").ToLower();
        transform.Find("General Resistances").Find("Pierce").Find("Damage").GetComponent<TMPro.TextMeshProUGUI>().text = ProbabilityResistances.GetDamageMultiplyer((DamageTypeResistances) player_data.GetResistance(DamageType.PIERCE)).ToString().Replace(",",".");
        transform.Find("General Resistances").Find("Pierce").Find("Probability").GetComponent<TMPro.TextMeshProUGUI>().text = (100 * ProbabilityResistances.GetEffectProbability((DamageTypeResistances) player_data.GetResistance(DamageType.PIERCE))).ToString() + "%";

        transform.Find("General Resistances").Find("Crush").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = ((DamageTypeResistances) player_data.GetResistance(DamageType.CRUSH)).ToString().Replace("_", " ").ToLower();
        transform.Find("General Resistances").Find("Crush").Find("Damage").GetComponent<TMPro.TextMeshProUGUI>().text = ProbabilityResistances.GetDamageMultiplyer((DamageTypeResistances) player_data.GetResistance(DamageType.CRUSH)).ToString().Replace(",",".");
        transform.Find("General Resistances").Find("Crush").Find("Probability").GetComponent<TMPro.TextMeshProUGUI>().text = (100 * ProbabilityResistances.GetEffectProbability((DamageTypeResistances) player_data.GetResistance(DamageType.CRUSH))).ToString() + "%";

        transform.Find("General Resistances").Find("Fire").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = ((DamageTypeResistances) player_data.GetResistance(DamageType.FIRE)).ToString().Replace("_", " ").ToLower();
        transform.Find("General Resistances").Find("Fire").Find("Damage").GetComponent<TMPro.TextMeshProUGUI>().text = ProbabilityResistances.GetDamageMultiplyer((DamageTypeResistances) player_data.GetResistance(DamageType.FIRE)).ToString().Replace(",",".");
        transform.Find("General Resistances").Find("Fire").Find("Probability").GetComponent<TMPro.TextMeshProUGUI>().text = (100 * ProbabilityResistances.GetEffectProbability((DamageTypeResistances) player_data.GetResistance(DamageType.FIRE))).ToString() + "%";

        transform.Find("General Resistances").Find("Ice").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = ((DamageTypeResistances) player_data.GetResistance(DamageType.ICE)).ToString().Replace("_", " ").ToLower();
        transform.Find("General Resistances").Find("Ice").Find("Damage").GetComponent<TMPro.TextMeshProUGUI>().text = ProbabilityResistances.GetDamageMultiplyer((DamageTypeResistances) player_data.GetResistance(DamageType.ICE)).ToString().Replace(",",".");
        transform.Find("General Resistances").Find("Ice").Find("Probability").GetComponent<TMPro.TextMeshProUGUI>().text = (100 * ProbabilityResistances.GetEffectProbability((DamageTypeResistances) player_data.GetResistance(DamageType.ICE))).ToString() + "%";

        transform.Find("General Resistances").Find("Lightning").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = ((DamageTypeResistances) player_data.GetResistance(DamageType.LIGHTNING)).ToString().Replace("_", " ").ToLower();
        transform.Find("General Resistances").Find("Lightning").Find("Damage").GetComponent<TMPro.TextMeshProUGUI>().text = ProbabilityResistances.GetDamageMultiplyer((DamageTypeResistances) player_data.GetResistance(DamageType.LIGHTNING)).ToString().Replace(",",".");
        transform.Find("General Resistances").Find("Lightning").Find("Probability").GetComponent<TMPro.TextMeshProUGUI>().text = (100 * ProbabilityResistances.GetEffectProbability((DamageTypeResistances) player_data.GetResistance(DamageType.LIGHTNING))).ToString() + "%";

        transform.Find("General Resistances").Find("Magic").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = ((DamageTypeResistances) player_data.GetResistance(DamageType.MAGIC)).ToString().Replace("_", " ").ToLower();
        transform.Find("General Resistances").Find("Magic").Find("Damage").GetComponent<TMPro.TextMeshProUGUI>().text = ProbabilityResistances.GetDamageMultiplyer((DamageTypeResistances) player_data.GetResistance(DamageType.MAGIC)).ToString().Replace(",",".");
        transform.Find("General Resistances").Find("Magic").Find("Probability").GetComponent<TMPro.TextMeshProUGUI>().text = (100 * ProbabilityResistances.GetEffectProbability((DamageTypeResistances) player_data.GetResistance(DamageType.MAGIC))).ToString() + "%";

        transform.Find("General Resistances").Find("Divine").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = ((DamageTypeResistances) player_data.GetResistance(DamageType.DIVINE)).ToString().Replace("_", " ").ToLower();
        transform.Find("General Resistances").Find("Divine").Find("Damage").GetComponent<TMPro.TextMeshProUGUI>().text = ProbabilityResistances.GetDamageMultiplyer((DamageTypeResistances) player_data.GetResistance(DamageType.DIVINE)).ToString().Replace(",",".");
        transform.Find("General Resistances").Find("Divine").Find("Probability").GetComponent<TMPro.TextMeshProUGUI>().text = (100 * ProbabilityResistances.GetEffectProbability((DamageTypeResistances) player_data.GetResistance(DamageType.DIVINE))).ToString() + "%";

        transform.Find("General Resistances").Find("Dark").Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = ((DamageTypeResistances) player_data.GetResistance(DamageType.DARK)).ToString().Replace("_", " ").ToLower();
        transform.Find("General Resistances").Find("Dark").Find("Damage").GetComponent<TMPro.TextMeshProUGUI>().text = ProbabilityResistances.GetDamageMultiplyer((DamageTypeResistances) player_data.GetResistance(DamageType.DARK)).ToString().Replace(",",".");
        transform.Find("General Resistances").Find("Dark").Find("Probability").GetComponent<TMPro.TextMeshProUGUI>().text = (100 * ProbabilityResistances.GetEffectProbability((DamageTypeResistances) player_data.GetResistance(DamageType.DARK))).ToString() + "%";
    }

    public void AddStrength()
    {
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.strength += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }

    public void AddVitality()
    {
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.vitality += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }

    public void AddDexterity()
    {
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.dexterity += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }

    public void AddConstitution()
    {
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.constitution += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }

    public void AddItelligence()
    {
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.intelligence += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }

    public void AddWillpower()
    {
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.willpower += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }
}
