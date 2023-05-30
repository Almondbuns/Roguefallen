using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChestHeavy : ItemPrototype
{
    public ItemChestHeavy(int level) : base(level)
    {
        name = "Breastplate";
        icon = "images/objects/armor_chest_heavy";
        type = ItemType.ARMOR_CHEST;
       
        weight = 10;
       
        effects_when_equipped.Add(new EffectAddMovementTime { amount = 50 });

        if (level <= 4)
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
        else if (level <= 8)
        {
            tier = 1;
            armor = new ArmorPrototype
            {
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 3,
                armor_elemental = 2,
                armor_magical = 0,
                durability_max = 600,
            };
            
            gold_value = 300;
            required_attributes.strength = 15;
        }
        else
        {
            tier = 2;
            armor = new ArmorPrototype
            {
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 4,
                armor_elemental = 3,
                armor_magical = 0,
                durability_max = 800,
            };
            
            gold_value = 500;
            required_attributes.strength = 25;
        }

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
                effect = new EffectAddToHit() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Agile",
                effect = new EffectAddDodge() { amount = 2*(tier+1)},
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

public class ItemBootsHeavy : ItemPrototype
{
    public ItemBootsHeavy(int level) : base(level)
    {
        name = "Heavy Boots";
        icon = "images/objects/armor_feet_heavy";
        type = ItemType.ARMOR_FEET;
           
        weight = 5;

        if (level <= 4)
        {
            tier = 0;
            armor = new ArmorPrototype
            {                
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 2,
                armor_elemental = 1,
                armor_magical = 0,
                durability_max = 50,
            };
            
            gold_value = 50;
            required_attributes.strength = 5;
        }
        else if (level <= 8)
        {
            tier = 1;
            armor = new ArmorPrototype
            {
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 3,
                armor_elemental = 2,
                armor_magical = 0,
                durability_max = 70,
            };
            
            gold_value = 150;
            required_attributes.strength = 15;
        }
        else
        {
            tier = 2;
            armor = new ArmorPrototype
            {
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 4,
                armor_elemental = 3,
                armor_magical = 0,
                durability_max = 90,
            };
            
            gold_value = 250;
            required_attributes.strength = 25;
        }

        effects_when_equipped.Add(new EffectAddMovementTime { amount = 15 });

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
                effect = new EffectAddArmorDurability() { amount = 50*(tier+1) },
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
                effect = new EffectAddToHit() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Agile",
                effect = new EffectAddDodge() { amount = 2*(tier+1)},
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

public class ItemHandsHeavy : ItemPrototype
{
    public ItemHandsHeavy(int level) : base(level)
    {
        name = "Gauntlets";
        icon = "images/objects/armor_hands_heavy";
        type = ItemType.ARMOR_HANDS;
            
        weight = 5;

        if (level <= 4)
        {
            tier = 0;
            armor = new ArmorPrototype
            {                
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 2,
                armor_elemental = 1,
                armor_magical = 0,
                durability_max = 50,
            };
            
            gold_value = 50;
            required_attributes.strength = 5;
        }
        else if (level <= 8)
        {
            tier = 1;
            armor = new ArmorPrototype
            {
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 3,
                armor_elemental = 2,
                armor_magical = 0,
                durability_max = 70,
            };
            
            gold_value = 150;
            required_attributes.strength = 15;
        }
        else
        {
            tier = 2;
            armor = new ArmorPrototype
            {
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 4,
                armor_elemental = 3,
                armor_magical = 0,
                durability_max = 90,
            };
            
            gold_value = 250;
            required_attributes.strength = 25;
        }

        effects_when_equipped.Add(new EffectAddMovementTime { amount = 15 });

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
                effect = new EffectAddArmorDurability() { amount = 50*(tier+1) },
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
                effect = new EffectAddToHit() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Agile",
                effect = new EffectAddDodge() { amount = 2*(tier+1)},
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

public class ItemHeadHeavy : ItemPrototype
{
    public ItemHeadHeavy(int level) : base(level)
    {
        name = "Heavy Helmet";
        icon = "images/objects/armor_head_heavy";
        type = ItemType.ARMOR_HEAD;
    
        weight = 5;

        if (level <= 4)
        {
            tier = 0;
            armor = new ArmorPrototype
            {                
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 2,
                armor_elemental = 1,
                armor_magical = 0,
                durability_max = 50,
            };
            
            gold_value = 50;
            required_attributes.strength = 5;
        }
        else if (level <= 8)
        {
            tier = 1;
            armor = new ArmorPrototype
            {
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 3,
                armor_elemental = 2,
                armor_magical = 0,
                durability_max = 70,
            };
            
            gold_value = 150;
            required_attributes.strength = 15;
        }
        else
        {
            tier = 2;
            armor = new ArmorPrototype
            {
                sub_type = ArmorSubType.HEAVY,
                armor_physical = 4,
                armor_elemental = 3,
                armor_magical = 0,
                durability_max = 90,
            };
            
            gold_value = 250;
            required_attributes.strength = 25;
        }

        effects_when_equipped.Add(new EffectAddMovementTime { amount = 15 });

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
                effect = new EffectAddArmorDurability() { amount = 50*(tier+1) },
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
                effect = new EffectAddToHit() { amount = 2*(tier+1) },
                trigger = ItemEffectTrigger.Equipped,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Agile",
                effect = new EffectAddDodge() { amount = 2*(tier+1)},
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
