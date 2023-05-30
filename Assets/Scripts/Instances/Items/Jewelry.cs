using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRing : ItemPrototype
{
    public ItemRing(int level) : base(level)
    {
        name = "Ring";
        icon = "images/objects/ring";
        type = ItemType.RING;
       
        weight = 1;
       
        if (level <= 4)
        {
            tier = 0;
            gold_value = 100;
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 300;
        }
        else
        {
            tier = 2;
            gold_value = 500;
        }

        if (level % 4 == 1 || level % 4 == 2)
            qualities_possible = new() { ItemQuality.Magical1};
        else if (level % 4 == 3)
            qualities_possible = new() { ItemQuality.Magical1, ItemQuality.Magical2};
        else
           qualities_possible = new() { ItemQuality.Magical1, ItemQuality.Magical2, ItemQuality.Unique };
        
        quality_effects_possible = new()
        {
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
                type = ItemEffectType.Prefix,
                name_presuffix = "Quick",
                effect = new EffectAddMovementTime() { amount = -10 },
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
                name_presuffix = "Accurate",
                effect = new EffectAddToHit() { amount = 4*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Agile",
                effect = new EffectAddDodge() { amount = 4*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Vaccinated",
                effect = new EffectAddMaxDiseaseResistance() { amount = 10*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Curing",
                effect = new EffectAddMaxPoisonResistance() { amount = 10*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Calming",
                effect = new EffectAddMaxInsanityResistance() { amount = 10*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Magical",
                effect = new EffectAddArmorMagical() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
            },
        };
    }
}

public class ItemAmulet : ItemPrototype
{
    public ItemAmulet(int level) : base(level)
    {
        name = "Amulet";
        icon = "images/objects/amulet";
        type = ItemType.AMULET;
       
        weight = 1;
       
        if (level <= 4)
        {
            tier = 0;
            gold_value = 100;
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 300;
        }
        else
        {
            tier = 2;
            gold_value = 500;
        }

        if (level % 4 == 1 || level % 4 == 2)
            qualities_possible = new() { ItemQuality.Magical1};
        else if (level % 4 == 3)
            qualities_possible = new() { ItemQuality.Magical1, ItemQuality.Magical2};
        else
           qualities_possible = new() { ItemQuality.Magical1, ItemQuality.Magical2, ItemQuality.Unique };
        
        quality_effects_possible = new()
        {
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
                type = ItemEffectType.Prefix,
                name_presuffix = "Quick",
                effect = new EffectAddMovementTime() { amount = -10 },
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
                name_presuffix = "Accurate",
                effect = new EffectAddToHit() { amount = 4*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Agile",
                effect = new EffectAddDodge() { amount = 4*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Vaccinated",
                effect = new EffectAddMaxDiseaseResistance() { amount = 10*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Curing",
                effect = new EffectAddMaxPoisonResistance() { amount = 10*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Calming",
                effect = new EffectAddMaxInsanityResistance() { amount = 10*(tier+1)},
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Magical",
                effect = new EffectAddArmorMagical() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
            },
        };
    }
}