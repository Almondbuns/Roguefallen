using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemGold : ItemPrototype
{
    public ItemGold(int level) : base(level)
    {
        name = "Gold";
        icon = "images/objects/gold";
        type = ItemType.OTHER;
        gold_value = 1;
        is_stackable = true;

        weight = 0;
        start_amount_min = 5 * (level) + 1;
        start_amount_max = 5 * (level + 1) + 1;
    }
}

