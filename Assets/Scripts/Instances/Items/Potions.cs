using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealthPotion : ItemPrototype
{
    public ItemHealthPotion(int level) : base(level)
    {
        name = "Health Potion";
        icon = "images/objects/potion_health_instant";
        type = ItemType.CONSUMABLE;
        is_stackable = true;
        stack_max = 5;
        weight = 1;

        if (level <= 4)
        {
            tier = 0;
            gold_value = 25;
            effects_when_consumed.Add(new EffectAddHitpoints { amount = 20, duration = 0});
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 50;
            effects_when_consumed.Add(new EffectAddHitpoints { amount = 30, duration = 0});
        }
        else
        {
            tier = 2;
            gold_value = 75;
            effects_when_consumed.Add(new EffectAddHitpoints { amount = 40, duration = 0});
        }
    }
}

public class ItemStaminaPotion : ItemPrototype
{
    public ItemStaminaPotion(int level) : base(level)
    {
        name = "Stamina Potion";
        icon = "images/objects/potion_stamina_instant";
        type = ItemType.CONSUMABLE;
        is_stackable = true;
        stack_max = 5;
        weight = 1;

        if (level <= 4)
        {
            tier = 0;
            gold_value = 20;
            effects_when_consumed.Add(new EffectAddStamina { amount = 20, duration = 0});
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 40;
            effects_when_consumed.Add(new EffectAddStamina { amount = 30, duration = 0});
        }
        else
        {
            tier = 2;
            gold_value = 65;
            effects_when_consumed.Add(new EffectAddStamina { amount = 40, duration = 0});
        }
    }
}

public class ItemManaPotion : ItemPrototype
{
    public ItemManaPotion(int level) : base(level)
    {
        name = "Mana Potion";
        icon = "images/objects/potion_mana_instant";
        type = ItemType.CONSUMABLE;
        is_stackable = true;
        stack_max = 5;
        gold_value = 20;
        weight = 1;
        effects_when_consumed.Add(new EffectAddMana { amount = 20, duration = 0 });
    }
}

public class ItemAlmondBun : ItemPrototype
{
    public ItemAlmondBun(int level) : base(level)
    {
        name = "Almond Bun";
        icon = "images/objects/almond_bun";
        type = ItemType.CONSUMABLE;
        is_stackable = true;
        stack_max = 5;
        weight = 1;

        if (level <= 4)
        {
            tier = 0;
            gold_value = 25;
            effects_when_consumed.Add(new EffectAddHitpoints { amount = 3, duration = 1000, execution_time = EffectDataExecutionTime.CONTINUOUS });
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 50;
            effects_when_consumed.Add(new EffectAddHitpoints { amount = 3, duration = 1500, execution_time = EffectDataExecutionTime.CONTINUOUS });
        }
        else
        {
            tier = 2;
            gold_value = 75;
            effects_when_consumed.Add(new EffectAddHitpoints { amount = 3, duration = 2000, execution_time = EffectDataExecutionTime.CONTINUOUS });
        }
        
    }
}

public class ItemMeatHorn : ItemPrototype
{
    public ItemMeatHorn(int level) : base(level)
    {
        name = "Meat Horn";
        icon = "images/objects/meat_horn";
        type = ItemType.CONSUMABLE;
        is_stackable = true;
        stack_max = 5;
        gold_value = 20;
        weight = 1;
        
        if (level <= 4)
        {
            tier = 0;
            gold_value = 25;
            effects_when_consumed.Add(new EffectAddStamina { amount = 3, duration = 1000, execution_time = EffectDataExecutionTime.CONTINUOUS });
        }
        else if (level <= 8)
        {
            tier = 1;
            gold_value = 50;
            effects_when_consumed.Add(new EffectAddStamina{ amount = 3, duration = 1500, execution_time = EffectDataExecutionTime.CONTINUOUS });
        }
        else
        {
            tier = 2;
            gold_value = 75;
            effects_when_consumed.Add(new EffectAddStamina{ amount = 3, duration = 2000, execution_time = EffectDataExecutionTime.CONTINUOUS });
        }
    }
}

public class ItemBlueberries : ItemPrototype
{
    public ItemBlueberries(int level) : base(level)
    {
        name = "Blueberries";
        icon = "images/objects/blueberries";
        type = ItemType.CONSUMABLE;
        is_stackable = true;
        stack_max = 5;
        gold_value = 20;
        weight = 1;
        effects_when_consumed.Add(new EffectAddMana { amount = 3, duration = 1000, execution_time = EffectDataExecutionTime.CONTINUOUS });
    }
}

public class ItemRepairPowder: ItemPrototype
{
    public ItemRepairPowder(int level) : base(level)
    {
        name = "Repair Powder";
        icon = "images/objects/repair_powder";
        type = ItemType.CONSUMABLE;
        is_stackable = true;
        stack_max = 5;
        gold_value = 100;
        weight = 1;
        talents_when_consumed.Add(new ItemTalentRepairItems());
    }
}

public class ItemCamomileTea: ItemPrototype
{
    public ItemCamomileTea(int level) : base(level)
    {
        name = "Camomile Tea";
        icon = "images/objects/tea_red";
        type = ItemType.CONSUMABLE;
        is_stackable = false;
        gold_value = 100;
        weight = 1;
        talents_when_consumed.Add(new ItemTalentCureDiseases());
    }
}

public class ItemPeppermintTea: ItemPrototype
{
    public ItemPeppermintTea(int level) : base(level)
    {
        name = "Peppermint Tea";
        icon = "images/objects/tea_green";
        type = ItemType.CONSUMABLE;
        is_stackable = false;
        gold_value = 100;
        weight = 1;
        talents_when_consumed.Add(new ItemTalentCurePoisons());
    }
}

public class ItemStrawberryTea: ItemPrototype
{
    public ItemStrawberryTea(int level) : base(level)
    {
        name = "Strawberry Tea";
        icon = "images/objects/tea_blue";
        type = ItemType.CONSUMABLE;
        is_stackable = false;
        gold_value = 100;
        weight = 1;
        talents_when_consumed.Add(new ItemTalentCureInsanity());
    }
}