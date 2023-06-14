using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemHandAxe1H : ItemPrototype
{
    public ItemHandAxe1H(int level) : base(level)
    {
        name = "Hand Axe";
        type = ItemType.WEAPON;
        icon = "images/objects/weapon_axe_1H";
        weight = 5;

        if (level <= 4)
        {
            tier = 0;
            gold_value = 100;
            required_attributes.strength = 5;
            required_attributes.dexterity = 5;
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 300;
            required_attributes.strength = 10;
            required_attributes.dexterity = 10;
        }
        else
        {
            tier = 2;
            gold_value = 500;
            required_attributes.strength = 15;
            required_attributes.dexterity = 15;
        }
        
        weapon = new WeaponPrototype
        {
            sub_type = WeaponSubType.AXE,
            equip_type = WeaponEquipType.ONEHANDED,
            damage = { (DamageType.SLASH, 3 + 2*tier, 5 + 2*tier, 1) },
            attack_time = 110,
            attack_talents =
            {
                new TalentWeaponAttackStandard(){icon = "images/talents/axe_standard"},
                new TalentAxeWarAxeAttackHeavy(),
            }
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
                effect = new EffectAddAttackTime() { amount = -10 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
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
                name_presuffix = "Quality",
                effect = new EffectAddMinWeaponDamage() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Sharp",
                effect = new EffectAddMaxWeaponDamage() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Penetrating",
                effect = new EffectAddWeaponPenetration() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Fire",
                effect = new EffectAddWeaponFireDamage() { amount = 2 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Ice",
                effect = new EffectAddWeaponIceDamage() { amount = 2},
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Lightning",
                effect = new EffectAddWeaponLightningDamage() { amount = 2},
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
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
        };
    }
}

public class ItemDoubleAxe1H : ItemPrototype
{
    public ItemDoubleAxe1H(int level) : base(level)
    {
        name = "Double Axe";
        type = ItemType.WEAPON;
        icon = "images/objects/axe_double";
        weight = 5;

        if (level <= 4)
        {
            tier = 0;
            gold_value = 100;
            required_attributes.strength = 5;
            required_attributes.dexterity = 5;
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 300;
            required_attributes.strength = 10;
            required_attributes.dexterity = 10;
        }
        else
        {
            tier = 2;
            gold_value = 500;
            required_attributes.strength = 15;
            required_attributes.dexterity = 15;
        }
        
        weapon = new WeaponPrototype
        {
            sub_type = WeaponSubType.AXE,
            equip_type = WeaponEquipType.ONEHANDED,
            damage = { (DamageType.SLASH, 2 + 2*tier, 6 + 2*tier, 1) },
            attack_time = 110,
            attack_talents =
            {
                new TalentWeaponAttackStandard(){icon = "images/talents/axe_standard"},
                new TalentAxeDoubleAxeAttackHeavy(),
            }
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
                effect = new EffectAddAttackTime() { amount = -10 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
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
                name_presuffix = "Quality",
                effect = new EffectAddMinWeaponDamage() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Sharp",
                effect = new EffectAddMaxWeaponDamage() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Penetrating",
                effect = new EffectAddWeaponPenetration() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Fire",
                effect = new EffectAddWeaponFireDamage() { amount = 2 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Ice",
                effect = new EffectAddWeaponIceDamage() { amount = 2},
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Lightning",
                effect = new EffectAddWeaponLightningDamage() { amount = 2},
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
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
        };
    }
}

public class ItemPickaxe1H : ItemPrototype
{
    public ItemPickaxe1H(int level) : base(level)
    {
        name = "Pickaxe";
        type = ItemType.WEAPON;
        icon = "images/objects/axe_pick";
        weight = 5;

        if (level <= 4)
        {
            tier = 0;
            gold_value = 100;
            required_attributes.strength = 5;
            required_attributes.dexterity = 5;
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 300;
            required_attributes.strength = 10;
            required_attributes.dexterity = 10;
        }
        else
        {
            tier = 2;
            gold_value = 500;
            required_attributes.strength = 15;
            required_attributes.dexterity = 15;
        }
        
        weapon = new WeaponPrototype
        {
            sub_type = WeaponSubType.AXE,
            equip_type = WeaponEquipType.ONEHANDED,
            damage = { (DamageType.SLASH, 4 + 2*tier, 4 + 2*tier, 1) },
            attack_time = 110,
            attack_talents =
            {
                new TalentWeaponAttackStandard(){icon = "images/talents/axe_standard"},
                new TalentAxePickaxeAttackHeavy(),
            }
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
                effect = new EffectAddAttackTime() { amount = -10 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
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
                name_presuffix = "Quality",
                effect = new EffectAddMinWeaponDamage() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Sharp",
                effect = new EffectAddMaxWeaponDamage() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Penetrating",
                effect = new EffectAddWeaponPenetration() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Fire",
                effect = new EffectAddWeaponFireDamage() { amount = 2 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Ice",
                effect = new EffectAddWeaponIceDamage() { amount = 2},
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Lightning",
                effect = new EffectAddWeaponLightningDamage() { amount = 2},
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
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
        };
    }
}

public class ItemBattleAxe2H : ItemPrototype
{
    public ItemBattleAxe2H(int level) : base(level)
    {
        name = "Battle Axe";
        type = ItemType.WEAPON;
        icon = "images/objects/weapon_axe_2H";
        weight = 10;

        if (level <= 4)
        {
            tier = 0;
            gold_value = 100;
            required_attributes.strength = 10;
            required_attributes.dexterity = 10;
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 300;
            required_attributes.strength = 15;
            required_attributes.dexterity = 15;
        }
        else
        {
            tier = 2;
            gold_value = 500;
            required_attributes.strength = 20;
            required_attributes.dexterity = 20;
        }
        
        weapon = new WeaponPrototype
        {
            sub_type = WeaponSubType.AXE,
            equip_type = WeaponEquipType.ONEHANDED,
            damage = { (DamageType.SLASH, 5 + 2*tier, 7 + 2*tier, 2) },
            attack_time = 125,
            attack_talents =
            {
                new TalentWeaponAttackStandard(){icon = "images/talents/axe_standard"},
                new TalentAxeBattleAxeAttackHeavy(),
            }
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
                effect = new EffectAddAttackTime() { amount = -10 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
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
                name_presuffix = "Quality",
                effect = new EffectAddMinWeaponDamage() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Sharp",
                effect = new EffectAddMaxWeaponDamage() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                type = ItemEffectType.Prefix,
                name_presuffix = "Penetrating",
                effect = new EffectAddWeaponPenetration() { amount = 1 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Fire",
                effect = new EffectAddWeaponFireDamage() { amount = 2 },
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Ice",
                effect = new EffectAddWeaponIceDamage() { amount = 2},
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
            },

            new ItemEffectData()
            {
                exclude_index = 7,
                type = ItemEffectType.Prefix,
                name_presuffix = "Lightning",
                effect = new EffectAddWeaponLightningDamage() { amount = 2},
                trigger = ItemEffectTrigger.Equipped,
                target = ItemEffectTarget.Item,
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
        };
    }
}
