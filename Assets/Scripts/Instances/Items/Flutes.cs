using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFluteOfHealing : ItemPrototype
{
    public ItemFluteOfHealing(int level) : base(level)
    {
        name = "Flute of Healing";
        icon = "images/objects/flute";
        type = ItemType.OTHER;

        if (level <= 7)
            tier = 0;
        else
            tier = 1;

        gold_value = 500;
        weight = 3;
        usable_item = new()
        {
            max_number_of_uses = 2 + tier,
            effects_when_used =
            {
                new EffectAddHitpoints { amount = 40 + 10 * tier, duration = 0},
            },
        };
    }
}

public class ItemFluteOfEndurance : ItemPrototype
{
    public ItemFluteOfEndurance(int level) : base(level)
    {
        name = "Flute of Endurance";
        icon = "images/objects/flute";
        type = ItemType.OTHER;

        if (level <= 7)
            tier = 0;
        else
            tier = 1;

        gold_value = 500;
        weight = 3;
        usable_item = new()
        {
            max_number_of_uses = 2 + tier,
            effects_when_used =
            {
                new EffectAddStamina { amount = 40 + 10 * tier, duration = 0},
            },
        };
    }
}

public class ItemFluteOfBraveness : ItemPrototype
{
    public ItemFluteOfBraveness(int level) : base(level)
    {
        name = "Flute of Braveness";
        icon = "images/objects/flute";
        type = ItemType.OTHER;

        if (level <= 7)
            tier = 0;
        else
            tier = 1;

        gold_value = 500;
        weight = 3;
        usable_item = new()
        {
            max_number_of_uses = 2 + tier,
            effects_when_used =
            {
                new EffectAddToHit { amount = 10 + 5 * tier, duration = 2000},
                new EffectAddDodge { amount = 10 + 5 * tier, duration = 2000},
            },
        };
    }
}