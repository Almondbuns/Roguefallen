using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemWarAxe1H : ItemPrototype
{
    public ItemWarAxe1H(int level) : base(level)
    {
        name = "War Axe";
        type = ItemType.WEAPON;
        icon = "images/objects/weapon_axe_1H";
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
            sub_type = WeaponSubType.AXE,
            equip_type = WeaponEquipType.ONEHANDED,
            damage = { (DamageType.SLASH, 4 + 2*tier, 4 + 2*tier, 1) },
            attack_time = 110,
            attack_talents =
            {
                new TalentWeaponAttackStandard(),
                //new TalentBluntHammerAttackHeavy(),
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

