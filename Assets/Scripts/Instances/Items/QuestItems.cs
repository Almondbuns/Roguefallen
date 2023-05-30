using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGuineaPig : ItemPrototype
{
    public ItemGuineaPig(int level) : base(level)
    {
        name = "Guinea Pig";
        icon = "images/objects/acid_flask";
        type = ItemType.OTHER;
        gold_value = 0;
        is_stackable = false;
        weight = 1;
    }
}