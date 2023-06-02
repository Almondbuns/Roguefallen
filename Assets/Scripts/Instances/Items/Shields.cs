using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShieldHeavy : ItemPrototype
{
    public ItemShieldHeavy(int level) : base(level)
    {
        name = "Heavy Shield";
        icon = "images/objects/shield_heavy";
        type = ItemType.SHIELD;
        weight = 5;
        
        effects_when_equipped.Add(new EffectAddMovementTime { amount = 25 });
        effects_when_equipped.Add(new EffectAddAttackTime { amount = 25 });

        if (level <= 20)
        {
            tier = 0;
            armor = new ArmorPrototype
            {                
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 2,
                armor_elemental = 1,
                armor_magical = 0,
                durability_max = 400,
            };
            
            gold_value = 100;
            required_attributes.strength = 5;
        }

        shield = new()
        {
            sub_type = ShieldSubType.HEAVY, 
            talents =
            {
                new TalentShieldBash(),
            },
        };

        if (level % 4 == 1)
            qualities_possible = new() { ItemQuality.Normal};
        else if (level % 4 == 2)
            qualities_possible = new() { ItemQuality.Normal, ItemQuality.Magical1};
        else if (level % 4 == 3)
            qualities_possible = new() { ItemQuality.Normal, ItemQuality.Magical1, ItemQuality.Magical2};
        else
           qualities_possible = new() { ItemQuality.Normal, ItemQuality.Magical1, ItemQuality.Magical2, ItemQuality.Unique };
        
        quality_effects_possible = new()
        {
            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Physical",
                effect = new EffectAddArmorPhysical() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Elemental",
                effect = new EffectAddArmorElemental() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {                
                type = ItemEffectType.Prefix,
                name_presuffix = "Magical",
                effect = new EffectAddArmorMagical() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Suffix,
                name_presuffix = "of Durability",
                effect = new EffectAddArmorDurability() { amount = 200*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 1,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Strength",
                effect = new EffectAddStrength() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 1,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Strength",
                effect = new EffectAddStrength() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 2,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Dexterity",
                effect = new EffectAddDexterity() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 2,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Dexterity",
                effect = new EffectAddDexterity() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 3,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Vitality",
                effect = new EffectAddVitality() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 3,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Vitality",
                effect = new EffectAddVitality() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 4,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Intelligence",
                effect = new EffectAddIntelligence() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 4,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Intelligence",
                effect = new EffectAddIntelligence() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 5,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Willpower",
                effect = new EffectAddWillpower() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 5,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Willpower",
                effect = new EffectAddWillpower() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 6,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Constitution",
                effect = new EffectAddConstitution() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 6,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Constitution",
                effect = new EffectAddConstitution() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Vaccinated",
                effect = new EffectAddMaxDiseaseResistance() { amount = 5*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Curing",
                effect = new EffectAddMaxPoisonResistance() { amount = 5*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Calming",
                effect = new EffectAddMaxInsanityResistance() { amount = 5*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },
        };
    }
}

public class ItemShieldMedium : ItemPrototype
{
    public ItemShieldMedium(int level) : base(level)
    {
        name = "Medium Shield";
        icon = "images/objects/shield_medium";
        type = ItemType.SHIELD;
        weight = 5;
        
        effects_when_equipped.Add(new EffectAddMovementTime { amount = 15 });
        effects_when_equipped.Add(new EffectAddAttackTime { amount = 15 });

        if (level <= 20)
        {
            tier = 0;
            armor = new ArmorPrototype
            {                
                sub_type = ArmorSubType.MEDIUM,
                armor_physical = 1,
                armor_elemental = 2,
                armor_magical = 1,
                durability_max = 400,
            };
            
            gold_value = 100;
            required_attributes.strength = 5;
        }

        shield = new()
        {
            sub_type = ShieldSubType.MEDIUM, 
            talents =
            {
                new TalentShieldParry(),
            },
        };

           if (level % 4 == 1)
            qualities_possible = new() { ItemQuality.Normal};
        else if (level % 4 == 2)
            qualities_possible = new() { ItemQuality.Normal, ItemQuality.Magical1};
        else if (level % 4 == 3)
            qualities_possible = new() { ItemQuality.Normal, ItemQuality.Magical1, ItemQuality.Magical2};
        else
           qualities_possible = new() { ItemQuality.Normal, ItemQuality.Magical1, ItemQuality.Magical2, ItemQuality.Unique };
        
        quality_effects_possible = new()
        {
            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Physical",
                effect = new EffectAddArmorPhysical() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Elemental",
                effect = new EffectAddArmorElemental() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {                
                type = ItemEffectType.Prefix,
                name_presuffix = "Magical",
                effect = new EffectAddArmorMagical() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Suffix,
                name_presuffix = "of Durability",
                effect = new EffectAddArmorDurability() { amount = 200*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 1,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Strength",
                effect = new EffectAddStrength() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 1,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Strength",
                effect = new EffectAddStrength() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 2,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Dexterity",
                effect = new EffectAddDexterity() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 2,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Dexterity",
                effect = new EffectAddDexterity() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 3,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Vitality",
                effect = new EffectAddVitality() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 3,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Vitality",
                effect = new EffectAddVitality() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 4,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Intelligence",
                effect = new EffectAddIntelligence() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 4,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Intelligence",
                effect = new EffectAddIntelligence() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 5,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Willpower",
                effect = new EffectAddWillpower() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 5,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Willpower",
                effect = new EffectAddWillpower() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 6,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Constitution",
                effect = new EffectAddConstitution() { amount = (tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                exclude_index = 6,
                type = ItemEffectType.Suffix,
                name_presuffix = "of Constitution",
                effect = new EffectAddConstitution() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Vaccinated",
                effect = new EffectAddMaxDiseaseResistance() { amount = 5*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Curing",
                effect = new EffectAddMaxPoisonResistance() { amount = 5*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Calming",
                effect = new EffectAddMaxInsanityResistance() { amount = 5*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },
        };
    }
}

