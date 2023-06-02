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
        
        effects_when_equipped.Add(new EffectAddMovementTime { amount = 50 });

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
                new TalentShieldBash()
                {
                    
                },
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
        
        effects_when_equipped.Add(new EffectAddMovementTime { amount = 50 });

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
                new TalentShieldParry()
                {
                    
                },
            },
        };
    }
}

