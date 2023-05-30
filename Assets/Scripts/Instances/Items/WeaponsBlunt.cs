using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemHammer1H : ItemPrototype
{
    public ItemHammer1H(int level) : base(level)
    {
        name = "Hammer";
        type = ItemType.WEAPON;
        icon = "images/objects/weapon_hammer_1H";
        weight = 5;

        if (level <= 4)
        {
            tier = 0;
            gold_value = 100;
            required_attributes.strength = 5;
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 300;
            required_attributes.strength = 15;
        }
        else
        {
            tier = 2;
            gold_value = 500;
            required_attributes.strength = 25;
        }
        
        weapon = new WeaponPrototype
        {
            sub_type = WeaponSubType.BLUNT,
            equip_type = WeaponEquipType.ONEHANDED,
            damage = { (DamageType.CRUSH, 5 + 2*tier, 5 + 2*tier, 2) },
            attack_time = 125,
            attack_talents =
            {
                new TalentWeaponAttackStandard(),
                new TalentBluntHammerAttackHeavy(),
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

public class ItemMace1H : ItemPrototype
{
    public ItemMace1H(int level) : base(level)
    {
        name = "Mace";
        type = ItemType.WEAPON;
        icon = "images/objects/weapon_mace_1H";
        weight = 5;
        
        if (level <= 4)
        {
            tier = 0;
            gold_value = 100;
            required_attributes.strength = 5;
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 300;
            required_attributes.strength = 15;
        }
        else
        {
            tier = 2;
            gold_value = 500;
            required_attributes.strength = 25;
        }

        weapon = new WeaponPrototype
        {
            sub_type = WeaponSubType.BLUNT,
            equip_type = WeaponEquipType.ONEHANDED,
            damage = { (DamageType.CRUSH, 3 + 2*tier, 7 + 2*tier, 2) },
            attack_time = 125,
            attack_talents =
            {
                new TalentWeaponAttackStandard(),
                new TalentBluntMaceAttackHeavy(),
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

public class ItemFlail1H : ItemPrototype
{
    public ItemFlail1H(int level) : base(level)
    {
        name = "Flail";
        type = ItemType.WEAPON;
        icon = "images/objects/weapon_flail_1H";
        weight = 5;
        
        if (level <= 4)
        {
            tier = 0;
            gold_value = 100;
            required_attributes.strength = 5;
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 300;
            required_attributes.strength = 15;
        }
        else
        {
            tier = 2;
            gold_value = 500;
            required_attributes.strength = 25;
        }

        weapon = new WeaponPrototype
        {
            sub_type = WeaponSubType.BLUNT,
            equip_type = WeaponEquipType.ONEHANDED,
            attack_time = 125,
            damage = { (DamageType.CRUSH, 4 + 2*tier, 6 + 2*tier, 2) },
            attack_talents =
            {
                new TalentWeaponAttackStandard(),
                new TalentBluntFlailAttackHeavy(),
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


public class ItemHammer2H : ItemPrototype
{
    public ItemHammer2H(int level) : base(level)
    {
        name = "Hammer";
        type = ItemType.WEAPON;
        icon = "images/objects/weapon_hammer_2H";
        weight = 10;

        if (level <= 4)
        {
            tier = 0;
            gold_value = 100;
            required_attributes.strength = 10;
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 300;
            required_attributes.strength = 20;
        }
        else
        {
            tier = 2;
            gold_value = 500;
            required_attributes.strength = 30;
        }

        weapon = new WeaponPrototype
        {
            sub_type = WeaponSubType.BLUNT,
            equip_type = WeaponEquipType.TWOHANDED,
            damage = { (DamageType.CRUSH, 6 + 3*tier, 8 + 3*tier, 3) },
            attack_time = 175,
            attack_talents =
            {
                new TalentWeaponAttackStandard(),
                new TalentBluntHammer2HAttackHeavy(),
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