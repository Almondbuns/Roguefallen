using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGuineaPig : ItemPrototype
{
    public ItemGuineaPig(int level) : base(level)
    {
        name = "Guinea Pig";
        icon = "images/objects/acid_flask";
        type = ItemType.QUEST_ITEM;
        gold_value = 0;
        is_stackable = false;
        weight = 1;
    }
}

public class FamilySymbol : ItemPrototype
{
    public FamilySymbol(int level) : base(level)
    {
        name = "Family Symbol";
        icon = "images/objects/acid_flask";
        type = ItemType.QUEST_ITEM;
        gold_value = 1000;
        is_stackable = false;
        weight = 0;
    }
}