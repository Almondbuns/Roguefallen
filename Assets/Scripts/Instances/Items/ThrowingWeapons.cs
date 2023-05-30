using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFirebomb : ItemPrototype
{
    public ItemFirebomb(int level) : base(level)
    {
        name = "Firebomb";
        icon = "images/objects/firebomb";
        type = ItemType.CONSUMABLE;
        is_stackable = true;
        stack_max = 10;
        weight = 1;

        if (level <= 4)
            tier = 0;
        else tier = 1;

        gold_value = 10 * (tier+1);

        talents_when_consumed.Add(new TalentThrowFirebomb(level));
        start_amount_min = 2;
        start_amount_max = 4;
    }
}

public class ItemThrowingKnife : ItemPrototype
{
    public ItemThrowingKnife(int level) : base(level)
    {
        name = "Throwing Knife";
        icon = "images/objects/throwing_knife";
        type = ItemType.CONSUMABLE;        
        is_stackable = true;
        stack_max = 10;
        weight = 1;

        if (level <= 4)
            tier = 0;
        else tier = 1;

        gold_value = 10 * (tier+1);

        talents_when_consumed.Add(new TalentItemThrowThrowingKnife(level));

        start_amount_min = 2;
        start_amount_max = 5;
    }
}

public class ItemAcidFlask : ItemPrototype
{
    public ItemAcidFlask(int level) : base(level)
    {
        name = "Acid Flask";
        icon = "images/objects/acid_flask";
        type = ItemType.CONSUMABLE;
        is_stackable = true;
        stack_max = 10;
        weight = 1;

        if (level <= 4)
            tier = 0;
        else tier = 1;

        gold_value = 10 * (tier+1);

        talents_when_consumed.Add(new TalentThrowAcidFlask(level));
        start_amount_min = 2;
        start_amount_max = 4;
    }
}
